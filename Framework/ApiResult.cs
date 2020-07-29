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
        public ResultStatusEnum Code { get; set; }
        public String Message { get; set; }
    }
    public class ApiResult<T> : ApiResult
    {
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
        public const String Success = "00000";
        /// <summary>
        /// 登录已失效
        /// </summary>
        public const String LogInFail = "01001";
        /// <summary>
        /// 当前时间与服务器不一致
        /// </summary>
        public const String FailTime = "01002";
        /// <summary>
        /// 参数不能为空
        /// </summary>
        public const String NoArgument = "01008";
        /// <summary>
        /// 参数格式不正确
        /// </summary>
        public const String FailArgument = "01010";


        /// <summary>
        /// 不支持的支付方式
        /// </summary>
        public const String FailPayWay = "02001";
        public const String TranceferError = "04000";
        public const String ArgumentNull = "04001";
        public const String FailPre = "04101";
        /// <summary>
        /// 重复订单号
        /// </summary>
        public const String RepeatOrderId = "04102";
        /// <summary>
        /// 无效订单号
        /// </summary>
        public const String FailOrderId = "04103";
        /// <summary>
        ///  获取汇率异常
        /// </summary>
        public const String FailGetRate = "04011";
        /// <summary>
        ///  转账异常
        /// </summary>
        public const String FailTransefer = "04011";
        /// <summary>
        /// 链和币种相关系统异常
        /// </summary>
        public const String FailSystem = "05000";
        /// <summary>
        /// 不支持币种
        /// </summary>
        public const String FailCoinType = "05002";
        /// <summary>
        /// 无效地址
        /// </summary>
        public const String FailAddress = "05004";
        /// <summary>
        /// 系统内部错误
        /// </summary>
        public const String SystemInnerFail = "09000";
        /// <summary>
        /// 系统异常，请联系客服处理。
        /// </summary>
        public const String InnerFail = "09001";
        /// <summary>
        /// 系统异常，请联系客服处理。
        /// </summary>
        public const String UnKnowStatus = "09002";

    }
}
