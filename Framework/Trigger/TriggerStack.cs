using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public static class TriggerStack
    {
        private static Queue<ITriggerExecute> m_list = new Queue<ITriggerExecute>();
        //进入队列的触发器不允许同时多个相同的Id
        private static HashSet<String> m_ids = new HashSet<string>();
        private static Object m_lock = new Object();
        /// <summary>
        /// 如果Id已存在,那么不会插入
        /// </summary>
        /// <param name="trigger"></param>
        public static void Push(ITriggerExecute trigger)
        {
            lock (m_lock)
            {
                try
                {
                    String id = trigger.GetId();
                    if (!String.IsNullOrWhiteSpace(id))
                    {
                        if (!m_ids.Contains(id))
                        {
                            m_list.Enqueue(trigger);
                            m_ids.Add(id);
                        }
                    }
                }catch(Exception e)
                {
                    LogHelper.Warn($"插入触发器报错,报错数据为{e.Message}");
                }
            }
        }
        public static void PushExiested(ITriggerExecute trigger)
        {
            lock (m_lock)
            {
                m_list.Enqueue(trigger);
            }
        }
        public static Queue<ITriggerExecute> GetAll()
        {
            lock (m_lock)
            {
                var item = m_list;
                m_list = new Queue<ITriggerExecute>();
                return item;
            }
        }
        public static Int32 Count()
        {
            lock (m_lock)
            {
                return m_list.Count;
            }
        }
        public static void Remove(ITriggerExecute item)
        {
            lock (m_lock)
            {
                try
                {
                    String id = item.GetId();
                    if (String.IsNullOrWhiteSpace(id)) return;
                    if (m_ids.Contains(id))
                    {
                        m_ids.Remove(id);
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Warn($"插入触发器报错,报错数据为{e.Message}");
                }
            }
        }
        public static void Remove(String id)
        {
            lock (m_lock)
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(id)) return;
                    if (m_ids.Contains(id))
                    {
                        m_ids.Remove(id);
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Warn($"插入触发器报错,报错数据为{e.Message}");
                }
            }
        }
    }
}
