using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Framework
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public class LogHelper<T>
    {
        private static LogKind m_logger;
        private static Type t = typeof(T);
        public static LogHelper<T> Instance = new LogHelper<T>();
        private StringBuilder sb = new StringBuilder();
        private String m_path;
        private LogHelper() 
        {
            m_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            m_logger = new LogKind();
        }
        private Object m_lock = new object();
        public static void UseNlog()
        {
            m_logger.UseNlog();
        }
        private String GetString(Object obj)
        {
            lock (m_lock)
            {
                String result = String.Empty;
                sb.AppendLine("");
                sb.AppendLine("---------------------------------------------");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("-----|");
                sb.Append(t.FullName);
                sb.AppendLine("");
                if (obj == null)
                {
                    sb.Append("参数为空");
                    result = sb.ToString();
                    sb.Clear();
                    return result;
                }
                result = sb.AppendLine(JsonConvert.SerializeObject(obj)).ToString();
                sb.Clear();
                return result;
            }
        }
        private String GetString(String obj)
        {
            lock (m_lock)
            {
                String result = String.Empty;
                sb.AppendLine("");
                sb.AppendLine("---------------------------------------------");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("-----|");
                sb.Append(t.FullName);
                sb.AppendLine("");
                if (obj == null)
                {
                    sb.Append("参数为空");
                    result = sb.ToString();
                    sb.Clear();
                    return result;
                }
                result = sb.AppendLine(obj).ToString();
                sb.Clear();
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="obj"></param>
        public void LogInformation(Object obj)
        {
            m_logger.Info(GetString(obj));
        }
        public void LogInformation(String obj)
        {
            m_logger.Info(GetString(obj));
        }
        public void LogWarning(Object obj)
        {
            m_logger.Warn(GetString(obj));
        }
        public void LogWarning(String obj)
        {
            m_logger.Warn(GetString(obj));
        }
        public void LogTrace(String obj)
        {
            m_logger.Trace(GetString(obj));
        }
        public void LogTrace(Object obj)
        {
            m_logger.Trace(GetString(obj));
        }
        public void LogError(String obj)
        {
            m_logger.Error(GetString(obj));
        }
        public void LogError(Object obj)
        {
            m_logger.Error(GetString(obj));
        }
        public void LogDebug(String obj)
        {
            m_logger.Debug(GetString(obj));
        }
        public void LogDebug(Object obj)
        {
            m_logger.Debug(GetString(obj));
        }
        public void LogCritical(Object obj)
        {
            m_logger.Critical(GetString(obj));
        }
        public void LogCritical(String obj)
        {
            m_logger.Critical(GetString(obj));
        }
    }
    public class LogHelper
    {
        private static readonly LogKind m_logger = new LogKind();
        private static StringBuilder sb = new StringBuilder();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="obj"></param>
        public static void Info(Object obj)
        {
            m_logger.Info(GetString(obj));
        }
        private static String GetString(Object obj)
        {
            String result = string.Empty;
            sb.AppendLine("");
            sb.AppendLine("---------------------------------------------");
            sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine("");
            if (obj == null)
            {
                sb.Append("参数为空");
                result = sb.ToString();
                sb.Clear();
                return result;
            }
            sb.Append(JsonConvert.SerializeObject(obj));
            result = sb.ToString();
            sb.Clear();
            return result;
        }
        private static String GetString(String obj)
        {
            String result = string.Empty;
            sb.AppendLine("");
            sb.AppendLine("---------------------------------------------");
            sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine("");
            if (String.IsNullOrEmpty(obj))
            {
                sb.Append("参数为空");
                result = sb.ToString();
                sb.Clear();
                return result;
            }
            sb.Append(obj);
            result = sb.ToString();
            sb.Clear();
            return result;
        }
        public static void Info(String obj)
        {
            m_logger.Info(GetString(obj));
        }
        public static void Warn(Object obj)
        {
            m_logger.Warn(GetString(obj));
        }
        public static void Warn(String obj)
        {
            m_logger.Warn(GetString(obj));
        }
        public static void Trace(Object obj)
        {
            m_logger.Trace(GetString(obj));
        }
        public static void Trace(String obj)
        {
            m_logger.Trace(GetString(obj));
        }
        public static void Error(Object obj)
        {
            m_logger.Error(GetString(obj));
        }
        public static void Error(String obj)
        {
            m_logger.Error(GetString(obj));
        }
        public static void Debug(Object obj)
        {
            m_logger.Debug(GetString(obj));
        }
        public static void Debug(String obj)
        {
            m_logger.Debug(GetString(obj));
        }
        public static void Critical(String obj)
        {
            m_logger.Critical(GetString(obj));
        }
        public static void Critical(Object obj)
        {
            m_logger.Critical(GetString(obj));
        }
    }
    public sealed class FlashLogger
    {

        /// <summary>
        /// 记录消息Queue
        /// </summary>
        private readonly ConcurrentQueue<FlashLogMessage> _que;

        /// <summary>
        /// 信号
        /// </summary>
        private readonly ManualResetEvent _mre;


        /// <summary>
        /// 日志
        /// </summary>
        private static FlashLogger _flashLog = new FlashLogger();

        private static LogKind m_log = new LogKind();
        private FlashLogger()
        {

            // 设置日志配置文件路径

            _que = new ConcurrentQueue<FlashLogMessage>();
            _mre = new ManualResetEvent(false);
        }

        /// <summary>
        /// 实现单例
        /// </summary>
        /// <returns></returns>
        public static FlashLogger Instance()
        {
            return _flashLog;
        }

        /// <summary>
        /// 另一个线程记录日志，只在程序初始化时调用一次
        /// </summary>
        public void Register()
        {
            Thread t = new Thread(new ThreadStart(WriteLog));
            t.IsBackground = false;
            t.Start();
        }

        /// <summary>
        /// 从队列中写日志至磁盘
        /// </summary>
        private void WriteLog()
        {
            while (true)
            {
                // 等待信号通知
                _mre.WaitOne();

                FlashLogMessage msg;
                // 判断是否有内容需要如磁盘 从列队中获取内容，并删除列队中的内容
                while (_que.Count > 0 && _que.TryDequeue(out msg))
                {
                    // 判断日志等级，然后写日志
                    switch (msg.Level)
                    {
                        case FlashLogLevel.Debug:
                            m_log.Debug(msg.Message);
                            break;
                        case FlashLogLevel.Info:
                            m_log.Info(msg.Message);
                            break;
                        case FlashLogLevel.Error:
                            m_log.Error(msg.Message);
                            break;
                        case FlashLogLevel.Warn:
                            m_log.Warn(msg.Message);
                            break;
                        case FlashLogLevel.Fatal:
                            m_log.Critical(msg.Message);
                            break;
                    }
                }

                // 重新设置信号
                _mre.Reset();
                Thread.Sleep(1);
            }
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">日志文本</param>
        /// <param name="level">等级</param>
        /// <param name="ex">Exception</param>
        public void EnqueueMessage(string message, FlashLogLevel level, Exception ex = null)
        {
            _que.Enqueue(new FlashLogMessage
            {
                Message = message,
                Level = level,
                Exception = ex
            });

            // 通知线程往磁盘中写日志
            _mre.Set();
        }

        public static void Debug(string msg, Exception ex = null)
        {
            Instance().EnqueueMessage(msg, FlashLogLevel.Debug, ex);
        }

        public static void Error(string msg, Exception ex = null)
        {
            Instance().EnqueueMessage(msg, FlashLogLevel.Error, ex);
        }

        public static void Fatal(string msg, Exception ex = null)
        {
            Instance().EnqueueMessage(msg, FlashLogLevel.Fatal, ex);
        }

        public static void Info(string msg, Exception ex = null)
        {
            Instance().EnqueueMessage(msg, FlashLogLevel.Info, ex);
        }

        public static void Warn(string msg, Exception ex = null)
        {
            Instance().EnqueueMessage(msg, FlashLogLevel.Warn, ex);
        }
    }
}
