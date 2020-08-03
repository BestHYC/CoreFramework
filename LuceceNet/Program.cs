using jieba.NET;
using JiebaNet.Segmenter;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.IO;
using System.Text;

namespace LuceceNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Analyzer analyzer =  new JieBaAnalyzer(TokenizerMode.Search);
            FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo("E:\\Luecence\\Net"));
            IndexWriterConfig writerConfig = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, analyzer);
            IndexWriter writer = new IndexWriter(fsDir, writerConfig);
            String path = System.IO.Path.Combine(Environment.CurrentDirectory, "testtxt.txt");
            if (System.IO.File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                //文件内容
                using (var contents = new StreamReader(file.FullName, Encoding.UTF8))
                {
                    Random rnd = new Random();
                    while (!contents.EndOfStream)
                    {
                        AddDocument(writer, DateTime.Now.Ticks.ToString(), contents.ReadLine());
                    }
                }
            }
            writer.Dispose();
            Console.WriteLine("Hello World!");

            while (true)
            {
                String query = Console.ReadLine();
                Search(query);
            }

        }
        static void Search(string keywords)
        {
            //Analyzer analyzer = new JieBaAnalyzer(TokenizerMode.Default);                  //分词器
            FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo("E:\\Luecence\\Net"));
            IndexSearcher searcher = new IndexSearcher(DirectoryReader.Open(fsDir));   //指定其搜索的目录  
            TermQuery query = new TermQuery(new Term("content", keywords));
            TopFieldDocs topField = searcher.Search(query, null, 20, new Sort(new SortField("Time", SortFieldType.STRING_VAL, true)));
            foreach (var docs in topField.ScoreDocs)
            {
                Document doc = searcher.Doc(docs.Doc);
                Console.WriteLine("{0}", doc.Get("content"));
            }
        }
        static void AddDocument(IndexWriter writer, string title, string content)
        {
            Document document = new Document();
            document.Add(new StringField("title", title, Field.Store.YES));
            document.Add(new TextField("content", content, Field.Store.YES));
            writer.AddDocument(document);
        }
    }
}
