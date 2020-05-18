using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Framework
{
    public static class EnumExtensions
    {
        public static String GetName(this Enum info)
        {
            return info.ToString();
        }
        public static T GetAttribute<T>(this Enum info) where T : Attribute
        {
            Type t = info.GetType();
            FieldInfo fd = t.GetField(info.ToString());
            if (fd == null) return null;
            T attr = fd.GetCustomAttribute<T>();
            return attr;
        }
    }
}
