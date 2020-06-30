using System;
using System.Diagnostics;
using System.Threading;

namespace Framework
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //LogHelper.UseNlog();
            Stopwatch span = new Stopwatch();
            span.Start();
            for (Int32 i = 0; i < 100; i++)
            {
                for(Int32 j=0; j< 10; j++)
                {
                    String str = i + "----" + j;
                    LogHelper.Critical(str);
                    LogHelper.Debug(str);
                    LogHelper.Error(str);
                    LogHelper.Info(str);
                    LogHelper.Trace(str);
                    LogHelper.Warn(str);
                }
            }
            span.Stop();
            Console.WriteLine("Hello World!       " + span.Elapsed.Milliseconds);
            Console.ReadLine();
        }
    }
}
