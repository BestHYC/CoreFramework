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
            Type t = typeof(TestMethod);
            var a = t.GetMethods(System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Instance);
            JWTTokenModel m = new JWTTokenModel()
            {
                Now = DateTime.Now,
                Accountno = "2010202830280"
            };
            String p = SecretHelper.DESEncrypt(m.ToJson(), "TrustKey", "TrustKey");
            String dt = SecretHelper.DESDecrypt(p, "TrustKey", "TrustKey");
            //var a = dt.ToObject<JWTTokenModel>();
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
