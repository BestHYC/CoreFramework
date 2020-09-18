using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;

namespace Framework
{
    public class NewBaseLogger
    {
        static NewBaseLogger()
        {
            SetLogger();
        }
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
        public static void SetLogger(IConfiguration configuration)
        {
            String level = configuration.GetSection("Logging:LogLevel:Default").Value;
            if (String.IsNullOrWhiteSpace(level)) level = "Max";
            SealedLogLevel logLevel;
            switch (level)
            {
                case "Trace":
                    logLevel = SealedLogLevel.Trace;
                    break;
                case "Debug":
                    logLevel = SealedLogLevel.Debug;
                    break;
                case "Information":
                    logLevel = SealedLogLevel.Info;
                    break;
                case "Warning":
                    logLevel = SealedLogLevel.Warn;
                    break;
                case "Error":
                    logLevel = SealedLogLevel.Error;
                    break;
                default:
                    logLevel = SealedLogLevel.Max;
                    break;
            }
            String zip = configuration.GetSection("Logging:LogLevel:Zip").Value;
            if (String.IsNullOrWhiteSpace(zip)) zip = "True";
            if (!Boolean.TryParse(zip, out Boolean iszip))
            {
                iszip = true; 
            }
            String keepDays = configuration.GetSection("Logging:LogLevel:Days").Value;
            if (String.IsNullOrWhiteSpace(keepDays)) keepDays = "30";
            if (!Int32.TryParse(keepDays, out Int32 days))
            {
                days = 30;
            }
            SetLogger(logLevel, iszip, days);
            var key = configuration.GetSection("MQLogger:Key").Value;
            if (!String.IsNullOrWhiteSpace(key))
            {
                SetMqLogger(configuration);
            }
        }
        /// <summary>
        /// 默认如此
        /// </summary>
        /// <param name="level"></param>
        /// <param name="zip"></param>
        /// <param name="day"></param>
        private static void SetLogger(SealedLogLevel level = SealedLogLevel.Max, Boolean zip=true, Int32 day=30)
        {
            LoggerSetting.Level = level;
            LoggerSetting.KeepDays = day;
            LoggerSetting.Zip = zip;
        }
        public static void SetMqLogger(IConfiguration configuration)
        {
            var key = configuration.GetSection("MQLogger:Key").Value;
            if (String.IsNullOrWhiteSpace(key)) throw new Exception("SetMqLogger配置错误");
            var queue = configuration.GetSection("MQLogger:Queue").Value;
            if (String.IsNullOrWhiteSpace(queue)) throw new Exception("SetMqLogger配置错误");
            var name = configuration.GetSection("MQLogger:SolutionName").Value;
            if (String.IsNullOrWhiteSpace(name)) name="";
            var isMq = configuration.GetSection("MQLogger:IsMQLogger").Value;
            if (String.IsNullOrWhiteSpace(isMq)) isMq = "False";
            if(Boolean.TryParse(isMq, out Boolean ismq))
            {
                if (ismq)
                {
                    SetMqLogger(key, queue, name);
                }
            }

        }
        /// <summary>
        /// 当前项目初始化,并且执行MQ操作中的key及Queue
        /// </summary>
        /// <param name="project">项目名</param>
        /// <param name="routeKey">路由值key</param>
        /// <param name="queue">消息队列</param>
        public static void SetMqLogger(String routeKey, String queue, String project="")
        {
            m_routeKey = routeKey;
            m_queue = queue;
            m_projectName = project;
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
    public class LogHelper<T> : NewBaseLogger
    {
        private static String m_name = typeof(T).Name;
        public static LogHelper<T> Instance = new LogHelper<T>();
        private LogHelper()
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
    public class LogHelper:NewBaseLogger
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
    public static class LoggerSetting
    {
        /// <summary>
        /// 最小保存单位,如果没有,则全部保存
        /// </summary>
        public static SealedLogLevel Level { get; set; }
        /// <summary>
        /// 是否zip压缩,默认压缩
        /// </summary>
        public static Boolean Zip { get; set; }
        /// <summary>
        /// -1是永久 0 是不保存 默认保存1个月,1个月前全部删除
        /// </summary>
        public static Int32 KeepDays { get; set; }
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
                if (m_models0.Count > 0)
                {
                    item = m_models0;
                    //m_models0 = new ConcurrentBag<SealedLogModel>();
                }
            }
            else if (Interlocked.CompareExchange(ref m_bag, 0, 1) == 1)
            {
                if (m_models1.Count > 0) 
                {
                    item = m_models1;
                    //m_models1 = new ConcurrentBag<SealedLogModel>();
                }
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
                ConvertToOtherPath();
            }
            catch (Exception e)
            {
                item.Add(new SealedLogModel()
                {
                    Time = DateTime.Now,
                    ControllerName = "执行操作失败",
                    Level = SealedLogLevel.Error,
                    Sign = "执行任务失败",
                    ProjectName = item.First()?.ProjectName,
                    Value = e.Message
                });
            }
            finally
            {
                if (item != null && item.Count > 0)
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
                while(!models.IsEmpty)
                {
                    models.TryTake(out var model);
                    if (model == null) continue;
                    String log = model.ToString();
                    sb_all.AppendLine(log);
                    if (model.Level < LoggerSetting.Level) continue;
                    switch (model.Level)
                    {
                        case SealedLogLevel.Debug:
                            sb_Debug.AppendLine(log);
                            break;
                        case SealedLogLevel.Error:
                            sb_Error.AppendLine(log);
                            break;
                        case SealedLogLevel.Info:
                            sb_Info.AppendLine(log);
                            break;
                        case SealedLogLevel.Trace:
                            sb_Trace.AppendLine(log);
                            break;
                        case SealedLogLevel.Warn:
                            sb_Warn.AppendLine(log);
                            break;
                        default:
                            sb_Trace.AppendLine(log);
                            break; ;
                    }
                }
                if (sb_Warn.Length > 0)
                {
                    String path = GetPath($"Warn-{DateTime.Now:yyyy-MM-dd}.log");
                    File.AppendAllText(path, sb_Warn.ToString());
                }
                if (sb_Debug.Length > 0)
                {
                    String path = GetPath($"Debug-{DateTime.Now:yyyy-MM-dd}.log");
                    File.AppendAllText(path, sb_Debug.ToString());
                }
                if (sb_Error.Length > 0)
                {
                    String path = GetPath($"Error-{DateTime.Now:yyyy-MM-dd}.log");
                    File.AppendAllText(path, sb_Error.ToString());
                }
                if (sb_Info.Length > 0)
                {
                    String path = GetPath($"Info-{DateTime.Now:yyyy-MM-dd}.log");
                    File.AppendAllText(path, sb_Info.ToString());
                }
                if (sb_Trace.Length > 0)
                {
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
                    //ConvertAllToOtherPath(sb_all.ToString());
                    String path = Path.Combine(m_path, $"alllog.log");
                    File.AppendAllText(path, sb_all.ToString());
                }
            }
        }
        private DateTime m_today = default;
        /// <summary>
        /// all中保存所有的文件,每次压缩至新的zip文件中
        /// zip文件保存30天的数据
        /// 2020-09-16文件夹中保存文本的日志,1个星期删除1次
        /// </summary>
        /// <param name="all"></param>
        private void ConvertToOtherPath()
        {
            if (m_today.Day != DateTime.Now.Day)
            {
                m_today = DateTime.Now;
                String path = Path.Combine(m_path, $"alllog.log");
                if (File.Exists(path))
                {
                    FileInfo info = new FileInfo(path);
                    if (info.CreationTime.Day != m_today.Day)
                    {
                        String newPath = GetPath(info.CreationTime, $"all-{info.CreationTime:yyyy-MM-dd}-{m_today.Minute}-{m_today.Millisecond}.log");
                        info.MoveTo(newPath);
                        String dic = Path.Combine(m_path, info.CreationTime.ToString("yyyy-MM-dd"));
                        ZipFile.CreateFromDirectory(dic, GetPath(Path.Combine(m_path, "Collection"), $"all-{info.CreationTime:yyyy-MM-dd}-{m_today.Minute}-{m_today.Millisecond}.zip"));
                        Directory.Delete(dic, true);
                    }
                }
                //删除30天以上的日志
                String collectionPath = Path.Combine(m_path, "Collection");
                if (Directory.Exists(collectionPath))
                {
                    var files = Directory.GetFiles(collectionPath);
                    foreach (var file in files)
                    {
                        FileInfo logger = new FileInfo(file);
                        if (logger.CreationTime.AddDays(30) < DateTime.Now)
                        {
                            File.Delete(file);
                        }
                    }
                }
                //删除7天以上的日志,注意,此处与460行代码之间差别
                //这里是对遗留数据进行保存
                if (Directory.Exists(m_path))
                {
                    var dics = Directory.GetDirectories(m_path);
                    foreach (var d in dics)
                    {
                        DirectoryInfo logger = new DirectoryInfo(d);
                        if (logger.Name == "Collection") continue;
                        if (logger.CreationTime.AddDays(30) < DateTime.Now)
                        {
                            Directory.Delete(d, true);
                        }
                    }
                }
            }
        }
        private string GetPath(String logname)
        {
            return GetPath(DateTime.Now, logname);
        }
        private String GetPath(DateTime dt, String logname)
        {
            String dic = Path.Combine(m_path, dt.ToString("yyyy-MM-dd"));
            return GetPath(dic, logname);
        }
        private String GetPath(String dic, String logname)
        {
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            return Path.Combine(dic, logname);
        }
        public void EnqueueLogger(SealedLogModel model)
        {
            if (model == null) return;
            if (Volatile.Read(ref m_bag) == 0)
            {
                m_models0.Add(model);
                return;
            }
            if (Volatile.Read(ref m_bag) == 1)
            {
                m_models1.Add(model);
                return;
            }
        }
    }
}
