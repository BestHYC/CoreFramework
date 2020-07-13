using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework
{
    /// <summary>
    /// 简单自旋锁,用于快速的内存操作
    /// 优点:速度快,基于用户模式
    /// 缺点:出现死锁,并且全部存在于CPU,速度过慢会出现性能浪费
    /// 
    /// 在数据库,网络请求等IO操作,请慎用此锁
    /// 
    /// 当前如果锁住,会额外执行Thread.Sleep(1);减少当前执行时间片,
    /// 所以不适用于高精度模式中
    /// 
    /// 使用方式:
    /// SimpleSpinLock m_lock = new SimpleSpinLock();
    /// m_lock.Enter();
    /// doAnthing();
    /// m_lock.Leave();
    /// </summary>
    public class SimpleSpinLock
    {
        //0开启,1 关闭
        private Int32 m_init = 0;
        public void Enter()
        {
            while (true)
            {
                if (Interlocked.Exchange(ref m_init, 1) == 0)
                {
                    return;
                }
                Thread.Sleep(1);
            }
        }
        public void Leave()
        {
            Volatile.Write(ref m_init, 0);
        }
    }
}
