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
            int i = 0;
            while (i++<30)
            {
                LogHelper.Debug("xxxxxx");
                LogHelper.Critical("xxxxxx");
                LogHelper.Error("xxxxxx");
                LogHelper.Info("xxxxxx");
                LogHelper.Trace("xxxxxx");
                LogHelper.Warn("xxxxxx");
            }

            Console.WriteLine("Hello World!       " +DateTime.Now.ToString("YYYYMMDDHHmmSS"));
            Console.ReadLine();
        }
    }
}
