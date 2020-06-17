using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    /// <summary>
    /// HTTP服务日志中间件
    /// <para>支持Request、Response信息输出</para>
    /// <para>支持请求处理耗时输出</para>
    /// </summary>
    public class LoggerMiddleware
    {
        const string _FileName = "请求日志";

        private readonly RequestDelegate _Next;

        /// <summary>
        /// 不记录日志的请求类型
        /// </summary>
        /// <summary>
        /// 日志中间件
        /// </summary>
        /// <param name="next"></param>
        public LoggerMiddleware(RequestDelegate next)
        {
            _Next = next;
        }

        /// <summary>
        /// 请求拦截处理
        /// </summary>
        /// <param name="context">HTTP请求</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            string fPath = context.Request.Path.ToString().ToLower();
            int fIndex = fPath.LastIndexOf(".");
            if (fIndex != -1)
            {
                await _Next(context);
                return;
            }
            context.Request.EnableBuffering();
            var requestReader = new StreamReader(context.Request.Body);
            var requestContent = await requestReader.ReadToEndAsync();
            StringBuilder fLog = new StringBuilder();
            fLog.AppendLine($"Request:{context.Request.Method}:{context.Request.Path.ToString()}{context.Request.QueryString}");
            fLog.AppendLine($"Request Body:{requestContent}");
            //YLog.Append(fLog.ToString(), _FileName, YConst.mFolderName);
            context.Request.Body.Position = 0;

            Stream originalBody = context.Response.Body;
            try
            {
                using (var ms = new MemoryStream())
                {
                    context.Response.Body = ms;
                    var fWatch = new Stopwatch();
                    fWatch.Start();
                    await _Next(context);
                    fWatch.Stop();
                    ms.Position = 0;
                    string responseBody = new StreamReader(ms).ReadToEnd();
                    fLog.Append($"Response Body({fWatch.ElapsedMilliseconds}ms):\r\n{responseBody}");
                    ms.Position = 0;
                    await ms.CopyToAsync(originalBody);
                }
                LogHelper.Info(fLog.ToString());
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
