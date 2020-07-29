using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Framework
{
    public class NewBaseLogger
    {
        protected static NewSealedLogger m_logger = new NewSealedLogger();
        private static String m_projectName;
        private static String m_routeKey;
        private static String m_queue;
        /// <summary>
        /// 当前项目初始化
        /// </summary>
        /// <param name="projectName"></param>
        public static void SetProjectName(String projectName)
        {
            m_projectName = projectName;
        }
        /// <summary>
        /// 当前项目初始化,并且执行MQ操作中的key及Queue
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="routeKey"></param>
        /// <param name="queue"></param>
        public static void SetMQLogger(String routeKey, String queue)
        {
            m_routeKey = routeKey;
            m_queue = queue;
            m_logger.ExecuteModel += models =>
            {
                while (!models.IsEmpty)
                {
                    models.TryTake(out var model);
                    RabbitMQClient.Instance.PushMessage(m_routeKey, m_queue, model);
                }
            };
        }
        protected static SealedLogModel GetLogModel(SealedLogLevel level, Object val)
        {
            return new SealedLogModel()
            {
                ProjectName = m_projectName,
                Time = DateTime.Now,
                Level = level,
                Value = val
            };
        }
        protected static SealedLogModel GetLogModel(String sign, SealedLogLevel level, Object val)
        {
            return new SealedLogModel()
            {
                Time = DateTime.Now,
                ProjectName = m_projectName,
                Sign = sign,
                Level = level,
                Value = val
            };
        }
        protected static SealedLogModel GetLogModel(String controller, String sign, SealedLogLevel level, Object val)
        {
            return new SealedLogModel()
            {
                Time = DateTime.Now,
                ProjectName = m_projectName,
                ControllerName = controller,
                Sign = sign,
                Level = level,
                Value = val
            };
        }
        protected static SealedLogModel GetLogModel(String project, String controller, String sign, SealedLogLevel level, Object val)
        {
            return new SealedLogModel()
            {
                Time = DateTime.Now,
                ProjectName = project,
                ControllerName = controller,
                Sign = sign,
                Level = level,
                Value = val
            };
        }
    }
    public class NewLogHelper<T> : NewBaseLogger
    {
        private static String m_name = typeof(T).Name;
        public static NewLogHelper<T> Instance = new NewLogHelper<T>();
        private NewLogHelper()
        {

        }
        public void LogInformation(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, null, SealedLogLevel.Info, obj));
        }
        public void LogInformation(String sign, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, sign, SealedLogLevel.Info, obj));
        }
        public void LogWarning(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, null, SealedLogLevel.Warn, obj));
        }
        public void LogWarning(String sign, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, sign, SealedLogLevel.Warn, obj));
        }
        public void LogTrace(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, null, SealedLogLevel.Trace, obj));
        }
        public void LogTrace(String sign, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, sign, SealedLogLevel.Trace, obj));
        }
        public void LogError(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, null, SealedLogLevel.Error, obj));
        }
        public void LogError(String sign, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, sign, SealedLogLevel.Error, obj));
        }
        public void LogDebug(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, null, SealedLogLevel.Debug, obj));
        }
        public void LogDebug(String sign, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, sign, SealedLogLevel.Debug, obj));
        }
        public void LogCritical(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, null, SealedLogLevel.Error, obj));
        }
        public void LogCritical(String sign, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(m_name, sign, SealedLogLevel.Error, obj));
        }
    }
    public class NewLogHelper:NewBaseLogger
    {
        public static void Info(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(SealedLogLevel.Info, obj));
        }
        public static void Info(String name, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(name, SealedLogLevel.Info, obj));
        }
        public static void Warn(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(SealedLogLevel.Warn, obj));
        }
        public static void Warn(String name, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(name, SealedLogLevel.Warn, obj));
        }
        public static void Trace(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(SealedLogLevel.Trace, obj));
        }
        public static void Trace(String name, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(name, SealedLogLevel.Trace, obj));
        }
        public static void Error(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(SealedLogLevel.Error, obj));
        }
        public static void Error(String name, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(name, SealedLogLevel.Error, obj));
        }
        public static void Debug(Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(SealedLogLevel.Debug, obj));
        }
        public static void Debug(String name, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(name, SealedLogLevel.Debug, obj));
        }
        public static void Critical(String obj)
        {
            m_logger.EnqueueLogger(GetLogModel(SealedLogLevel.Error, obj));
        }
        public static void Critical(String name, Object obj)
        {
            m_logger.EnqueueLogger(GetLogModel(name, SealedLogLevel.Error, obj));
        }
    }
    public class NewSealedLogger
    {
        private ConcurrentBag<SealedLogModel> m_models0 = new ConcurrentBag<SealedLogModel>();
        private ConcurrentBag<SealedLogModel> m_models1 = new ConcurrentBag<SealedLogModel>();
        /// <summary>
        /// 0时,进入m_models0队列,1时进入m_models1队列
        /// </summary>
        private Int32 m_bag = 0;
        private Timer m_timer;
        private String m_path;
        /// <summary>
        /// 事件委托,注意,如果在执行数据后,依旧保留数据,会在本地保存一份日志
        /// 如果不希望在本地保留数据,那么将ConcurrentBag中数据清空
        /// 不建议在本地保留数据,不建议出现无限循环,容易死锁
        /// </summary>
        public event Action<ConcurrentBag<SealedLogModel>> ExecuteModel;
        public NewSealedLogger()
        {
            m_timer = new Timer(Register, null, Timeout.Infinite, Timeout.Infinite);
            m_path = Path.Combine(Environment.CurrentDirectory, "Logs");
            if (!Directory.Exists(m_path))
            {
                Directory.CreateDirectory(m_path);
            }
            m_timer.Change(1000 * 1, Timeout.Infinite);
        }
        private void Register(Object state)
        {
            ConcurrentBag<SealedLogModel> item = null;
            if (Interlocked.CompareExchange(ref m_bag, 1, 0) == 0)
            {
                if (m_models0.Count <= 0) return;
                item = m_models0;
                m_models0 = new ConcurrentBag<SealedLogModel>();
            }
            else if (Interlocked.CompareExchange(ref m_bag, 0, 1) == 1)
            {
                if (m_models1.Count <= 0) return;
                item = m_models1;
                m_models1 = new ConcurrentBag<SealedLogModel>();
            }
            ExecutingModels(item);
            m_timer.Change(1000 * 1, Timeout.Infinite);
        }
        private void ExecutingModels(ConcurrentBag<SealedLogModel> item)
        {
            if (item == null) return;
            try
            {
                ExecuteModel?.Invoke(item);
            }
            catch (Exception e)
            {
                item.Add(new SealedLogModel()
                {
                    Time = DateTime.Now,
                    ControllerName = "执行操作失败",
                    Level = SealedLogLevel.Error,
                    Sign = "执行任务失败",
                    Value = e.Message
                });
            }
            finally
            {
                if (item.Count > 0)
                {
                    CurrentExecuteModel(item);
                }
            }
        }
        private void CurrentExecuteModel(ConcurrentBag<SealedLogModel> models)
        {
            StringBuilder sb_Warn = new StringBuilder();
            StringBuilder sb_Info = new StringBuilder();
            StringBuilder sb_Trace = new StringBuilder();
            StringBuilder sb_Debug = new StringBuilder();
            StringBuilder sb_Error = new StringBuilder();
            StringBuilder sb_all = new StringBuilder();
            try
            {
                while (!m_models0.IsEmpty)
                {
                    m_models0.TryTake(out var model);
                    switch (model.Level)
                    {
                        case SealedLogLevel.Debug:
                            sb_Debug.AppendLine(model.ToString());
                            break;
                        case SealedLogLevel.Error:
                            sb_Error.AppendLine(model.ToString());
                            break;
                        case SealedLogLevel.Info:
                            sb_Info.AppendLine(model.ToString());
                            break;
                        case SealedLogLevel.Trace:
                            sb_Trace.AppendLine(model.ToString());
                            break;
                        case SealedLogLevel.Warn:
                            sb_Warn.AppendLine(model.ToString());
                            break;
                        default:
                            sb_Trace.AppendLine(model.ToString());
                            break; ;
                    }
                }
                sb_all.Append(sb_Debug);
                sb_all.Append(sb_Error);
                sb_all.Append(sb_Info);
                sb_all.Append(sb_Trace);
                if (sb_Warn.Length > 0)
                {
                    sb_all.Append(sb_Warn);
                    String path = GetPath($"Warn-{DateTime.Now:yyyy-MM-dd}.log");
                    File.AppendAllText(path, sb_Warn.ToString());
                }
                if (sb_Debug.Length > 0)
                {
                    sb_all.Append(sb_Debug);
                    String path = GetPath($"Debug-{DateTime.Now:yyyy-MM-dd}.log");
                    File.AppendAllText(path, sb_Debug.ToString());
                }
                if (sb_Error.Length > 0)
                {
                    sb_all.Append(sb_Error);
                    String path = GetPath($"Error-{DateTime.Now:yyyy-MM-dd}.log");
                    File.AppendAllText(path, sb_Error.ToString());
                }
                if (sb_Info.Length > 0)
                {
                    sb_all.Append(sb_Info);
                    String path = GetPath($"Info-{DateTime.Now:yyyy-MM-dd}.log");
                    File.AppendAllText(path, sb_Info.ToString());
                }
                if (sb_Trace.Length > 0)
                {
                    sb_all.Append(sb_Trace);
                    String path = GetPath($"Trace-{DateTime.Now:yyyy-MM-dd}.log");
                    File.AppendAllText(path, sb_Trace.ToString());
                }
            }
            catch(Exception e)
            {
                sb_all.AppendLine(e.Message);
            }
            finally
            {
                if (sb_all.Length > 0)
                {
                    ConvertAllToOtherPath(sb_all.ToString());
                }
            }
        }
        private DateTime m_today = default;
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
                        String newPath = GetPath(info.CreationTime, $"all-{info.CreationTime:yyyy-MM-dd}-{m_today.Minute}-{m_today.Millisecond}.log");
                        File.Move(path, newPath);
                    }
                }
            }
            File.AppendAllText(path, all);
        }
        private string GetPath(String logname)
        {
            return GetPath(DateTime.Now, logname);
        }
        private String GetPath(DateTime dt, String logname)
        {
            String dic = Path.Combine(m_path, dt.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            return Path.Combine(dic, logname);
        }
        public void EnqueueLogger(SealedLogModel model)
        {
            if (model == null) return;
            if(Volatile.Read(ref m_bag) == 0)
            {
                m_models0.Add(model);
            }else
            {
                m_models1.Add(model);
            }
        }
    }
}
