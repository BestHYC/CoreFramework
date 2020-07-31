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
    /// 注意此处只有3个日志 warning日志,Infomation日志 及 all日志
    /// </summary>
    public class OldLogHelper<T>:BaseLogger
    {
        private static String m_name = typeof(T).Name;
        public static OldLogHelper<T> Instance = new OldLogHelper<T>();
        private OldLogHelper()
        {

        }
        private String GetName(String name = null)
        {
            if (String.IsNullOrEmpty(name)) return $"---类型{m_name}--";
            return $"---类型{m_name}--{name}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="obj"></param>
        public void LogInformation(Object obj)
        {
            m_logger.Info(GetString(GetName(), obj));
        }
        public void LogInformation(String obj)
        {
            m_logger.Info(GetString(GetName(), obj));
        }
        public void LogInformation(String name, Object obj)
        {
            m_logger.Info(GetString(GetName(name), obj));
        }
        public void LogInformation(String name, String obj)
        {
            m_logger.Info(GetString(GetName(name), obj));
        }
        public void LogWarning(Object obj)
        {
            m_logger.Warn(GetString(GetName(), obj));
        }
        public void LogWarning(String obj)
        {
            m_logger.Warn(GetString(GetName(), obj));
        }
        public void LogWarning(String name, Object obj)
        {
            m_logger.Warn(GetString(GetName(name), obj));
        }
        public void LogWarning(String name, String obj)
        {
            m_logger.Warn(GetString(GetName(name), obj));
        }
        public void LogTrace(String obj)
        {
            m_logger.Trace(GetString(GetName(), obj));
        }
        public void LogTrace(Object obj)
        {
            m_logger.Trace(GetString(GetName(), obj));
        }
        public void LogTrace(String name, Object obj)
        {
            m_logger.Trace(GetString(GetName(name), obj));
        }
        public void LogTrace(String name, String obj)
        {
            m_logger.Trace(GetString(GetName(name), obj));
        }
        public void LogError(String obj)
        {
            m_logger.Error(GetString(GetName(), obj));
        }
        public void LogError(Object obj)
        {
            m_logger.Error(GetString(GetName(), obj));
        }
        public void LogError(String name, Object obj)
        {
            m_logger.Error(GetString(GetName(name), obj));
        }
        public void LogError(String name, String obj)
        {
            m_logger.Error(GetString(GetName(name), obj));
        }
        public void LogDebug(String obj)
        {
            m_logger.Debug(GetString(GetName(), obj));
        }
        public void LogDebug(Object obj)
        {
            m_logger.Debug(GetString(GetName(), obj));
        }
        public void LogDebug(String name, Object obj)
        {
            m_logger.Debug(GetString(GetName(name), obj));
        }
        public void LogDebug(String name, String obj)
        {
            m_logger.Debug(GetString(GetName(name), obj));
        }
        public void LogCritical(Object obj)
        {
            m_logger.Error(GetString(GetName(), obj));
        }
        public void LogCritical(String obj)
        {
            m_logger.Error(GetString(GetName(), obj));
        }
        public void LogCritical(String name, Object obj)
        {
            m_logger.Error(GetString(GetName(name), obj));
        }
        public void LogCritical(String name, String obj)
        {
            m_logger.Error(GetString(GetName(name), obj));
        }
    }
    public class OldLogHelper : BaseLogger
    {
        public static void Info(Object obj)
        {
            m_logger.Info(GetString(obj));
        }
        public static void Info(String obj)
        {
            m_logger.Info(GetString(obj));
        }
        public static void Info(String name, Object obj)
        {
            m_logger.Info(GetString(name, obj));
        }
        public static void Info(String name, String obj)
        {
            m_logger.Info(GetString(name, obj));
        }
        public static void Warn(Object obj)
        {
            m_logger.Warn(GetString(obj));
        }
        public static void Warn(String obj)
        {
            m_logger.Warn(GetString(obj));
        }
        public static void Warn(String name, Object obj)
        {
            m_logger.Warn(GetString(name, obj));
        }
        public static void Warn(String name, String obj)
        {
            m_logger.Warn(GetString(name, obj));
        }
        public static void Trace(Object obj)
        {
            m_logger.Trace(GetString(obj));
        }
        public static void Trace(String obj)
        {
            m_logger.Trace(GetString(obj));
        }
        public static void Trace(String name, Object obj)
        {
            m_logger.Trace(GetString(name, obj));
        }
        public static void Trace(String name, String obj)
        {
            m_logger.Trace(GetString(name, obj));
        }
        public static void Error(Object obj)
        {
            m_logger.Error(GetString(obj));
        }
        public static void Error(String obj)
        {
            m_logger.Error(GetString(obj));
        }
        public static void Error(String name, Object obj)
        {
            m_logger.Error(GetString(name, obj));
        }
        public static void Error(String name, String obj)
        {
            m_logger.Error(GetString(name, obj));
        }
        public static void Debug(Object obj)
        {
            m_logger.Debug(GetString(obj));
        }
        public static void Debug(String obj)
        {
            m_logger.Debug(GetString(obj));
        }
        public static void Debug(String name, Object obj)
        {
            m_logger.Debug(GetString(name, obj));
        }
        public static void Debug(String name, String obj)
        {
            m_logger.Debug(GetString(name, obj));
        }
        public static void Critical(String obj)
        {
            m_logger.Error(GetString(obj));
        }
        public static void Critical(Object obj)
        {
            m_logger.Error(GetString(obj));
        }
        public static void Critical(String name, Object obj)
        {
            m_logger.Error(GetString(name, obj));
        }
        public static void Critical(String name, String obj)
        {
            m_logger.Error(GetString(name, obj));
        }
    }
    public class BaseLogger
    {
        protected static SealedLogger m_logger = new SealedLogger();
        public static void UseNlog()
        {
            m_logger.UseNlog();
        }
        private static Object m_lock = new object();
        private static StringBuilder sb = new StringBuilder();
        protected static String GetString(Object obj)
        {
            lock (m_lock)
            {
                String result = string.Empty;
                sb.AppendLine("");
                sb.AppendLine("---------------------------------------------");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("-----|");
                if (obj == null)
                {
                    sb.Append("参数为空");
                }
                else
                {
                    sb.Append(JsonConvert.SerializeObject(obj));
                }
                result = sb.ToString();
                sb.Clear();
                return result;
            }
        }
        protected static String GetString(String obj)
        {
            lock (m_lock)
            {
                String result = string.Empty;
                sb.AppendLine("");
                sb.AppendLine("---------------------------------------------");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("-----|----");
                if (String.IsNullOrEmpty(obj))
                {
                    sb.Append("参数为空");
                }
                else
                {
                    sb.Append(obj);
                }
                result = sb.ToString();
                sb.Clear();
                return result;
            }
        }
        protected static String GetString(String name, Object obj)
        {
            String str;
            if (obj != null)
            {
                str = $"{name}----数据为----{JsonConvert.SerializeObject(obj)}";
            }
            else
            {
                str = $"{name}----数据为空";
            }
            return GetString(str);
        }
        protected static String GetString(String name, String obj)
        {
            String str;
            if (!String.IsNullOrWhiteSpace(obj))
            {
                str = $"{name}----数据为----{obj}";
            }
            else
            {
                str = $"{name}----数据为空";
            }
            return GetString(str);
        }
    }
}
