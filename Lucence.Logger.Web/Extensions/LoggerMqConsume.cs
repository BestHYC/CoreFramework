using Framework;
using Microsoft.Extensions.Hosting;
using System;

namespace Lucence.Logger.Web
{
    public class LoggerMqConsume : RabbitListener, IHostedService
    {
        
        public LoggerMqConsume()
        {
            RouteKey = LoggerModel.MQRouteKey;
            QueueName = LoggerModel.MQQueueName;
            
        }
        public override bool Process(string message)
        {
            if (String.IsNullOrWhiteSpace(message)) return true;
            try
            {
                SealedLogModel model = message.ToObject<SealedLogModel>();
                LucenceHelper.StorageData(model);
            }
            catch (Exception e)
            {
                LogHelper.Error("LoggerMqConsume", e.Message);
                return false;
            }
            return true;
        }
        public override void StopRegist()
        {
        }
    }
}
