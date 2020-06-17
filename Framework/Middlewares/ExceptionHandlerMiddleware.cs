using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly LogHelper<ExceptionHandlerMiddleware> m_logger;

        public ExceptionHandlerMiddleware(RequestDelegate rd)
        {
            requestDelegate = rd;
            m_logger = LogHelper<ExceptionHandlerMiddleware>.Instance;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            if (e == null) return;
            await WriteExceptionAsync(context, e).ConfigureAwait(false);
        }

        private async Task WriteExceptionAsync(HttpContext context, Exception e)
        {
            m_logger.LogWarning(e.ToString());
            ApiResult apiResult = new ApiResult();
            apiResult.Code = ResultStatusEnum.Error;
            apiResult.Message = e.Message?.ToString();
            context.Response.ContentType = "text/plain;charset=utf-8";
            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(apiResult));
        }
    }
}
