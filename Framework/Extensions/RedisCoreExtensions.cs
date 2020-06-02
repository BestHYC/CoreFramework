using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Framework
{
    public static class RedisExtension
    {
        public static Object[] Convert<Tkey, Tvalue>(this IDictionary<Tkey, Tvalue> dic)
        {
            if (dic == null) return null;
            List<Object> list = new List<Object>();
            foreach (var item in dic)
            {
                list.Add(item.Key);
                list.Add(item.Value);
            }
            var arr = list.ToArray();
            return arr;
        }
    }
}
