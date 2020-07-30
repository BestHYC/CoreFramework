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
            MQRabbitConfig.SetConfig(Configuration);
            services.AddHostedService<LoggerMqConsume>();
            LoggerModel.SetLogger(Configuration);
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
            Path = Configuration.GetSection("LoggerModel:Path").Value;
            MQRouteKey = Configuration.GetSection("LoggerModel:MQRouteKey").Value;
            MQQueueName = Configuration.GetSection("LoggerModel:MQQueueName").Value;
        }
        public static String Getpath(String name)
        {
            String dic = System.IO.Path.Combine(Path, name, DateTime.Now.ToDayTime());
            if (!System.IO.Directory.Exists(dic))
            {
                System.IO.Directory.CreateDirectory(dic);
            }
            return dic;
        }
    }
}
