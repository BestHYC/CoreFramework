using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public class RabbitMQClient
    {
        private readonly IModel _channel;
        private static RabbitMQClient m_instance;
        public static RabbitMQClient Instance
        {

            get
            {
                if(m_instance == null)
                {
                    lock (m_lock)
                    {
                        if(m_instance == null)
                        {
                            m_instance = new RabbitMQClient();
                        }
                    }
                }
                return m_instance;
            }
        }
        public RabbitMQClient()
        {
            if (MQRabbitConfig.RabbitConfig == null) throw new Exception("执行MQ参数失败");
            RabbitConfig config = MQRabbitConfig.RabbitConfig;
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = config.Host,
                    UserName = config.UserName,
                    Password = config.Password,
                    Port = config.Port,
                    VirtualHost = config.VHost
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RabbitMQClient init fail,ErrorMessage{ex}");
            }
        }
        private static Object m_lock = new object();
        public virtual void PushMessage(string routingKey, String queue, object message)
        {
            lock (m_lock)
            {
                if (String.IsNullOrWhiteSpace(routingKey) || String.IsNullOrWhiteSpace(queue)) return;
                if (message == null) return;
                string msgJson;
                if (message.GetType().IsValueType || message.GetType() == typeof(String))
                {
                    msgJson = message.ToString();
                }
                else
                {
                    msgJson = JsonConvert.SerializeObject(message);
                }
                var body = Encoding.UTF8.GetBytes(msgJson);
                _channel.QueueDeclare(queue: queue,
                    exclusive: false,
                    durable: true,
                    autoDelete: false);
                _channel.BasicPublish(exchange: "message",
                                        routingKey: routingKey,
                                        basicProperties: null,
                                        body: body);
            }
        }
    }
}
