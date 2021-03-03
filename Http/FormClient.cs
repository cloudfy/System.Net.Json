using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Infrastructure;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    /// Provides methods to handle form based request processing.
    /// </summary>
    public sealed class FormClient
    {
        #region === constructor ===
        /// <summary>
        /// Constructor.
        /// </summary>
        public FormClient() { }
        #endregion

        #region === IRequestDebugger implementation ===
        /// <summary>
        /// 
        /// </summary>
        public static IRequestDebugger Debugger { get; set; } 
        #endregion

        #region === POST ===
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">URL of request.</param>
        /// <param name="values"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, StringDictionary values, StringDictionary headers
            , Action<WebRequest> onRequestDelegate = null)
        {
            string body = GetFormsBody(values);
            return await ExecuteRequestAsync("POST", url, body
                , headers, onRequestDelegate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="values"></param>
        /// <param name="headers"></param>
        /// <param name="onRequestDelegate"></param>
        /// <returns></returns>
        public static string Post(
            string url
            , StringDictionary values
            , StringDictionary headers
            , Action<WebRequest> onRequestDelegate = null) 
            => PostAsync(url, values, headers, onRequestDelegate).Result;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Post(string url, StringDictionary headers
            , Action<WebRequest> onRequestDelegate = null
            , params Tuple<string, string>[] values) 
            => PostAsync(url, headers, onRequestDelegate, values).Result;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="onRequestDelegate"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, StringDictionary headers
            , Action<WebRequest> onRequestDelegate = null
            , params Tuple<string, string>[] values)
        {
            string body = GetFormsBody(values);
            return await ExecuteRequestAsync("POST", url, body
                , headers, onRequestDelegate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync<T>(string url, StringDictionary headers, T request, Action<WebRequest> onRequestDelegate = null)
        {
            string body = Serialization.FormsSerializer.Serialize(request);

            DebugSerialization(body, request);

            return await ExecuteRequestAsync("POST", url, body
                , headers, onRequestDelegate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="request"></param>
        /// <param name="onRequestDelegate"></param>
        /// <returns></returns>
        public static string Post<T>(string url, StringDictionary headers
            , T request, Action<WebRequest> onRequestDelegate = null) 
            => PostAsync(url, headers, request, onRequestDelegate).Result;
        #endregion

        #region === private methods ===
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="obj"></param>
        private static void DebugSerialization(string body, object obj)
        {
            if (FormClient.Debugger != null && body != null)
                FormClient.Debugger.OnSerialization(body, obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        private static void DebugRequest(WebRequest request)
        {
            if (FormClient.Debugger != null && request != null)
                FormClient.Debugger.OnRequest(request);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        private static void DebugResponse(WebResponse response)
        {
            if (FormClient.Debugger != null && response != null)
                FormClient.Debugger.OnResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static string GetFormsBody(Tuple<string, string>[] values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in values)
            {
                if (sb.Length > 0)
                    sb.Append("&");
                sb.Append($"{item.Item1}={item.Item2}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static string GetFormsBody(StringDictionary values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in values.Keys)
            {
                if (sb.Length > 0)
                    sb.Append("&");
                sb.Append($"{key}={values[key]}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="headers"></param>
        /// <param name="onRequestDelegate"></param>
        /// <returns></returns>
        /// <exception cref="FormClientException"></exception>
        private static string ExecuteRequest(string method, string url, string body
            , StringDictionary headers
            , Action<WebRequest> onRequestDelegate) 
            => ExecuteRequestAsync(method, url, body, headers, onRequestDelegate).Result;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="headers"></param>
        /// <param name="onRequestDelegate"></param>
        /// <returns></returns>
        private static async Task<string> ExecuteRequestAsync(string method, string url, string body
            , StringDictionary headers
            , Action<WebRequest> onRequestDelegate)
        {
            // prepare request
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = method.ToUpper();
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            request.KeepAlive = true;
            request.AllowAutoRedirect = true;
            request.Timeout = 50000;

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    if (key.ToLower() == "content-type")
                        request.ContentType = headers[key];
                    else if (key.ToLower() == "accept")
                        request.Accept = headers[key];
                    else if (key.ToLower() == "content-length")
                        // nothing
                        continue;
                    else
                        request.Headers.Add(key, headers[key]);
                }
            }
            onRequestDelegate?.Invoke(request);

            DebugRequest(request);

            try
            {
                if (!string.IsNullOrEmpty(body) && (request.Method == "POST" | request.Method == "PUT" | request.Method == "PATCH"))
                {
                    // write body as UTF-8 encoding
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] bytes = encoding.GetBytes(body);

                    // set content length
                    request.ContentLength = bytes.Length;

                    using (Stream requestStream = await request.GetRequestStreamAsync())
                        await requestStream.WriteAsync(bytes, 0, bytes.Length);
                }

                using (var firstResponse = await request.GetResponseAsync())
                {
                    var response = (HttpWebResponse)firstResponse;
                    DebugResponse(response);

                    if (response.StatusCode == HttpStatusCode.OK |
                        response.StatusCode == HttpStatusCode.Accepted |
                        response.StatusCode == HttpStatusCode.Created |
                        response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return await ReadResponseAsync(response.GetResponseStream());
                    }
                    else
                    {
                        throw new Exception("Unknown status code");
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure ||
                    e.Status == WebExceptionStatus.ConnectionClosed ||
                    e.Status == WebExceptionStatus.Timeout)
                    throw e;

                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var text = await ReadResponseAsync(e.Response.GetResponseStream());
                    throw new FormClientException(text
                        , (e.Response as HttpWebResponse).StatusCode
                        , (e.Response as HttpWebResponse).StatusDescription);
                }
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string ReadResponse(Stream s) => ReadResponseAsync(s).Result;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static async Task<string> ReadResponseAsync(Stream s)
        {
            using (var reader = new StreamReader(s))
            {
                return await reader.ReadToEndAsync();
            }
        }
        #endregion
    }
}
