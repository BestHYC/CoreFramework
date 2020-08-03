using Lucene.Net.Documents;   using Lucene.Net.Index;   using Lucene.Net.Store;   using Lucene.Net.Util; 
  using Lucene.Net.Analysis;
using System.Text;
using System.IO;
using System.Diagnostics;
using System;
using System.Linq;
using Lucene.Net.Search;
using Framework;
using Lucene.Net.Analysis.Standard;

namespace LuecenceTest
{
    public class ProgramTest
    {
        //索引存放位置
        public static String INDEX_STORE_PATH = @"E:\MyCode\Best_Hong.Tool.Solution\Luecence\Data";
        //数据位置
        public static String DATA_PATH = @"E:\MyCode\Best_Hong.Tool.Solution\Luecence\2020-07-23";
        static IndexWriter fsWriter = null;
        public static void Execute()
        {
            Boolean rebuild = true;
            FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(INDEX_STORE_PATH));
            Analyzer analyser = new PanGuAnalyzer();
            fsWriter = new IndexWriter(fsDir, analyser, rebuild, IndexWriter.MaxFieldLength.UNLIMITED);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int count = IndexFiles(new FileInfo(DATA_PATH));
            fsWriter.Optimize();
            fsWriter.Close();
            watch.Stop();
            Console.WriteLine("总共耗时{0}毫秒", watch.ElapsedMilliseconds);
            Console.WriteLine("总共索引{0}个文件", count);
            Console.ReadLine();
        }
        static int IndexFiles(FileInfo file)
        {
            int num = 0;
            if (System.IO.Directory.Exists(file.FullName))
            {
                //处理文件
                var files = System.IO.Directory.GetFiles(file.FullName).Select(f => new FileInfo(f));
                if (files != null)
                {
                    foreach (var f in files)
                    {
                        getDocument(f);
                        num++;
                    }
                }
                //处理目录
                var directorys = System.IO.Directory.GetDirectories(file.FullName).Select(d => new FileInfo(d));
                if (directorys != null)
                {
                    foreach (var d in directorys)
                    {
                        num += IndexFiles(d);
                    }
                }
            }
            return num;
        }
        static Document getDocument(FileInfo file)
        {
            Document doc = new Document();
            //文件路径
            doc.Add(new Field("path", file.FullName, Field.Store.YES, Field.Index.NOT_ANALYZED));
            //文件名
            doc.Add(new Field("title", file.Name, Field.Store.YES, Field.Index.ANALYZED));
            fsWriter.AddDocument(doc);
            //文件内容
            using (var contents  = new StreamReader(file.FullName, Encoding.UTF8))
            {
                
                while (!contents.EndOfStream)
                {
                    Document detail = new Document();
                    String content = contents.ReadLine();
                    detail.Add(new Field("Content", content, Field.Store.YES, Field.Index.ANALYZED));
                    fsWriter.AddDocument(detail);
                }

            }
            return doc;
        }
    }
    public class LoggerMqConsume : RabbitListener
    {
        //索引存放位置
        public static String INDEX_STORE_PATH = @"E:\Luecence\Data";
        public static IndexWriter fsWriter = null;
        public static LoggerMqConsume Instance = new LoggerMqConsume();
        public LoggerMqConsume()
        {
            RouteKey = "Logger_Route_Key";
            QueueName = "Logger_Queue";
            FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(INDEX_STORE_PATH));
            Analyzer analyser = new PanGuAnalyzer();
            fsWriter = new IndexWriter(fsDir, analyser, true, IndexWriter.MaxFieldLength.UNLIMITED);
        }
        public override bool Process(string message)
        {
            if (String.IsNullOrWhiteSpace(message)) return true;
            try
            {
                SealedLogModel model = message.ToObject<SealedLogModel>();
                Document doc = new Document();
                //文件路径
                doc.Add(new Field("Time", model.Time.ToDefaultTrimTime(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                //文件名
                doc.Add(new Field("Level", model.Level.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("Content", model.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                fsWriter.AddDocument(doc);
                fsWriter.Commit();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public override void StopRegist()
        {
            fsWriter.Close();
        }
    }
}