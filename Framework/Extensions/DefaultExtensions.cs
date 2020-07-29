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
        public static String ToDefaultTime(this DateTime dt)
        {
            if (dt == default(DateTime)) return "";
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static String ToDefaultTrimTime(this DateTime dt)
        {
            if (dt == default(DateTime)) return "";
            return dt.ToString("yyyyMMddHHmmss");
        }
        public static String To12Time(this DateTime dt)
        {
            if (dt == default(DateTime)) return "";
            return dt.ToString("yyyy-MM-dd hh:mm:ss");
        }
        public static String ToDayTime(this DateTime dt)
        {
            if (dt == default(DateTime)) return "";
            return dt.ToString("yyyy-MM-dd");
        }
        public static String ToDayTrimTime(this DateTime dt)
        {
            if (dt == default(DateTime)) return "";
            return dt.ToString("yyyyMMdd");
        }
        public static String ToFFFTime(this DateTime dt)
        {
            if (dt == default(DateTime)) return "";
            return dt.ToString("yyyy-MM-dd HH:mm:ss fff");
        }
    }
}
