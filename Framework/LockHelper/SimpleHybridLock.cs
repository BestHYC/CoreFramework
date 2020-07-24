using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework
{
    /// <summary>
    /// 简单混合锁,包含原子锁及事件锁,
    /// 在速度极快的情况下,会通过原子锁快速执行
    /// 在速度慢的情况下,会通过事件锁锁住,避免浪费cpu,以及可能产生的死锁
    /// </summary>
    public class SimpleHybridLock : IDisposable
    {
        private Int32 m_waiters = 0;
        private AutoResetEvent m_waiterLock = new AutoResetEvent(false);
        public void Enter()
        {
            if (Interlocked.Increment(ref m_waiters) == 1)
                return;
            m_waiterLock.WaitOne();
        }
        public void Leave()
        {
            if (Interlocked.Decrement(ref m_waiters) == 0)
                return;
            m_waiterLock.Set();
        }
        public void Dispose()
        {
            m_waiterLock.Dispose();
        }
    }
}
