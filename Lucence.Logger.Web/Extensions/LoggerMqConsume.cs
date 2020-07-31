using Framework;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                return false;
            }
            return true;
        }
        public override void StopRegist()
        {
        }
    }
}
