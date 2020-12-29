using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework
{
    /// <summary>
    /// 目前只提供基础类型事项,一级缓存尽量只保留基础数据,所有非基础数据,请保存在redis中
    /// 之前做了比较,只是将索引id保存在本地缓存,而数据保存在redis,与全部保存在本地相比,属于毫秒级差距
    /// 但是复杂度和内存消耗差别很大,所以不建议重复造复杂轮子,只适合制造简单的易用的轮子
    /// 
    /// 
    /// 此处:在Cache中只会保留对应关系,做同步更新处理及监听处理
    /// 用的是Redis的的发布订阅,速度最快,虽然危险性最高,不支持在发送
    /// 后期还需要新增心跳机制
    /// </summary>
    public class LodisCache
    {
        private String m_null = String.Empty;
        /// <summary>
        /// 将所有的对象都转为字符串
        /// </summary>
        private ConcurrentDictionary<Object, Object> m_cache = new ConcurrentDictionary<Object, Object>();
        private static Object m_lock = new Object();
        /// <summary>
        /// 异步执行超时查询,然后删除所有超时查询对象
        /// </summary>
        private static void ExecuteExpire(Object state)
        {
            try
            {
                if (m_expire.Count == 0) return;
                List<double> t = new List<double>();
                foreach (var item in m_expire)
                {
                    if (item.Key > GetSeconds(DateTime.Now)) break;
                    t.Add(item.Key);
                }
                if (t.Count != 0)
                {
                    foreach (var item in t)
                    {
                        var v = m_expire[item];
                        if (v != null && v.Count() != 0)
                        {
                            foreach (var objs in v)
                            {
                                if (objs == null) continue;
                                var obj = objs as ExpiredTimeObject;
                                //移除此对象的缓存值
                                obj.Cache.Remove(obj.Key);
                            }
                        }
                        //移除当前的时间
                        m_expire.Remove(item);
                    }
                }
            }
            //捕获所有的异常
            catch (Exception e)
            {
                LogHelper.Critical("定时报错,错误日志为:" + e.ToString());
            }
            finally
            {
                m_timer.Change(1000 * 1, Timeout.Infinite);
            }
        }
        private static double GetSeconds(DateTime date)
        {
            return (date - new DateTime(2020, 1, 1)).TotalSeconds;
        }
        private static Timer m_timer;
        static LodisCache()
        {
            m_timer = new Timer(ExecuteExpire, null, Timeout.Infinite, Timeout.Infinite);
            m_timer.Change(1000 * 1, Timeout.Infinite);
            Init();
        }
        /// <summary>
        /// 保存的是超时时间与超时对象与Key之间的对应关系
        /// </summary>
        private static SortedList<double, LockHashSet<ExpiredTimeObject>> m_expire = new SortedList<double, LockHashSet<ExpiredTimeObject>>();
        /// <summary>
        /// 到秒级
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expires">秒级超时时间</param>
        public void Add(Object key, Object val, UInt32 expires = Int32.MaxValue)
        {
            //小于0 则直接不存储
            if (expires <= 0) return;
            //如果是空,那么当做重复新增数据
            if (key == null) key = m_null;
            if (val == null) val = m_null;
            if (m_cache.ContainsKey(key)) return;
            m_cache.TryAdd(key, val);
            if (expires != Int32.MaxValue)
            {
                lock (m_lock)
                {
                    double ex = GetSeconds(DateTime.Now.AddSeconds(expires));
                    LockHashSet<ExpiredTimeObject> set;
                    if (!m_expire.TryGetValue(ex, out set))
                    {
                        set = new LockHashSet<ExpiredTimeObject>();
                        m_expire.Add(ex, set);
                    }
                    set.Add(new ExpiredTimeObject()
                    {
                        Cache = this,
                        Key = key
                    });
                }
            }
        }
        public void Remove(Object key)
        {
            if (key == null) return;
            if (m_cache.ContainsKey(key))
            {
                m_cache.TryRemove(key, out var a);
            }
        }
        public void AddOrUpdate(Object key, Object val)
        {
            if (key == null) key = m_null;
            if (val == null) val = m_null;
            if (m_cache.ContainsKey(key))
            {
                m_cache[key] = val;
            }
            else
            {
                m_cache.TryAdd(key, val);
            }
        }
        public static void Init()
        {
            //只要启动过此程序,那么就新增1,后期更新一次,就降低一次数据,
            //如果一直不为0,会一直产生消费者数据,所以注意次数
            //注意:此处可能会出现前一次已经是10,后期没有消费完成,
            //重启程序后,此处变成20,那么就需要一个检测机制,是否属可以正常新增
            String path = AppDomain.CurrentDomain.DynamicDirectory;
            RedisHelper.SAdd("channel_redis_localcache_subscribe_num", path);
            RedisHelper.Subscribe(("channel_redis_localcache_subscribe",
                item =>
                {
                    String body = item.Body;
                    if (!String.IsNullOrWhiteSpace(body))
                    {
                        if (m_action.ContainsKey(body))
                        {
                            m_action[body].Invoke();
                        }
                    }
                }
            ));
        }
        private static ConcurrentDictionary<String, Action> m_action = new ConcurrentDictionary<string, Action>();
        /// <summary>
        /// 添加后,便会执行当前添加的action,不需要单独执行一次
        /// 如 Init();LodisCache.AddAction("ASubstribute", Init);不需要
        /// LodisCache.AddAction("ASubstribute", Init)中单行已经执行了Init.Invoke();
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public static void AddAction(String key, Action action)
        {
            if (String.IsNullOrWhiteSpace(key) || action == null) return;
            m_action.TryAdd(key, action);
            action();
        }
        /// <summary>
        /// 注意此处,channel是写死,导致此处如果被大量写入时候,会经常发生监听
        /// 所以做成可配置的,这样可以做成一个guid配置上去,做此数据单列
        /// </summary>
        /// <param name="key"></param>
        public static void Publish(String key)
        {
            if (String.IsNullOrWhiteSpace(key)) key = String.Empty;
            RedisHelper.Publish("channel_redis_localcache_subscribe", key);
        }

    }
    public class ExpiredTimeObject
    {
        public LodisCache Cache { get; set; }
        public Object Key { get; set; }
    }
}
