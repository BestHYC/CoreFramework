using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Framework
{
    public enum EnumA
    {
        A
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            MQRabbitConfig.RabbitConfig = new RabbitConfig()
            {
                Host = "172.18.10.127",
                Password = "1234",
                Port = 5672,
                UserName = "qq",
                VHost = "/walletcloud"
            };
            LogHelper.SetMQLogger("Logger_Route_Key", "Logger_Queue", "TestConsole");
            for (Int32 i = 0; i < 10000; i++)
            {
                
                LogHelper.Error(Guid.NewGuid().ToString());
                LogHelper.Debug(Guid.NewGuid().ToString());
                LogHelper.Info(Guid.NewGuid().ToString());
                LogHelper.Trace(Guid.NewGuid().ToString());
                LogHelper.Warn(Guid.NewGuid().ToString());
                Thread.Sleep(1000*10);
            }
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
