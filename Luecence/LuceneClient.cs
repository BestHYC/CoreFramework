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
using Lucene.Net.Analysis.Standard;

namespace LuceneClient
{
    class ProgramClient
    {
        //索引存放位置
        public static String INDEX_STORE_PATH = LoggerMqConsume.INDEX_STORE_PATH;
        public static void SearchData(String str)
        {
            if (String.IsNullOrWhiteSpace(str)) return;
            bool ReadOnly = true;
            FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(INDEX_STORE_PATH));
            IndexSearcher searcher = new IndexSearcher(IndexReader.Open(fsDir, ReadOnly));
            Stopwatch watch = new Stopwatch();
            watch.Start();
            bool InOrder = true;
            ScoreDoc[] scoreDoc = SearchTime(searcher, str, "Content", 10, InOrder);

            watch.Stop();
            Console.WriteLine("总共耗时{0}毫秒", watch.ElapsedMilliseconds);
            Console.WriteLine("总共找到{0}个文件", scoreDoc.Count());

            foreach (var docs in scoreDoc)
            {
                Document doc = searcher.Doc(docs.doc);
                Console.WriteLine("{0}", doc.Get("Content"));
            }
            searcher.Close();
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
        static ScoreDoc[] SearchTime(IndexSearcher searcher, string queryString, string field, int numHit, bool inOrder)
        {
            //TopScoreDocCollector collector = TopScoreDocCollector.create(numHit, inOrder);
            Analyzer analyser = new PanGuAnalyzer();
            
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, field, analyser);
            var querys = queryString.Split('&');
            if (querys != null || querys.Length > 1)
            {
                BooleanQuery query = new BooleanQuery();
                foreach(var str in querys)
                {
                    query.Add(parser.Parse(str), BooleanClause.Occur.MUST);
                }
                TopFieldDocs topField = searcher.Search(query, null, 20, new Sort(new SortField("Time", SortField.STRING_VAL, true)));
                return topField.scoreDocs;
            }
            else
            {
                Query query = parser.Parse(queryString);
                TopFieldDocs topField = searcher.Search(query, null, 20, new Sort(new SortField("Time", SortField.STRING_VAL, true)));
                //searcher.Search(query, collector);

                return topField.scoreDocs;
            }

        }
    }
}