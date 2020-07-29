using Framework;
using LuceneClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LuecenceTest
{
    class Program
    {
        static Program()
        {
            MQRabbitConfig.RabbitConfig = new RabbitConfig()
            {
                Host = "172.18.10.127",
                Password = "1234",
                Port = 5672,
                UserName = "qq",
                VHost = "/walletcloud"
            };
        }
        static void Main(string[] args)
        {
            var instance = LoggerMqConsume.Instance.StartAsync(new CancellationTokenSource().Token);
            while (true)
            {
                String str = Console.ReadLine();
                ProgramClient.SearchData(str);
            }
        }
    }
}
