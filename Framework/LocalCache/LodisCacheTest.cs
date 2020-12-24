using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework
{
    class LodisCacheTest
    {
        static void Main(string[] args)
        {
            RedisHelper.Initialization(new CSRedisClient("127.0.0.1:6379,defaultDatabase=0,poolsize=3,tryit=0"));
            ACache cache = new ACache();
            cache.Add(new A() { Num = 111, String = "1111" });
            while (true)
            {
                //测试定时删除操作
                Thread.Sleep(20 * 1000);
                Console.WriteLine(ACache.m_cache_String);
            }
        }
    }
    public class ACache
    {
        public static LodisCache m_cache_String = new LodisCache();
        public static LodisCache m_cache_int = new LodisCache();
        private static List<A> m_list = new List<A>();
        static ACache()
        {
            m_list.Add(new A() { Num = 1, String = "A" });
            m_list.Add(new A() { Num = 2, String = "B" });
            m_list.Add(new A() { Num = 3, String = "C" });
            m_list.Add(new A() { Num = 4, String = "D" });
            LodisCache.AddAction("ASubstribute", Init);
        }
        public static void Init()
        {
            foreach (var item in m_list)
            {
                m_cache_int.Add(item.Num, item, 10);
                m_cache_String.Add(item.String, item, 20);
            }
        }
        public void Add(A a)
        {
            LodisCache.Publish("ASubstribute");
            m_list.Add(a);
        }
    }
    public class A
    {
        public Int32 Num { get; set; }
        public String String { get; set; }
    }
}
