using Framework;
using jieba.NET;
using JiebaNet.Segmenter;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

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
            List<String> list = new List<String>();
            try
            {
                IndexSearcher searcher = GetSearcher(project);
                if (searcher == null) return list;
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
            }
            catch(Exception e)
            {
                LogHelper.Error("LucenceHelper Search", e.Message);
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
            var querys = queryString.Split('&');
            if (querys != null && querys.Length > 1)
            {
                BooleanQuery query = new BooleanQuery();
                foreach (var str in querys)
                {
                    if (String.IsNullOrWhiteSpace(str)) continue;
                    BooleanClause clause = new BooleanClause(query, Occur.MUST);
                    TermQuery term = new TermQuery(new Term("Content", str));
                    query.Add(term, clause.Occur);
                }
                TopFieldDocs topField = searcher.Search(query, null, 20, new Sort(new SortField("Time", SortFieldType.STRING_VAL, true)));
                return topField.ScoreDocs;
            }
            else
            {
                TermQuery term = new TermQuery(new Term("Content", queryString));
                TopFieldDocs topField = searcher.Search(term, null, 20, new Sort(new SortField("Time", SortFieldType.STRING_VAL, true)));
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
            doc.Add(new StringField("Time", model.Time.ToDefaultTrimTime(), Field.Store.YES));
            //文件名
            doc.Add(new StringField("Level", model.Level.ToString(), Field.Store.YES));
            doc.Add(new TextField("Content", model.ToString(), Field.Store.YES));

            lock (m_lock)
            {
               
                try
                {
                    IndexWriter fsWriter = GetWriter(model.ProjectName);
                    fsWriter.AddDocument(doc);
                    fsWriter.Commit();
                }
                catch(Exception e)
                {
                    LogHelper.Critical("MQ IndexWriter Create", e);
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
        private static ConcurrentDictionary<String, IndexSearcher> m_indexSearch = new ConcurrentDictionary<String, IndexSearcher>();
        private static DateTime m_now = DateTime.Now;
        /// <summary>
        /// 每天清除一次数据,新建新的索引及写入索引
        /// </summary>
        private static void Remove()
        {
            if(m_now.Day != DateTime.Now.Day)
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
        private static  IndexWriter GetWriter(String project)
        {
            Remove();
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
                FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(path));
                Analyzer analyser = new JieBaAnalyzer(TokenizerMode.Search);
                IndexWriterConfig writerConfig = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, analyser);
                fsWriter = new IndexWriter(fsDir, writerConfig);
                m_indexWrite.TryAdd(project, fsWriter);
                return fsWriter;
            }
        }
        private static IndexSearcher GetSearcher(String project)
        {
            Remove();
            if (String.IsNullOrWhiteSpace(project)) project = "NoneName";
            String path = LoggerModel.Getpath(project);
            if (!File.Exists(Path.Combine(path, "write.lock"))) return null;
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
                FSDirectory fsDir = FSDirectory.Open(new DirectoryInfo(path));
                IndexSearcher searcher = new IndexSearcher(DirectoryReader.Open(fsDir));
                m_indexSearch.TryAdd(project, searcher);
                return searcher;
            }
        }
    }
}
