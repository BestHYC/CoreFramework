using System;

namespace Framework
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //LogHelper.UseNlog();
            for (Int32 i = 0; i < 10000; i++)
            {
                LogHelper.Critical(i);
                LogHelper.Debug(i);
                LogHelper.Error(i);
                LogHelper.Info(i);
                LogHelper.Trace(i);
                LogHelper.Warn(i);
            }
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
