using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Framework
{
    class Program
    {
        
        static void Main(string[] args)
        {
            LogHelper<A> helper = LogHelper<A>.Instance;
            Int32 num = 30;
            Int32 count = 0;
            while (Volatile.Read(ref count)<100000)
            {
                if(Volatile.Read(ref num) == 0)
                {
                    Thread.Sleep(1);
                    continue;
                }
                Interlocked.Decrement(ref num);
                Task.Run(() =>
                {
                    LogHelper.Debug("xxxxxx");
                    LogHelper.Critical("xxxxxx");
                    LogHelper.Error("xxxxxx");
                    LogHelper.Info("xxxxxx");
                    LogHelper.Trace("xxxxxx");
                    LogHelper.Warn("xxxxxx");
                    helper.LogCritical("测试", "xxxxxx");
                    helper.LogDebug("测试", "xxxxxx");
                    helper.LogError("测试", "xxxxxx");
                    helper.LogInformation("测试", "xxxxxx");
                    helper.LogTrace("测试", "xxxxxxx");
                    helper.LogWarning("测试", "xxxxxx");
                    Interlocked.Increment(ref count);
                    Interlocked.Increment(ref num);
                });
                
            }

            Console.WriteLine("Hello World!       " +DateTime.Now.ToString("YYYYMMDDHHmmSS"));
            Console.ReadLine();
        }
    }
    public class A
    {

    }
}
