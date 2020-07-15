using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework
{
    public class TimeTrigger : IHostedService
    {
        private IServiceProvider m_provider;
        private Timer m_timer;
        private HashSet<String> m_ids = new HashSet<string>();
        public TimeTrigger(IServiceProvider provider)
        {
            m_provider = provider;
            m_timer = new Timer(Register, null, Timeout.Infinite, Timeout.Infinite);
        }
        /// <summary>
        /// 在上个定时任务执行完毕后,隔1分钟在重新发起一起定时任务
        /// 每次允许2个线程执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            m_timer.Change(0, Timeout.Infinite);
            InitAction?.Invoke();
            return Task.CompletedTask;
        }
        public static event Action InitAction;
        public static event Action EndAction;
        /// <summary>
        /// 定时任务执行程序
        /// </summary>
        /// <param name="state"></param>
        public void Register(Object state)
        {
            Int32 num = 2;
            DateTime date = DateTime.Now;
            Queue<ITriggerExecute> all = TriggerStack.GetAll();
            Int32 count = all.Count;
            while (Volatile.Read(ref count) > 0)
            {
                try
                {
                    if (all.Count == 0)
                    {
                        //超时直接丢弃
                        Thread.Sleep(10);
                        continue;
                    }
                    //开启线程超过2个,那么就等10ms重新循环,避免始终执行线程操作
                    //注意,这里会出现如果队列只剩2个数据时候,会在第二次执行
                    //完毕后,有一个线程出现死锁,那么会出现死锁状态丢弃状态
                    Interlocked.Decrement(ref num);
                    var item = all.Dequeue();
                    date = DateTime.Now.AddSeconds(10);
                    if (!TryAdd(item))
                    {
                        continue;
                    }
                    Task.Run(() =>
                    {
                        ExecuteTrigger(item);
                        Interlocked.Decrement(ref count);
                        if (Volatile.Read(ref num) < 2)
                        {
                            Interlocked.Increment(ref num);
                        }
                    });
                    //给线程一个超时时间,如果当前线程超时1分钟,
                    //那么会被丢弃并且记录日志
                    //如果所有线程执行完毕后,就继续执行下一笔数据
                    //Warn,如果所有的数据都丢失,那么会产生大量的后台线程执行
                    while (Volatile.Read(ref num) <= 0)
                    {
                        if (DateTime.Now < date)
                        {
                            Thread.Sleep(10);
                        }
                        else
                        {
                            Volatile.Write(ref num, 1);
                            LogHelper.Critical($"TimeTrigger执行出现10s超时异常:{m_ids.Aggregate((i1, i2) => $"{i1}-{i2}")}");
                        }
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Warn($"定时任务出现错误{e.Message}");
                    Interlocked.Decrement(ref count);
                }
            }
            RemoveAll();
            m_timer.Change(1000 * 1, Timeout.Infinite);
        }
        private void ExecuteTrigger(ITriggerExecute trigger)
        {
            try
            {
                if (!trigger.Execute(m_provider))
                {
                    TriggerStack.PushExiested(trigger);
                }
                else
                {
                    TriggerStack.Remove(trigger);
                }
            }
            catch (Exception)
            {
                TriggerStack.PushExiested(trigger);
            }
            finally
            {
                String id = trigger.GetId();
                Remove(id);
            }
        }
        private Object m_lock = new object();
        private void Remove(String id)
        {
            lock (m_lock)
            {
                if (String.IsNullOrWhiteSpace(id)) return;
                if (m_ids.Contains(id))
                {
                    m_ids.Remove(id);
                }
            }
        }
        private void RemoveAll()
        {
            lock (m_lock)
            {
                m_ids.Clear();
            }
        }
        private Boolean TryAdd(ITriggerExecute trigger)
        {
            lock (m_lock)
            {
                try
                {
                    String id = trigger.GetId();
                    if (String.IsNullOrWhiteSpace(id)) return false;
                    if (m_ids.Contains(id))
                    {
                        TriggerStack.PushExiested(trigger);
                        return false;
                    }
                    m_ids.Add(id);
                    return true;
                }
                catch(Exception e)
                {
                    LogHelper.Error("执行定时任务的TryAdd报错,请查证.错误代码" + e.Message);
                }
                return false;
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            EndAction?.Invoke();
            return Task.CompletedTask;
        }
    }
}
