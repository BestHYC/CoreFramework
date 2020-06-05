using CSRedis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public class HRedisContext
    {
        private static Dictionary<HRedisName, CSRedisClient> m_dic = new Dictionary<HRedisName, CSRedisClient>();
        public static CSRedisClient Get(HRedisName name)
        {
            if (m_dic.ContainsKey(name)) return m_dic[name];
            return null;
        }
        public void AddDb(HRedisName name, String conn)
        {
            if (m_dic.ContainsKey(name)) throw new ArgumentException($"已经使用了{name}的仓储层");
            CSRedisClient client = new CSRedisClient(conn);
            m_dic.Add(name, client);

        }
        public void UseDb0(String conn)
        {
            if (m_dic.ContainsKey(HRedisName.DB0)) throw new ArgumentException($"已经使用了{HRedisName.DB0}的仓储层");
            CSRedisClient client = new CSRedisClient(conn);
            m_dic.Add(HRedisName.DB0, client);
        }
        public void UseDb1(String conn)
        {
            if (m_dic.ContainsKey(HRedisName.DB1))
                throw new ArgumentException($"已经使用了{HRedisName.DB1}的仓储层");
            CSRedisClient client = new CSRedisClient(conn);
            m_dic.Add(HRedisName.DB1, client);
        }
        public void UseDb2(String conn)
        {
            if (m_dic.ContainsKey(HRedisName.DB2))
                throw new ArgumentException($"已经使用了{HRedisName.DB2}的仓储层");
            CSRedisClient client = new CSRedisClient(conn);
            m_dic.Add(HRedisName.DB2, client);
        }
    }
    public static class HRedisExtension
    {
        public static IServiceCollection AddRedisContext(this IServiceCollection service, Func<HRedisContext> context)
        {
            context();
            return service;
        }
    }
    public enum HRedisName
    {
        DB0 = 0,
        DB1 = 1,
        DB2 = 2,
        DB3 = 3,
        DB4 = 4,
        DB5 = 5
    }
}
