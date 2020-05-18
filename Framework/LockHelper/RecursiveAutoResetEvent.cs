using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework
{
    /// <summary>
    /// 代替互斥锁,达到当前线程通过,其他线程互斥的目的
    /// </summary>
    public class RecursiveAutoResetEvent : IDisposable
    {
        private AutoResetEvent m_lock = new AutoResetEvent(true);
        private Int32 m_owningThreadId = 0;
        private Int32 m_recursionCount = 0;
        public void Enter()
        {
            Int32 currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if(m_owningThreadId == currentThreadId)
            {
                m_recursionCount++;
                return;
            }
            m_lock.WaitOne();
            m_owningThreadId = currentThreadId;
            m_recursionCount = 1;
        }
        public void Leave()
        {
            if (m_owningThreadId != Thread.CurrentThread.ManagedThreadId)
                throw new InvalidOperationException();
            if(--m_recursionCount == 0)
            {
                m_owningThreadId = 0;
                m_lock.Set();
            }
        }
        public void Dispose()
        {
            m_lock.Dispose();
        }
    }
}
