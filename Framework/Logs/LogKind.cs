using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Framework
{
    public class LogKind
    {
        private String m_path;
        private Logger m_logger;
        private Boolean m_isNlog = false;
        public LogKind()
        {
            m_path = Path.Combine(Environment.CurrentDirectory, "Logs");
            if (!Directory.Exists(m_path))
            {
                Directory.CreateDirectory(m_path);
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
            if (m_isNlog)
            {
                m_logger.Info(message);
            }
            else
            {
                String path = Path.Combine(m_path, $"nlog-info-{DateTime.Now:yyyy-MM-dd}.log");
                File.AppendAllText(path, message);
            }
        }
        public void Warn(String message)
        {
            if (m_isNlog)
            {
                m_logger.Warn(message);
            }
            else
            {
                String path = Path.Combine(m_path, $"nlog-warn-{DateTime.Now:yyyy-MM-dd}.log");
                File.AppendAllText(path, message);
            }
        }
        public void Trace(String message)
        {
            if (m_isNlog)
            {
                m_logger.Trace(message);
            }
            else
            {
                String path = Path.Combine(m_path, $"nlog-trace-{DateTime.Now:yyyy-MM-dd}.log");
                File.AppendAllText(path, message);
            }
        }
        public void Debug(String message)
        {
            if (m_isNlog)
            {
                m_logger.Debug(message);
            }
            else
            {
                String path = Path.Combine(m_path, $"nlog-debug-{DateTime.Now:yyyy-MM-dd}.log");
                File.AppendAllText(path, message);
            }
        }
        public void Error(String message)
        {
            if (m_isNlog)
            {
                m_logger.Error(message);
            }
            else
            {
                String path = Path.Combine(m_path, $"nlog-error-{DateTime.Now:yyyy-MM-dd}.log");
                File.AppendAllText(path, message);
            }
        }
        public void Critical(String message)
        {
            if (m_isNlog)
            {
                m_logger.Fatal(message);
            }
            else
            {
                String path = Path.Combine(m_path, $"nlog-fatal-{DateTime.Now:yyyy-MM-dd}.log");
                File.AppendAllText(path, message);
            }
        }
    }
}
