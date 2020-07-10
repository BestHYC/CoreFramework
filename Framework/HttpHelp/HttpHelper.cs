using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class HttpHelper
    {
        /// <summary>
        /// 同步请求post（键值对形式）
        /// </summary>
        /// <param name="url">网络的地址("/api/UMeng")</param>
        /// <param name="formData">键值对List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();formData.Add(new KeyValuePair<string, string>("userid", "29122"));formData.Add(new KeyValuePair<string, string>("umengids", "29122"));</param>
        /// <param name="charset">编码格式</param>
        /// <param name="mediaType">头媒体类型</param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, Dictionary<string, string> dic)
        {
            HttpClient client = new HttpClient();
            FormUrlEncodedContent data = new FormUrlEncodedContent(dic);
            var r = await client.PostAsync(url, data);
            return await r.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// Post 请求，无参
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, string content = "")
        {
            HttpClient client = new HttpClient();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] bytes = Encoding.Unicode.GetBytes(content);
                ms.Write(bytes, 0, bytes.Length);
                HttpContent hc = new StreamContent(ms);
                HttpResponseMessage resp = await client.PostAsync(url, null);
                return await resp.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// 发起POST同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>        
        /// <returns></returns>
        public static string Post(string url, string postData = null, Dictionary<string, string> headers = null, string contentType = "application/json", int timeOut = 30)
        {
            postData = postData ?? "";
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(timeOut);
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
                {
                    if (contentType != null)
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                    HttpResponseMessage response = client.PostAsync(url, httpContent).Result;
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
        /// <summary>
        /// 发起GET同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Get(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            using (HttpClient client = new HttpClient())
            {
                if (contentType != null)
                    client.DefaultRequestHeaders.Add("ContentType", contentType);
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                HttpResponseMessage response = client.GetAsync(url).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
