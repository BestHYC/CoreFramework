using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework
{
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
        public void UseNlog()
        {
            m_log.UseNlog();
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
