﻿using Microsoft.Extensions.Options;
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
        public RabbitMQClient(IOptions<RabbitConfig> option)
        {
            var config = option.Value;
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
        private Object m_lock = new object();
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
