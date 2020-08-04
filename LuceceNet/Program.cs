using Framework;
using LucenceNet;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.IO;
using System.Text;

namespace LuceceNet
{
    public class AllConfig
    {
        public static String m_path = "E:\\Luecence\\Net\\B";
    }
    class Program
    {
        static void Main(string[] args)
        {
            String path = Path.Combine(Environment.CurrentDirectory, "testtxt.txt");
            if (File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                //文件内容
                using (var contents = new StreamReader(file.FullName, Encoding.UTF8))
                {
                    Random rnd = new Random();
                    while (!contents.EndOfStream)
                    {
                        int level = rnd.Next(0, 4);
                        SealedLogModel detail = new SealedLogModel()
                        {
                            Level = (SealedLogLevel)level,
                            ProjectName = "testtxt",
                            Sign = "测试",
                            Time = DateTime.Now.AddMinutes(level),
                            Value = contents.ReadLine()
                        };
                        LucenceHelper.StorageData(detail);
                    }
                }
            }
            Console.WriteLine("Hello World!");

            while (true)
            {
                String query = Console.ReadLine();
                LucenceHelper.SearchData("testtxt", query);
            }

        }
    }
}
