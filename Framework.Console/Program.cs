using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Framework
{
    public class A
    {
        public static event  Action Init;
        public void xx()
        {
            Init?.Invoke();
            Console.WriteLine("A");
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            A.Init += () => { Console.WriteLine("1"); };
            A.Init += () => { Console.WriteLine("2"); };
            A.Init += () => { Console.WriteLine("3"); };
            A a = new A();
            a.xx();
            Console.WriteLine("Hello World!       " +DateTime.Now.ToString("YYYYMMDDHHmmSS"));
            Console.ReadLine();
        }
    }
}
