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
}
