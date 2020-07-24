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
            LockHashSet<String> hs = new LockHashSet<string>();
            HashSet<String> ha = new HashSet<string>();
            for(int i =0; i< 100; i++)
            {
                int a = i;
                Task.Run(() => 
                {
                    Console.WriteLine(a);
                    hs.Add(a.ToString());
                });
            }
            while(hs.Count() < 99)
            {
                Thread.Sleep(10);
            }
            Console.WriteLine("Hello World!       " + hs.ToJson());
            Console.ReadLine();
        }
        private void SetMq()
        {
            var mqclient = new RabbitMQClient();
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
