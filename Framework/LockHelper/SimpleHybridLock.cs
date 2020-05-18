using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework
{
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
