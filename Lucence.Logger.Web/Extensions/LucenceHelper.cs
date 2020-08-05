using Framework;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

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
        public static List<String> SearchData(String project, String str, DateTime dt)
        {
            if (String.IsNullOrWhiteSpace(str)) return null;
            String path = LoggerModel.Getpath(project, dt);
            if (!File.Exists(Path.Combine(path, "write.lock"))) return null;
            List<String> list = new List<String>();
            try
            {
                IndexSearcher searcher = GetSearcher(project, dt);
                bool InOrder = true;
                ScoreDoc[] scoreDoc = SearchTime(searcher, str, "Content", 10, InOrder);
                foreach (var docs in scoreDoc)
                {
                    Document doc = searcher.Doc(docs.Doc);
                    String result = doc.Get("Content");
                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        list.Add(result);
                    }
                }
                searcher.Dispose();
            }
            catch (Exception e)
            {
                LogHelper.Critical("日志查询报错" + e.Message);
            }
            return list;
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
            Analyzer analyser = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            //Analyzer analyser = new PanGuAnalyzer();
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, field, analyser);
            var querys = queryString.Split('&');
            if (querys != null && querys.Length > 1)
            {
                BooleanQuery query = new BooleanQuery();
                foreach (var str in querys)
                {
                    if (String.IsNullOrWhiteSpace(str)) continue;
                    TermQuery term = new TermQuery(new Term("Content", str));
                    query.Add(parser.Parse(str), Occur.MUST);
                }
                TopFieldDocs topField = searcher.Search(query, null, 20, new Sort(new SortField("Time", SortField.STRING_VAL, true)));
                return topField.ScoreDocs;
            }
            else
            {
                Query query = parser.Parse(queryString);
                TopFieldDocs topField = searcher.Search(query, null, 20, new Sort(new SortField("Time", SortField.STRING_VAL, true)));
                return topField.ScoreDocs;
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
                try
                {
                    fsWriter.AddDocument(doc);
                    fsWriter.Commit();
                }catch(Exception e)
                {
                    LogHelper.Critical(e.ToJson());
                }
            }
        }
        private static DateTime m_now = DateTime.Now;
        /// <summary>
        /// 每天清除一次数据,新建新的索引及写入索引
        /// </summary>
        private static void Remove()
        {
            if (m_now.Day != DateTime.Now.Day)
            {
                lock (m_lock)
                {
                    if (m_now.Day == DateTime.Now.Day) return;
                    m_now = DateTime.Now;
                    m_indexSearch.Clear();
                    m_indexWrite.Clear();
                }
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
        private static ConcurrentDictionary<String, ConcurrentDictionary<String, IndexSearcher>> m_indexSearch = new ConcurrentDictionary<String, ConcurrentDictionary<String, IndexSearcher>>();
        private static Int32 m_update = 0;
        private static IndexWriter GetWriter(String project)
        {
            Remove();
            if (String.IsNullOrWhiteSpace(project)) project = "NoneName";
            String path = LoggerModel.Getpath(project, DateTime.Now);
            if (m_indexWrite.ContainsKey(project))
            {
                Interlocked.Exchange(ref m_update, 1);
                return m_indexWrite[project];
            }
            lock (m_lock)
            {
                if (m_indexWrite.ContainsKey(project))
                {
                    Interlocked.Exchange(ref m_update, 1);
                    return m_indexWrite[project];
                }
                IndexWriter fsWriter = null;
                Boolean isExiested = File.Exists(Path.Combine(path, "write.lock"));
                FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(path));
                Analyzer analyser = new StandardAnalyzer( Lucene.Net.Util.Version.LUCENE_30);
                //Analyzer analyser = new PanGuAnalyzer();
                fsWriter = new IndexWriter(fsDir, analyser, !isExiested, IndexWriter.MaxFieldLength.UNLIMITED);
                m_indexWrite.TryAdd(project, fsWriter);
                return fsWriter;
            }
        }
        private static IndexSearcher GetSearcher(String project, DateTime dt)
        {
            Remove();
            if (String.IsNullOrWhiteSpace(project)) project = "NoneName";
            String path = LoggerModel.Getpath(project, dt);
            if (m_indexSearch.ContainsKey(project))
            {
                var cacheIndex = m_indexSearch[project];
                if (cacheIndex.ContainsKey(path))
                {
                    if (Interlocked.CompareExchange(ref m_update, 0, 1) == 1 && dt.Day == m_now.Day)
                    {
                        cacheIndex.TryRemove(path, out var re);
                    }
                    else
                    {
                        return cacheIndex[path];
                    }
                }
                 
            }
            lock (m_lock)
            {
                if (!m_indexSearch.ContainsKey(project))
                {
                    m_indexSearch.TryAdd(project, new ConcurrentDictionary<string, IndexSearcher>());
                }
                var cacheIndex = m_indexSearch[project];
                if (cacheIndex.ContainsKey(path))
                {
                    return cacheIndex[path];
                }
                bool ReadOnly = true;
                FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(path));
                IndexSearcher searcher = new IndexSearcher(IndexReader.Open(fsDir, ReadOnly));
                cacheIndex.TryAdd(path, searcher);
                return searcher;
            }
        }
    }
}
