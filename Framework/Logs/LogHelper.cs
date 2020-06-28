﻿using Newtonsoft.Json;
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
}
