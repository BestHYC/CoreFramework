using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Framework
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //LogHelper.UseNlog();
            Console.WriteLine("Hello World!       " +DateTime.Now.ToString("YYYYMMDDHHmmSS"));
            Console.ReadLine();
        }
    }
}
