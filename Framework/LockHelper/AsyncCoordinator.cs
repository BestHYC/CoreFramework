using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework
{
    /// <summary>
    /// 在多个Task任务执行完成时,进行通知,
    /// 类似单Task任务的ContinueWith
    /// 
    /// 
    /// 使用方式:
    /// AsyncCoordinator ac = new AsyncCoordinator();
    /// foreach(var item in 10){
    /// Task.Run(()=>{
    /// ac.AboutToBegin();
    /// }).Continuewith(()=> ac.JustEnded())
    /// }
    /// ac.AllBegun(status=> console.log("1"); )
    /// 
    /// </summary>
    public enum CoordinationStatus { AllDone, Timeout, Cancel}
    public sealed class AsyncCoordinator
    {
        private Int32 m_opCount = 1;
        private Int32 m_statusReported = 0;
        private Action<CoordinationStatus> m_callback;
        private Timer m_timer;
        public void AboutToBegin(Int32 opsToAdd = 1)
        {
            Interlocked.Add(ref m_opCount, opsToAdd);
        }
        public void JustEnded()
        {
            if (Interlocked.Decrement(ref m_opCount) == 0)
                ReportStatus(CoordinationStatus.AllDone);
        }
        public void AllBegun(Action<CoordinationStatus> callback, Int32 timeout = Timeout.Infinite)
        {
            m_callback = callback;
            if (timeout != Timeout.Infinite)
                m_timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
            JustEnded();
        }
        private void TimeExpired(Object o)
        {
            ReportStatus(CoordinationStatus.Timeout);
        }
        public void Cancel()
        {
            ReportStatus(CoordinationStatus.Cancel);
        }
        private void ReportStatus(CoordinationStatus status)
        {
            if(Interlocked.Exchange(ref m_statusReported, 1) == 0)
            {
                m_callback(status);
            }
        }
    }
}
