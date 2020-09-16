using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Framework
{
    public enum EnumA
    {
        A, B, C
    }
    public class EnumAT
    {
        public static EnumA A { get; set; }
    }
    public class JWTTokenModel
    {
        public DateTime Now { get; set; }
        public String Accountno { get; set; }
    }
    public class TestMethod
    {
        public void A()
        {

        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            LogHelper.Critical("xxxxxxxxxxxxxxx");
            Console.ReadLine();
        }
        private static void SetMq()
        {
            MQRabbitConfig.RabbitConfig = new RabbitConfig()
            {
                Host = "172.18.10.127",
                Password = "1234",
                Port = 5672,
                UserName = "qq",
                VHost = "/walletcloud"
            };
            RabbitConfig config = MQRabbitConfig.RabbitConfig;
            IModel _channel;
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
                _channel.ExchangeDeclare(exchange: "topic", type: "topic");
                _channel.QueueDeclare(queue: "topic_queue1", autoDelete:false);
                _channel.QueueDeclare(queue: "topic_queue2", autoDelete: false);
                _channel.QueueBind(queue: "topic_queue1", exchange: "topic", routingKey: "topic.queue1");
                _channel.QueueBind(queue: "topic_queue2", exchange: "topic", routingKey: "topic.#");
                for (int i = 0; i < 10; i++)
                {
                    var body1 = Encoding.UTF8.GetBytes("fanout_queue" + i);
                    var body2 = Encoding.UTF8.GetBytes("fanout_queue2" + i);
                    _channel.BasicPublish(exchange: "topic",
                                            routingKey: "topic.queue1",
                                            basicProperties: null,
                                            body: body1);
                    _channel.BasicPublish(exchange: "topic",
                             routingKey: "topic.topic_queue2",
                             basicProperties: null,
                             body: body2);
                }
                Thread.Sleep(1000 * 1);
                var consumer = new EventingBasicConsumer(_channel);
                Int32 all = 0;
                consumer.Received += (model, ea) =>
                {
                    Interlocked.Increment(ref all);
                    var body = ea.Body;
                    var result = true;
                    try
                    {
                        var message = Encoding.UTF8.GetString(body.Span.ToArray());
                        Console.WriteLine(message);
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                    if (result)
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                };
                _channel.BasicConsume(queue: "topic_queue1", consumer: consumer);
                Thread.Sleep(1000);
                var consumer1 = new EventingBasicConsumer(_channel);
                consumer1.Received += (model, ea) =>
                {
                    Interlocked.Increment(ref all);
                    var body = ea.Body;
                    var result = true;
                    try
                    {
                        var message = Encoding.UTF8.GetString(body.Span.ToArray());
                        Console.WriteLine(message);
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                    if (result)
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                };
                _channel.BasicConsume(queue: "topic_queue2", consumer: consumer1);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void Consume()
        {

        }
        private void LogAction()
        {
            int i = 0;
            while (i++ < 30)
            {
                LogHelper.Debug("xxxxxx");
                LogHelper.Critical("xxxxxx");
                LogHelper.Error("xxxxxx");
                LogHelper.Info("xxxxxx");
                LogHelper.Trace("xxxxxx");
                LogHelper.Warn("xxxxxx");
            }
        }
    }
}
