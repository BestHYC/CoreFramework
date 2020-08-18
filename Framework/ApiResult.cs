using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public enum ResultStatusEnum
    {
        Success = 0, Error = 1
    }
    public class ApiResult
    {
        [JsonProperty("code")]
        public ResultStatusEnum Code { get; set; }
        [JsonProperty("message")]
        public String Message { get; set; }
    }
    public class ApiResult<T> : ApiResult
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
    /// <summary>
    /// 延伸的状态码数据
    /// </summary>
    public class ApiStatusCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const Int32 Success = 0;
        /// <summary>
        /// 登录已失效
        /// </summary>
        public const Int32 LogInFail = 1001;
        /// <summary>
        /// 当前时间与服务器不一致
        /// </summary>
        public const Int32 FailTime = 1002;
        /// <summary>
        /// 参数不能为空
        /// </summary>
        public const Int32 NoArgument = 1008;
        /// <summary>
        /// 参数格式不正确
        /// </summary>
        public const Int32 FailArgument = 1010;


        /// <summary>
        /// 不支持的支付方式
        /// </summary>
        public const Int32 FailPayWay = 2001;
        public const Int32 TranceferError = 4000;
        /// <summary>
        /// 关键参数为空(参数不为空,但是关键参数为空)
        /// </summary>
        public const Int32 ArgumentNull = 4001;
        public const Int32 FailPre = 4101;
        /// <summary>
        /// 重复订单号
        /// </summary>
        public const Int32 RepeatOrderId = 4102;
        /// <summary>
        /// 无效订单号
        /// </summary>
        public const Int32 FailOrderId = 4103;
        /// <summary>
        ///  获取汇率异常
        /// </summary>
        public const Int32 FailGetRate = 4011;
        /// <summary>
        ///  转账异常
        /// </summary>
        public const Int32 FailTransefer = 4011;
        /// <summary>
        /// 链和币种相关系统异常
        /// </summary>
        public const Int32 FailSystem = 5000;
        /// <summary>
        /// 不支持币种
        /// </summary>
        public const Int32 FailCoinType = 5002;
        /// <summary>
        /// 无效地址
        /// </summary>
        public const Int32 FailAddress = 5004;
        /// <summary>
        /// 系统内部错误
        /// </summary>
        public const Int32 SystemInnerFail = 9000;
        /// <summary>
        /// 系统异常，请联系客服处理。
        /// </summary>
        public const Int32 InnerFail = 9001;
        /// <summary>
        /// 系统异常，请联系客服处理。
        /// </summary>
        public const Int32 UnKnowStatus = 9002;

    }
}
