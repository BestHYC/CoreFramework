using CSRedis;
using Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lucence.Logger.Web
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMyService(this IServiceCollection services, IConfiguration Configuration)
        {
            LoggerModel.SetLogger(Configuration);
            //MQRabbitConfig.SetConfig(Configuration);
            //日志的MQ接受消息,每次MQ的通信1m速度大概是40ms,消费能够达到10次/1ms, 
            //那么在执行期间最好控制在800条以下能够控制MQ消费
            // 注意只测试了1G数量的日志,再多没测试,理论会逐渐下降,
            //mq消息队列,配置时候展示,用的时候直接使用
            //services.AddHostedService<LoggerMqConsume>();
            return services;
        }
    }
    public class LoggerModel
    {
        public static String Path;
        public static String MQRouteKey;
        public static String MQQueueName;
        public static void SetLogger(IConfiguration Configuration)
        {
            Path = System.IO.Path.Combine(Environment.CurrentDirectory, "Lucence");
            MQRouteKey = Configuration.GetSection("LoggerModel:MQRouteKey").Value;
            MQQueueName = Configuration.GetSection("LoggerModel:MQQueueName").Value;
        }
        public static String Getpath(String name, DateTime dt)
        {
            String dic;
            if (name == "testtxt")
            {
                dic = System.IO.Path.Combine(Path, name);
            }
            else
            {
                dic = System.IO.Path.Combine(Path, name, dt.ToDayTime());
            }
            if (!System.IO.Directory.Exists(dic))
            {
                System.IO.Directory.CreateDirectory(dic);
            }
            return dic;
        }
    }
}
