using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public static class DefaultExtensions
    {
        public static String ToJson(this Object obj)
        {
            if (obj == null) return "";
            return JsonConvert.SerializeObject(obj);
        }
        public static T ToObject<T>(this String obj)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(obj)) return default(T);
                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch (Exception e)
            {
                LogHelper.Critical(obj, $"转成{typeof(T)}类型时候报错");
                throw e;
            }
        }
    }
}
