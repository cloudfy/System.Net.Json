using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace System.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FormClient
    {
        #region === constructor ===
        /// <summary>
        /// 
        /// </summary>
        public FormClient() { }
        #endregion

        #region === POST ===
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">URL of request.</param>
        /// <param name="values"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string Post(string url, StringDictionary values, StringDictionary headers)
        {
            string body = GetFormsBody(values);
            return ExecuteRequest("POST", url, body
                , headers);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Post(string url, StringDictionary headers, params Tuple<string, string>[] values)
        {
            string body = GetFormsBody(values);
            return ExecuteRequest("POST", url, body
                , headers);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string Post<T>(string url, StringDictionary headers, T request)
        {
            string body = Serialization.FormsSerializer.Serialize(request);
            return ExecuteRequest("POST", url, body
                , headers);
        }
        #endregion

        #region === private methods ===
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
        /// <returns></returns>
        /// <exception cref="FormClientException"></exception>
        private static string ExecuteRequest(string method, string url, string body, StringDictionary headers)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = method.ToUpper();
            request.ContentType = "application/x-www-form-urlencoded";
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

            try
            {
                if (!string.IsNullOrEmpty(body) && (request.Method == "POST" | request.Method == "PUT" | request.Method == "PATCH"))
                {
                    // write body as UTF-8 encoding
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] bytes = encoding.GetBytes(body);

                    // set content length
                    request.ContentLength = bytes.Length;

                    using (Stream requestStream = request.GetRequestStream())
                        requestStream.Write(bytes, 0, bytes.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK |
                        response.StatusCode == HttpStatusCode.Accepted |
                        response.StatusCode == HttpStatusCode.Created |
                        response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return ReadResponse(response.GetResponseStream());
                    }
                    else
                    {
                        throw new Exception();
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
                    var text = ReadResponse(e.Response.GetResponseStream());
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
        private static string ReadResponse(Stream s)
        {
            using (var reader = new StreamReader(s))
            {
                return reader.ReadToEnd();
            }
        } 
        #endregion
    }
}
