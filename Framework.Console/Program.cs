using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework
{
    class Program
    {
        
        static void Main(string[] args)
        {
            HashSet<String> hs = new HashSet<string>();
            hs.Add("a");
            hs.Add("a");
            hs.Add("a");
            hs.Add("a");
            hs.Add("a");
            Queue<String> cd = new Queue<string>();
            cd.Enqueue("a");
            cd.Enqueue(Guid.NewGuid().ToString());
            cd.Enqueue(Guid.NewGuid().ToString());
            cd.Enqueue(Guid.NewGuid().ToString());
            var a = cd.FirstOrDefault(item => item == "a");
            var b = cd.FirstOrDefault(item => item == "b");
            Console.WriteLine("Hello World!       " +DateTime.Now.ToString("YYYYMMDDHHmmSS"));
            Console.ReadLine();
        }
        private void SetMq()
        {
            var mqclient = new RabbitMQClient(new RabbitConfig()
            {
                Host = "172.18.10.127",
                UserName = "qq",
                Password = "1234",
                VHost = "/walletcloud",
                Port = 5672
            });
            dynamic model = new
            {
                Key = 2,
                OrderId = "ceshikey"
            };
            mqclient.PushMessage("dispatch_order_key", "dispatch_order_queue", model);
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
