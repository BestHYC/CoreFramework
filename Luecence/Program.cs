using LuceneClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuecenceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramTest.Execute(args);
            ProgramClient.SearchData(args);
            
        }
    }
}
