using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Framework
{

    public sealed class SealedLogger
    {
        private String m_path;
        private Logger m_logger;
        private Boolean m_isNlog = false;
        /// <summary>
        /// 所有锁分为2个状态
        /// </summary>
        private Int32 lock_Warn = 0;
        private Int32 lock_Info = 0;
        private Int32 lock_Trace = 0;
        private Int32 lock_Debug = 0;
        private Int32 lock_Error = 0;
        private Int32 lock_Critical = 0;
        private Int32 lock_Warn_0 = 0;
        private Int32 lock_Info_0 = 0;
        private Int32 lock_Trace_0 = 0;
        private Int32 lock_Debug_0 = 0;
        private Int32 lock_Error_0 = 0;
        private Int32 lock_Critical_0 = 0;
        private Int32 lock_Warn_1 = 0;
        private Int32 lock_Info_1 = 0;
        private Int32 lock_Trace_1 = 0;
        private Int32 lock_Debug_1 = 0;
        private Int32 lock_Error_1 = 0;
        private Int32 lock_Critical_1 = 0;
        private StringBuilder sb_Warn_0 = new StringBuilder();
        private StringBuilder sb_Info_0 = new StringBuilder();
        private StringBuilder sb_Trace_0 = new StringBuilder();
        private StringBuilder sb_Debug_0 = new StringBuilder();
        private StringBuilder sb_Error_0 = new StringBuilder();
        private StringBuilder sb_Critical_0 = new StringBuilder();
        private StringBuilder sb_Warn_1 = new StringBuilder();
        private StringBuilder sb_Info_1 = new StringBuilder();
        private StringBuilder sb_Trace_1 = new StringBuilder();
        private StringBuilder sb_Debug_1 = new StringBuilder();
        private StringBuilder sb_Error_1 = new StringBuilder();
        private StringBuilder sb_Critical_1 = new StringBuilder();
        private Timer m_timer;
        public SealedLogger()
        {
            m_timer = new Timer(Register, null, Timeout.Infinite, Timeout.Infinite);
            m_path = Path.Combine(Environment.CurrentDirectory, "Logs");
            if (!Directory.Exists(m_path))
            {
                Directory.CreateDirectory(m_path);
            }
            m_timer.Change(1000 * 1, Timeout.Infinite);
        }
        public void Register(Object state)
        {
            try
            {
                StringBuilder sb_all = new StringBuilder();
                #region critical
                if (sb_Critical_0.Length > 0 || sb_Critical_1.Length > 0)
                {
                    String critical = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Critical, 1, 0) == 0)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Critical_0, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        critical = sb_Critical_0.ToString();
                        sb_Critical_0.Clear();
                        Volatile.Write(ref lock_Critical_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Critical, 0, 1) == 1)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Critical_1, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        critical = sb_Critical_1.ToString();
                        sb_Critical_1.Clear();
                        Volatile.Write(ref lock_Critical_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Fatal(critical);
                    }
                    else
                    {
                        sb_all.AppendLine(critical);
                        String path = GetPath($"nlog-fatal-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, critical);
                    }
                }
                #endregion
                #region Debug
                if (sb_Debug_0.Length > 0 || sb_Debug_1.Length > 0)
                {
                    String Debug = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Debug, 1, 0) == 0)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Debug_0, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Debug = sb_Debug_0.ToString();
                        sb_Debug_0.Clear();
                        Volatile.Write(ref lock_Debug_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Debug, 0, 1) == 1)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Debug_1, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Debug = sb_Debug_1.ToString();
                        sb_Debug_1.Clear();
                        Volatile.Write(ref lock_Debug_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Debug(Debug);
                    }
                    else
                    {
                        sb_all.AppendLine(Debug);
                        String path = GetPath($"nlog-debug-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Debug);
                    }
                }
                #endregion
                #region Error
                if (sb_Error_0.Length > 0 || sb_Error_1.Length > 0)
                {
                    String Error = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Error, 1, 0) == 0)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Error_0, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Error = sb_Error_0.ToString();
                        sb_Error_0.Clear();
                        Volatile.Write(ref lock_Error_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Error, 0, 1) == 1)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Error_1, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Error = sb_Error_1.ToString();
                        sb_Error_1.Clear();
                        Volatile.Write(ref lock_Error_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Error(Error);
                    }
                    else
                    {
                        sb_all.AppendLine(Error);
                        String path = GetPath($"nlog-error-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Error);
                    }
                }
                #endregion
                #region Info
                if (sb_Info_0.Length > 0 || sb_Info_1.Length > 0)
                {
                    String Info = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Info, 1, 0) == 0)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Info_0, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Info = sb_Info_0.ToString();
                        sb_Info_0.Clear();
                        Volatile.Write(ref lock_Info_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Info, 0, 1) == 1)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Info_1, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Info = sb_Info_1.ToString();
                        sb_Info_1.Clear();
                        Volatile.Write(ref lock_Info_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Info(Info);
                    }
                    else
                    {
                        sb_all.AppendLine(Info);
                        String path = GetPath($"nlog-info-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Info);
                    }
                }
                #endregion
                #region Trace
                if (sb_Trace_0.Length > 0 || sb_Trace_1.Length > 0)
                {
                    String Trace = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Trace, 1, 0) == 0)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Trace_0, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Trace = sb_Trace_0.ToString();
                        sb_Trace_0.Clear();
                        Volatile.Write(ref lock_Trace_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Trace, 0, 1) == 1)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Trace_1, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Trace = sb_Trace_1.ToString();
                        sb_Trace_1.Clear();
                        Volatile.Write(ref lock_Trace_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Trace(Trace);
                    }
                    else
                    {
                        sb_all.AppendLine(Trace);
                        String path = GetPath($"nlog-trace-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Trace);
                    }
                }
                #endregion

                #region Warn
                if (sb_Warn_0.Length > 0 || sb_Warn_1.Length > 0)
                {
                    String Warn = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Warn, 1, 0) == 0)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Warn_0, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Warn = sb_Warn_0.ToString();
                        sb_Warn_0.Clear();
                        Volatile.Write(ref lock_Warn_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Warn, 0, 1) == 1)
                    {
                        while (true)
                        {
                            if (Interlocked.CompareExchange(ref lock_Warn_1, 1, 0) == 0)
                            {
                                break;
                            }
                            Thread.Sleep(1);
                        }
                        Warn = sb_Warn_1.ToString();
                        sb_Warn_1.Clear();
                        Volatile.Write(ref lock_Warn_1, 0);
                    }
                    sb_all.AppendLine(Warn);
                    if (m_isNlog)
                    {
                        m_logger.Warn(Warn);
                    }
                    else
                    {
                        String path = GetPath($"nlog-warn-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Warn);
                    }
                }
                if (sb_all.Length > 0)
                {
                    if (m_isNlog)
                    {
                        //m_logger.Warn(sb_all.ToString());
                    }
                    else
                    {
                        ConvertAllToOtherPath(sb_all.ToString());
                    }
                }
                #endregion
            }
            catch (Exception)
            {

            }
            finally
            {
                m_timer.Change(1000 * 1, Timeout.Infinite);
            }
        }
        private DateTime m_today = default;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="all"></param>
        private void ConvertAllToOtherPath(String all)
        {
            String path = Path.Combine(m_path, $"alllog.log");
            if (m_today == default || m_today.Day != DateTime.Now.Day)
            {
                m_today = DateTime.Now;
                if (File.Exists(path))
                {
                    FileInfo info = new FileInfo(path);
                    if (info.CreationTime.Day != m_today.Day)
                    {
                        String newPath = GetPath(info.CreationTime, $"nlog-all-{info.CreationTime:yyyy-MM-dd}-{m_today.Minute}-{m_today.Millisecond}.log");
                        File.Move(path, newPath);
                    }
                }
            }
            File.AppendAllText(path, all);
        }
        public void SetPath(String path)
        {
            m_path = path;
        }
        private string GetPath(String logname)
        {
            return GetPath(DateTime.Now, logname);
        }
        private String GetPath(DateTime dt , String logname)
        {
            String dic = Path.Combine(m_path, dt.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            return Path.Combine(dic, logname);
        }
        public void UseNlog()
        {
            m_isNlog = true;
            m_logger = LogManager.GetCurrentClassLogger();
        }
        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public void Info(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Info) == 0)
            {
                while (true)
                {
                    if(Interlocked.CompareExchange(ref lock_Info_0, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Info_0.AppendLine(message);
                Volatile.Write(ref lock_Info_0, 0);
            }
            else
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Info_1, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Info_1.AppendLine(message);
                Volatile.Write(ref lock_Info_1, 0);
            }
        }
        public void Warn(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Warn) == 0)
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Warn_0, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Warn_0.AppendLine(message);
                Volatile.Write(ref lock_Warn_0, 0);
            }
            else
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Warn_1, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Warn_1.AppendLine(message);
                Volatile.Write(ref lock_Warn_1, 0);
            }
        }
        public void Trace(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Trace) == 0)
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Trace_0, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Trace_0.AppendLine(message);
                Volatile.Write(ref lock_Trace_0, 0);
            }
            else
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Trace_1, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Trace_1.AppendLine(message);
                Volatile.Write(ref lock_Trace_1, 0);
            }
        }
        public void Debug(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Debug) == 0)
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Debug_0, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Debug_0.AppendLine(message);
                Volatile.Write(ref lock_Debug_0, 0);
            }
            else
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Debug_1, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Debug_1.AppendLine(message);
                Volatile.Write(ref lock_Debug_1, 0);
            }
        }
        public void Error(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Error) == 0)
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Error_0, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Error_0.AppendLine(message);
                Volatile.Write(ref lock_Error_0, 0);
            }
            else
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Error_1, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Error_1.AppendLine(message);
                Volatile.Write(ref lock_Error_1, 0);
            }
        }
        public void Critical(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Critical) == 0)
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Critical_0, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Critical_0.AppendLine(message);
                Volatile.Write(ref lock_Critical_0, 0);
            }
            else
            {
                while (true)
                {
                    if (Interlocked.CompareExchange(ref lock_Critical_1, 1, 0) == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1);
                }
                sb_Critical_1.AppendLine(message);
                Volatile.Write(ref lock_Critical_1, 0);
            }
        }
    }

    /// <summary>
    /// 此类有比较严重 也比较典型的并发异常数据,引以为戒,
    /// 所以并不是写的多,循环的多就是好代码,越简单的数据越能够清晰
    /// </summary>
    public class LogKind
    {
        private String m_path;
        private Logger m_logger;
        private Boolean m_isNlog = false;
        private Int32 lock_Warn = 0;
        private Int32 lock_Info = 0;
        private Int32 lock_Trace = 0;
        private Int32 lock_Debug = 0;
        private Int32 lock_Error = 0;
        private Int32 lock_Critical = 0;
        private StringBuilder sb_Warn = new StringBuilder();
        private StringBuilder sb_Info = new StringBuilder();
        private StringBuilder sb_Trace = new StringBuilder();
        private StringBuilder sb_Debug = new StringBuilder();
        private StringBuilder sb_Error = new StringBuilder();
        private StringBuilder sb_Critical = new StringBuilder();
        private Timer m_timer;
        public LogKind()
        {
            m_timer = new Timer(Register, null, Timeout.Infinite, Timeout.Infinite);
            m_path = Path.Combine(Environment.CurrentDirectory, "Logs");
            if (!Directory.Exists(m_path))
            {
                Directory.CreateDirectory(m_path);
            }
            m_timer.Change(1000 * 1, Timeout.Infinite);
        }
        public void Register(Object state)
        {
            try
            {
                while(Volatile.Read(ref lock_Critical) == -2)
                {
                    Thread.Sleep(2);
                }
                if (Interlocked.CompareExchange(ref lock_Critical, -1, 1) == 1)
                {
                    if (m_isNlog)
                    {
                        m_logger.Fatal(sb_Critical.ToString());
                    }
                    else
                    {
                        String path = Path.Combine(m_path, $"nlog-fatal-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, sb_Critical.ToString());
                    }
                    sb_Critical.Clear();
                }
                while (Volatile.Read(ref lock_Debug) == -2)
                {
                    Thread.Sleep(2);
                }
                if (Interlocked.CompareExchange(ref lock_Debug, -1, 1) == 1)
                {
                    if (m_isNlog)
                    {
                        m_logger.Debug(sb_Debug.ToString());
                    }
                    else
                    {
                        String path = Path.Combine(m_path, $"nlog-debug-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, sb_Debug.ToString());
                    }
                    sb_Debug.Clear();
                }
                while (Volatile.Read(ref lock_Error) == -2)
                {
                    Thread.Sleep(2);
                }
                if (Interlocked.CompareExchange(ref lock_Error, -1, 1) == 1)
                {
                    if (m_isNlog)
                    {
                        m_logger.Debug(sb_Error.ToString());
                    }
                    else
                    {
                        String path = Path.Combine(m_path, $"nlog-error-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, sb_Error.ToString());
                    }
                    sb_Error.Clear();
                }
                while (Volatile.Read(ref lock_Info) == -2)
                {
                    Thread.Sleep(2);
                }
                if (Interlocked.CompareExchange(ref lock_Info, -1, 1) == 1)
                {
                    if (m_isNlog)
                    {
                        m_logger.Info(sb_Info.ToString());
                    }
                    else
                    {
                        String path = Path.Combine(m_path, $"nlog-info-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, sb_Info.ToString());
                    }
                    sb_Info.ToString();
                }
                while (Volatile.Read(ref lock_Trace) == -2)
                {
                    Thread.Sleep(2);
                }
                if (Interlocked.CompareExchange(ref lock_Trace, -1, 1) == 1)
                {
                    if (m_isNlog)
                    {
                        m_logger.Trace(sb_Trace.ToString());
                    }
                    else
                    {
                        String path = Path.Combine(m_path, $"nlog-trace-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, sb_Trace.ToString());
                    }
                    sb_Trace.ToString();
                }
                while (Volatile.Read(ref lock_Warn) == -2)
                {
                    Thread.Sleep(2);
                }
                if (Interlocked.CompareExchange(ref lock_Warn, -1, 1) == 1)
                {
                    if (m_isNlog)
                    {
                        m_logger.Warn(sb_Warn.ToString());
                    }
                    else
                    {
                        String path = Path.Combine(m_path, $"nlog-warn-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, sb_Warn.ToString());
                    }
                    sb_Warn.ToString();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Interlocked.CompareExchange(ref lock_Warn, 0, -1);
                Interlocked.CompareExchange(ref lock_Critical, 0, -1);
                Interlocked.CompareExchange(ref lock_Debug, 0, -1);
                Interlocked.CompareExchange(ref lock_Error, 0, -1);
                Interlocked.CompareExchange(ref lock_Info, 0, -1);
                Interlocked.CompareExchange(ref lock_Trace, 0, -1);
                m_timer.Change(1000 * 1, Timeout.Infinite);
            }
        }
        public void SetPath(String path)
        {
            m_path = path;
        }
        public void UseNlog()
        {
            m_isNlog = true;
            m_logger = LogManager.GetCurrentClassLogger();
        }
        public void Info(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            while(Volatile.Read(ref lock_Info) == -1)
            {
                Thread.Sleep(10);
            }
            Volatile.Write(ref lock_Info, -2);
            sb_Info.AppendLine(message);
            Volatile.Write(ref lock_Info, 1);
        }
        public void Warn(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            while (Volatile.Read(ref lock_Warn) == -1)
            {
                Thread.Sleep(10);
            }
            Volatile.Write(ref lock_Warn, -2);
            sb_Warn.AppendLine(message);
            Volatile.Write(ref lock_Warn, 1);
        }
        public void Trace(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            while (Volatile.Read(ref lock_Trace) == -1)
            {
                Thread.Sleep(10);
            }
            Volatile.Write(ref lock_Trace, -2);
            sb_Trace.AppendLine(message);
            Volatile.Write(ref lock_Trace, 1);
        }
        public void Debug(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            while (Volatile.Read(ref lock_Debug) == -1)
            {
                Thread.Sleep(10);
            }
            Volatile.Write(ref lock_Debug, -2);
            sb_Debug.AppendLine(message);
            Volatile.Write(ref lock_Debug, 1);
        }
        public void Error(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            while (Volatile.Read(ref lock_Error) == -1)
            {
                Thread.Sleep(10);
            }
            Volatile.Write(ref lock_Error, -2);
            sb_Error.AppendLine(message);
            Volatile.Write(ref lock_Error, 1);
        }
        public void Critical(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            while (Volatile.Read(ref lock_Critical) == -1)
            {
                Thread.Sleep(10);
            }
            Volatile.Write(ref lock_Critical, -2);
            sb_Critical.AppendLine(message);
            Volatile.Write(ref lock_Critical, 1);
        }
    }
    
    /// <summary>
    /// 发现好的代码就应该不要用线程,少用锁就是最好的锁
    /// </summary>
    public sealed class SealedNoLockLogger
    {
        private String m_path;
        private Logger m_logger;
        private Boolean m_isNlog = false;
        /// <summary>
        /// 所有的状态分为2种
        /// 锁的状态<10时候使用sb_*_0
        /// 锁的状态>=10 < 20使用sb_*_1(此处可以>9既可)
        /// 
        /// 当从sb_*_0切换到sb_*_1时,有一个临界状态,此时正在切换,但是在AppendLine没有结束,
        /// 那么出现sb.ToString()时候出现临时性错误,即ToString()没有把当前的数据写入
        /// 所以 当 lock < 10 时候直接进入 sb_*_1的类型
        /// 进入后做循环处理判断是否为11,如果是11 说明是线程在定时器中在进行ToString()操作
        /// 换个说法
        /// 在lock 在 1--(01)的时候,说明是在sb_*_0 写入的状态 0 说明sb_*_0不在写入
        /// 在lock 在 11的时候,说明是在sb_*_1 写入的状态 10 说明sb_*_1不在写入
        /// 
        /// 结果:
        /// 当sb_*_0转为String时候,得 >=10 且不为 1
        /// 当sb_*_1转为String时候,得 <=9 且不为 11
        /// </summary>
        private Int32 lock_Warn = 0;
        private Int32 lock_Info = 0;
        private Int32 lock_Trace = 0;
        private Int32 lock_Debug = 0;
        private Int32 lock_Error = 0;
        private Int32 lock_Critical = 0;
        private Int32 lock_Warn_0 = 0;
        private Int32 lock_Info_0 = 0;
        private Int32 lock_Trace_0 = 0;
        private Int32 lock_Debug_0 = 0;
        private Int32 lock_Error_0 = 0;
        private Int32 lock_Critical_0 = 0;
        private Int32 lock_Warn_1 = 0;
        private Int32 lock_Info_1 = 0;
        private Int32 lock_Trace_1 = 0;
        private Int32 lock_Debug_1 = 0;
        private Int32 lock_Error_1 = 0;
        private Int32 lock_Critical_1 = 0;
        private StringBuilder sb_Warn_0 = new StringBuilder();
        private StringBuilder sb_Info_0 = new StringBuilder();
        private StringBuilder sb_Trace_0 = new StringBuilder();
        private StringBuilder sb_Debug_0 = new StringBuilder();
        private StringBuilder sb_Error_0 = new StringBuilder();
        private StringBuilder sb_Critical_0 = new StringBuilder();
        private StringBuilder sb_Warn_1 = new StringBuilder();
        private StringBuilder sb_Info_1 = new StringBuilder();
        private StringBuilder sb_Trace_1 = new StringBuilder();
        private StringBuilder sb_Debug_1 = new StringBuilder();
        private StringBuilder sb_Error_1 = new StringBuilder();
        private StringBuilder sb_Critical_1 = new StringBuilder();
        private Timer m_timer;
        public SealedNoLockLogger()
        {
            m_timer = new Timer(Register, null, Timeout.Infinite, Timeout.Infinite);
            m_path = Path.Combine(Environment.CurrentDirectory, "Logs");
            if (!Directory.Exists(m_path))
            {
                Directory.CreateDirectory(m_path);
            }
            m_timer.Change(1000 * 1, Timeout.Infinite);
        }
        public void Register(Object state)
        {
            try
            {
                #region critical
                if (sb_Critical_0.Length > 0 || sb_Critical_1.Length > 0)
                {
                    String critical = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Critical, 1, 0) == 0)
                    {
                        while (Interlocked.CompareExchange(ref lock_Critical_0, 1, 0) == 0)
                        {
                            critical = sb_Critical_0.ToString();
                            sb_Critical_0.Clear();
                        }
                        Volatile.Write(ref lock_Critical_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Critical, 0, 1) == 1)
                    {
                        while (Interlocked.CompareExchange(ref lock_Critical_1, 1, 0) == 0)
                        {
                            critical = sb_Critical_1.ToString();
                            sb_Critical_1.Clear();
                        }
                        Volatile.Write(ref lock_Critical_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Fatal(critical);
                    }
                    else
                    {
                        String path = GetPath($"nlog-fatal-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, critical);
                    }
                }
                #endregion
                #region Debug
                if (sb_Debug_0.Length > 0 || sb_Debug_1.Length > 0)
                {
                    String Debug = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Debug, 1, 0) == 0)
                    {
                        while (Interlocked.CompareExchange(ref lock_Debug_0, 1, 0) == 0)
                        {
                            Debug = sb_Debug_0.ToString();
                            sb_Debug_0.Clear();
                        }
                        Volatile.Write(ref lock_Debug_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Debug, 0, 1) == 1)
                    {
                        while (Interlocked.CompareExchange(ref lock_Debug_1, 1, 0) == 0)
                        {
                            Debug = sb_Debug_1.ToString();
                            sb_Debug_1.Clear();
                        }
                        Volatile.Write(ref lock_Debug_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Debug(Debug);
                    }
                    else
                    {
                        String path = GetPath($"nlog-debug-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Debug);
                    }
                }
                #endregion
                #region Error
                if (sb_Error_0.Length > 0 || sb_Error_1.Length > 0)
                {
                    String Error = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Error, 1, 0) == 0)
                    {
                        while (Interlocked.CompareExchange(ref lock_Error_0, 1, 0) == 0)
                        {
                            Error = sb_Error_0.ToString();
                            sb_Error_0.Clear();
                        }
                        Volatile.Write(ref lock_Error_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Error, 0, 1) == 1)
                    {
                        while (Interlocked.CompareExchange(ref lock_Error_1, 1, 0) == 0)
                        {
                            Error = sb_Error_1.ToString();
                            sb_Error_1.Clear();
                        }
                        Volatile.Write(ref lock_Error_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Error(Error);
                    }
                    else
                    {
                        String path = GetPath($"nlog-error-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Error);
                    }
                }
                #endregion
                #region Info
                if (sb_Info_0.Length > 0 || sb_Info_1.Length > 0)
                {
                    String Info = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Info, 1, 0) == 0)
                    {
                        while (Interlocked.CompareExchange(ref lock_Info_0, 1, 0) == 0)
                        {
                            Info = sb_Info_0.ToString();
                            sb_Info_0.Clear();
                        }
                        Volatile.Write(ref lock_Info_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Info, 0, 1) == 1)
                    {
                        while (Interlocked.CompareExchange(ref lock_Info_1, 1, 0) == 0)
                        {
                            Info = sb_Info_1.ToString();
                            sb_Info_1.Clear();
                        }
                        Volatile.Write(ref lock_Info_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Info(Info);
                    }
                    else
                    {
                        String path = GetPath($"nlog-info-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Info);
                    }
                }
                #endregion
                #region Trace
                if (sb_Trace_0.Length > 0 || sb_Trace_1.Length > 0)
                {
                    String Trace = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Trace, 1, 0) == 0)
                    {
                        while (Interlocked.CompareExchange(ref lock_Trace_0, 1, 0) == 0)
                        {
                            Trace = sb_Trace_0.ToString();
                            sb_Trace_0.Clear();
                        }
                        Volatile.Write(ref lock_Trace_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Trace, 0, 1) == 1)
                    {
                        while (Interlocked.CompareExchange(ref lock_Trace_1, 1, 0) == 0)
                        {
                            Trace = sb_Trace_1.ToString();
                            sb_Trace_1.Clear();
                        }
                        Volatile.Write(ref lock_Trace_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Trace(Trace);
                    }
                    else
                    {
                        String path = GetPath($"nlog-trace-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Trace);
                    }
                }
                #endregion

                #region Warn
                if (sb_Warn_0.Length > 0 || sb_Warn_1.Length > 0)
                {
                    String Warn = String.Empty;
                    if (Interlocked.CompareExchange(ref lock_Warn, 1, 0) == 0)
                    {
                        while (Interlocked.CompareExchange(ref lock_Warn_0, 1, 0) == 0)
                        {
                            Warn = sb_Warn_0.ToString();
                            sb_Warn_0.Clear();
                        }
                        Volatile.Write(ref lock_Warn_0, 0);
                    }
                    else if (Interlocked.CompareExchange(ref lock_Warn, 0, 1) == 1)
                    {
                        while (Interlocked.CompareExchange(ref lock_Warn_1, 1, 0) == 0)
                        {
                            Warn = sb_Warn_1.ToString();
                            sb_Warn_1.Clear();
                        }
                        Volatile.Write(ref lock_Warn_1, 0);
                    }
                    if (m_isNlog)
                    {
                        m_logger.Warn(Warn);
                    }
                    else
                    {
                        String path = GetPath($"nlog-warn-{DateTime.Now:yyyy-MM-dd}.log");
                        File.AppendAllText(path, Warn);
                    }
                }
                #endregion
            }
            catch (Exception)
            {

            }
            finally
            {
                m_timer.Change(1000 * 1, Timeout.Infinite);
            }
        }
        private string GetPath(String path)
        {
            String dic = Path.Combine(m_path, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            return Path.Combine(dic, path);
        }
        public void SetPath(String path)
        {
            m_path = path;
        }
        public void UseNlog()
        {
            m_isNlog = true;
            m_logger = LogManager.GetCurrentClassLogger();
        }
        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public void Info(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Info) == 0)
            {
                while (Interlocked.CompareExchange(ref lock_Info_0, 1, 0) == 0)
                {
                    sb_Info_0.AppendLine(message);
                }
                Volatile.Write(ref lock_Info_0, 0);
            }
            else
            {
                while (Interlocked.CompareExchange(ref lock_Info_1, 1, 0) == 0)
                {
                    sb_Info_1.AppendLine(message);
                }
                Volatile.Write(ref lock_Info_1, 0);
            }
        }
        public void Warn(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Warn) == 0)
            {
                while (Interlocked.CompareExchange(ref lock_Warn_0, 1, 0) == 0)
                {
                    sb_Warn_0.AppendLine(message);
                }
                Volatile.Write(ref lock_Warn_0, 0);
            }
            else
            {
                while (Interlocked.CompareExchange(ref lock_Warn_1, 1, 0) == 0)
                {
                    sb_Warn_1.AppendLine(message);
                }
                Volatile.Write(ref lock_Warn_1, 0);
            }
        }
        public void Trace(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Trace) == 0)
            {
                while (Interlocked.CompareExchange(ref lock_Trace_0, 1, 0) == 0)
                {
                    sb_Trace_0.AppendLine(message);
                }
                Volatile.Write(ref lock_Trace_0, 0);
            }
            else
            {
                while (Interlocked.CompareExchange(ref lock_Trace_1, 1, 0) == 0)
                {
                    sb_Trace_1.AppendLine(message);
                }
                Volatile.Write(ref lock_Trace_1, 0);
            }
        }
        public void Debug(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Debug) == 0)
            {
                while (Interlocked.CompareExchange(ref lock_Debug_0, 1, 0) == 0)
                {
                    sb_Debug_0.AppendLine(message);
                }
                Volatile.Write(ref lock_Debug_0, 0);
            }
            else
            {
                while (Interlocked.CompareExchange(ref lock_Debug_1, 1, 0) == 0)
                {
                    sb_Debug_1.AppendLine(message);
                }
                Volatile.Write(ref lock_Debug_1, 0);
            }
        }
        public void Error(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Error) == 0)
            {
                while (Interlocked.CompareExchange(ref lock_Error_0, 1, 0) == 0)
                {
                    sb_Error_0.AppendLine(message);
                }
                Volatile.Write(ref lock_Error_0, 0);
            }
            else
            {
                while (Interlocked.CompareExchange(ref lock_Error_1, 1, 0) == 0)
                {
                    sb_Error_1.AppendLine(message);
                }
                Volatile.Write(ref lock_Error_1, 0);
            }
        }
        public void Critical(String message)
        {
            if (String.IsNullOrEmpty(message)) message = "";
            if (Volatile.Read(ref lock_Critical) == 0)
            {
                while (Interlocked.CompareExchange(ref lock_Critical_0, 1, 0) == 0)
                {
                    sb_Critical_0.AppendLine(message);
                }
                Volatile.Write(ref lock_Critical_0, 0);
            }
            else
            {
                while (Interlocked.CompareExchange(ref lock_Critical_1, 1, 0) == 0)
                {
                    sb_Critical_1.AppendLine(message);
                }
                Volatile.Write(ref lock_Critical_1, 0);
            }
        }
    }
}
