using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework
{
    public class RabbitListener
    {
        private readonly IConnection connection;
        private readonly IModel channel;


        public RabbitListener(RabbitConfig options)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    // 这是我这边的配置,自己改成自己用就好
                    HostName = options.Host,
                    UserName = options.UserName,
                    Password = options.Password,
                    Port = options.Port,
                    VirtualHost = options.VHost,
                    RequestedChannelMax = 5
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }
        protected string RouteKey;
        protected string QueueName;

        // 处理消息的方法
        public virtual bool Process(string message)
        {
            throw new NotImplementedException();
        }

        // 注册消费者监听在这里
        public void Register()
        {
            Console.WriteLine($"RabbitListener register,routeKey:{RouteKey}");
            channel.ExchangeDeclare(exchange: "message", type: "topic");
            channel.QueueDeclare(queue: QueueName, exclusive: false, durable: true, autoDelete: false);
            channel.QueueBind(queue: QueueName,
                              exchange: "message",
                              routingKey: RouteKey);
            channel.BasicQos(0, 5, false);//每次只接收5条消息
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var result = false;
                try
                {
                    var message = Encoding.UTF8.GetString(body.Span.ToArray());
                    result = Process(message);
                }
                catch (Exception)
                {
                    result = false;
                }
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };
            channel.BasicConsume(queue: QueueName, consumer: consumer);
        }

        public void DeRegister()
        {
            this.connection.Close();
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.connection.Close();
            return Task.CompletedTask;
        }
    }
}
