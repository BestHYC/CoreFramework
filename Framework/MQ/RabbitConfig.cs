using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public class RabbitConfig
    {
        public String Host { get; set; }
        public Int32 Port { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String VHost { get; set; }
    }
    public static class MQRabbitConfig
    {
        public static RabbitConfig RabbitConfig { get; set; }
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
            RabbitConfig = rabbit;
        }
    }
}
