using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 日志内容
    /// </summary>
    public class SealedLogModel
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public SealedLogLevel Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 当前项目名
        /// </summary>
        public String ProjectName { get; set; }
        /// <summary>
        /// 当前类型名
        /// </summary>
        public String ControllerName { get; set; }
        /// <summary>
        /// 当前标记
        /// </summary>
        public String Sign { get; set; }
        /// <summary>
        /// 当前值
        /// </summary>
        public Object Value { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if(Time == default(DateTime))
            {
                Time = DateTime.Now;
            }
            sb.Append($"{{ \"{nameof(Time)}\":\"{Time.ToDefaultTime()}\",");
            sb.Append($"\"{nameof(Level)}\":\"{Level}\",");
            if (!String.IsNullOrWhiteSpace(ProjectName))
            {
                sb.Append($"\"{nameof(ProjectName)}\":\"{ProjectName}\",");
            }
            if (!String.IsNullOrWhiteSpace(ControllerName))
            {
                sb.Append($"\"{nameof(ControllerName)}\":\"{ControllerName}\",");
            }
            if (!String.IsNullOrWhiteSpace(Sign))
            {
                sb.Append($"\"{nameof(Sign)}\":\"{Sign}\",");
            }
            if(Value == null)
            {
                Value = "";
            }
            if (Value.GetType().IsValueType || Value.GetType() == typeof(String))
            {
                sb.Append($"\"{nameof(Value)}\":\"{Value}\"}}");
            }
            else
            {
                sb.Append($"\"{nameof(Value)}\":\"{Value.ToJson()}\"}}");
            }
            return sb.ToString();
        }
    }
}
