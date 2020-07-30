using Framework;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lucence.Logger.Web
{
    public class LucenceHelper
    {
        private static Object m_lock = new object();
        public static List<String> SearchData(String project, String str)
        {
            if (String.IsNullOrWhiteSpace(str)) return null;
            String path = LoggerModel.Getpath(project);
            if (!File.Exists(Path.Combine(path, "write.lock"))) return null;
            List<String> list = new List<String>();
            try
            {
                IndexSearcher searcher = GetSearcher(project);
                bool InOrder = true;
                ScoreDoc[] scoreDoc = SearchTime(searcher, str, "Content", 10, InOrder);
                foreach (var docs in scoreDoc)
                {
                    Document doc = searcher.Doc(docs.doc);
                    String result = doc.Get("Content");
                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        list.Add(result);
                    }
                }
                searcher.Close();
            }
            catch(Exception)
            {

            }
            return list;
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
                foreach (var str in querys)
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
        public static void StorageData(SealedLogModel model)
        {
            if (model == null) return;
            Document doc = new Document();
            //文件路径
            doc.Add(new Field("Time", model.Time.ToDefaultTrimTime(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //文件名
            doc.Add(new Field("Level", model.Level.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Content", model.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            lock (m_lock)
            {
                IndexWriter fsWriter = GetWriter(model.ProjectName);
                fsWriter.AddDocument(doc);
                fsWriter.Commit();
            }
        }
        private static ConcurrentDictionary<String, IndexWriter> m_indexWrite = new ConcurrentDictionary<String, IndexWriter>();
        private static ConcurrentDictionary<String, IndexSearcher> m_indexReader = new ConcurrentDictionary<String, IndexSearcher>();
        private static  IndexWriter GetWriter(String project)
        {
            if (String.IsNullOrWhiteSpace(project)) project = "NoneName";
            String path = LoggerModel.Getpath(project);
            if (m_indexWrite.ContainsKey(project))
            {
                m_indexReader.TryRemove(project, out var a);
                return m_indexWrite[project];
            }
            lock (m_lock)
            {
                if (m_indexWrite.ContainsKey(project))
                {
                    m_indexReader.TryRemove(project, out var a);
                    return m_indexWrite[project];
                }
                IndexWriter fsWriter = null;
                Boolean isExiested = File.Exists(Path.Combine(path, "write.lock"));
                FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(path));
                Analyzer analyser = new PanGuAnalyzer();
                fsWriter = new IndexWriter(fsDir, analyser, !isExiested, IndexWriter.MaxFieldLength.UNLIMITED);
                m_indexWrite.TryAdd(project, fsWriter);
                return fsWriter;
            }
        }
        private static IndexSearcher GetSearcher(String project)
        {
            if (String.IsNullOrWhiteSpace(project)) project = "NoneName";
            String path = LoggerModel.Getpath(project);
            if (m_indexReader.ContainsKey(project))
            {
                return m_indexReader[project];
            }
            lock (m_lock)
            {
                if (m_indexReader.ContainsKey(project))
                {
                    return m_indexReader[project];
                }
                bool ReadOnly = true;
                FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(path));
                IndexSearcher searcher = new IndexSearcher(IndexReader.Open(fsDir, ReadOnly));
                m_indexReader.TryAdd(project, searcher);
                return searcher;
            }

        }
    }
}
