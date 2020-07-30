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
        /// <summary>
        /// 搜索日志
        /// </summary>
        /// <param name="project"></param>
        /// <param name="str"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 根据时间倒叙查询日志, 注意,如果是并搜索,那么需要&,否则是空格
        /// 如 仅搜索 1 and 123 传递的参数 是 "1&123"
        /// 如果搜索 1 or 123 传递的参数 空格拆分 "1 123"
        /// </summary>
        /// <param name="searcher"></param>
        /// <param name="queryString"></param>
        /// <param name="field"></param>
        /// <param name="numHit"></param>
        /// <param name="inOrder"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 存储日志信息
        /// </summary>
        /// <param name="model"></param>
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
        /// <summary>
        /// 复用写操作,每个系统一个写对象
        /// </summary>
        private static ConcurrentDictionary<String, IndexWriter> m_indexWrite = new ConcurrentDictionary<String, IndexWriter>();
        /// <summary>
        /// 复用搜索对象,注意,如果当前系统(上面的m_indexWrite)有更改,
        /// 那么需要重新将索引加载至内存中(每次m_indexWrite时候,去掉缓存的索引)
        /// </summary>
        private static ConcurrentDictionary<String, IndexSearcher> m_indexSearch = new ConcurrentDictionary<String, IndexSearcher>();
        private static  IndexWriter GetWriter(String project)
        {
            if (String.IsNullOrWhiteSpace(project)) project = "NoneName";
            String path = LoggerModel.Getpath(project);
            if (m_indexWrite.ContainsKey(project))
            {
                m_indexSearch.TryRemove(project, out var a);
                return m_indexWrite[project];
            }
            lock (m_lock)
            {
                if (m_indexWrite.ContainsKey(project))
                {
                    m_indexSearch.TryRemove(project, out var a);
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
            if (m_indexSearch.ContainsKey(project))
            {
                return m_indexSearch[project];
            }
            lock (m_lock)
            {
                if (m_indexSearch.ContainsKey(project))
                {
                    return m_indexSearch[project];
                }
                bool ReadOnly = true;
                FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(path));
                IndexSearcher searcher = new IndexSearcher(IndexReader.Open(fsDir, ReadOnly));
                m_indexSearch.TryAdd(project, searcher);
                return searcher;
            }

        }
    }
}
