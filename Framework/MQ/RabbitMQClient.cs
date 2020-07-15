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
        private static RabbitConfig m_config;
        public static void SetConfig(IConfiguration Configuration)
        {
            RabbitConfig rabbit = new RabbitConfig()
            {
                Host = Configuration.GetSection("RabbitConfig:Host").Value,
                Password = Configuration.GetSection("RabbitConfig:Password").Value,
                Port = Int32.Parse(Configuration.GetSection("RabbitConfig:Port").Value),
                VHost = Configuration.GetSection("RabbitConfig:VHost").Value,
                UserName = Configuration.GetSection("RabbitConfig:UserName").Value,
            };
            m_config = rabbit;
        }
        public static RabbitMQClient Instance
        {

            get
            {
                if (m_config == null) throw new Exception("MQ无设置启动项,请配置好启动");
                if(m_instance == null)
                {
                    lock (m_lock)
                    {
                        if(m_instance == null)
                        {
                            m_instance = new RabbitMQClient(m_config);
                        }
                    }
                }
                return m_instance;
            }
        }
        public RabbitMQClient(IOptions<RabbitConfig> option) : this(option.Value)
        {
        }
        public RabbitMQClient(RabbitConfig config)
        {
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
                string msgJson = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(msgJson);
                LogHelper.Info($"PushMessage,routingKey:{routingKey},Body is {body}");
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
