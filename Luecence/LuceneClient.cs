using Lucene.Net.Analysis; 
using Lucene.Net.Documents; 
using Lucene.Net.Index; 
using Lucene.Net.QueryParsers; 
using Lucene.Net.Store; 
using Lucene.Net.Search;
using System;
using System.Diagnostics;
using System.IO;
using LuecenceTest;
using System.Linq;

namespace LuceneClient
{
    class ProgramClient
    {
        //索引存放位置
        public static String INDEX_STORE_PATH = ProgramTest.INDEX_STORE_PATH;
        public static void SearchData(string[] args)
        {
            bool ReadOnly = true;
            FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(INDEX_STORE_PATH));
            IndexSearcher searcher = new IndexSearcher(IndexReader.Open(fsDir, ReadOnly));
            Stopwatch watch = new Stopwatch();
            watch.Start();
            bool InOrder = true;
            ScoreDoc[] scoreDoc = Search(searcher, "1595493765", "content", 10, InOrder);

            watch.Stop();
            Console.WriteLine("总共耗时{0}毫秒", watch.ElapsedMilliseconds);
            Console.WriteLine("总共找到{0}个文件", scoreDoc.Count());

            foreach (var docs in scoreDoc)
            {
                Document doc = searcher.Doc(docs.doc);
                Console.WriteLine("得分：{0}，文件名：{1}", docs.score, doc.Get("content"));
            }
            searcher.Close();
            Console.ReadLine();
        }

        static ScoreDoc[] Search(IndexSearcher searcher, string queryString, string field, int numHit, bool inOrder)
        {
            TopScoreDocCollector collector = TopScoreDocCollector.create(numHit, inOrder);

            Analyzer analyser = new PanGuAnalyzer();

            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, field, analyser);

            Query query = parser.Parse(queryString);

            searcher.Search(query, collector);

            return collector.TopDocs().scoreDocs;
        }
    }
}