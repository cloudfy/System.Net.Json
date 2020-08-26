using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Json
{
    /// <summary>
    /// Async methods.
    /// </summary>
    public static partial class JsonClient
    {
        #region === GET ===
        /// <summary></summary>
        /// <param name="url">Url of request.</param>
        /// <param name="headers">Header of request.</param>
        /// <returns>TResponse</returns>
        public static async Task<TResponse> GetAsync<TResponse>(string url, StringDictionary headers)
        {
            return await ExecuteRequestAsync<Object, TResponse>("GET", url, null, headers);
        }
        #endregion

        #region === POST Async ===
        /// <summary>Provides a serialized POST method for an API.</summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url">Url of the request.</param>
        /// <param name="request">Body of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <returns></returns>
        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string url
            , TRequest request, StringDictionary headers)
        {
            return await ExecuteRequestAsync<TRequest, TResponse>("POST", url, request, headers);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="headers"></param>
        /// <param name="jsonSerializerSettings"></param>
        /// <returns></returns>
        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string url
            , TRequest request, StringDictionary headers, Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings)
        {
            return await ExecuteRequestAsync<TRequest, TResponse>("POST", url, request, headers, jsonSerializerSettings);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="headers"></param>
        public async static void PostNoResponseAsync<TRequest>(string url, TRequest request, StringDictionary headers)
        {
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            await ExecuteRequestAsync("POST", url, requestBody, headers);
        }
        /// <summary>
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="headers"></param>
        public async static Task<string> PostAsync<TRequest>(string url, TRequest request, StringDictionary headers)
        {
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            return await ExecuteRequestAsync("POST", url, requestBody, headers);
        }
        #endregion

        #region === PUT ===
        /// <summary>
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="headers"></param>
        public static async Task<TResponse> PutAsync<TRequest, TResponse>(string url
            , TRequest request, StringDictionary headers)
        {
            return await ExecuteRequestAsync<TRequest, TResponse>("PUT", url, request, headers);
        }
        #endregion

        #region === private methods ===
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="headers"></param>
        /// <param name="jsonSerializerSettings"></param>
        /// <returns></returns>
        private async static Task<TResponse> ExecuteRequestAsync<TRequest, TResponse>(
            string method
            , string url
            , TRequest request
            , StringDictionary headers
            , Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings = null)
        {
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request,
                Newtonsoft.Json.Formatting.None,
                jsonSerializerSettings ?? GetJsonSerializerSettings()
            );

            string responseBody = await ExecuteRequestAsync(method, url, requestBody, headers);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(responseBody, GetJsonSerializerSettings());
        }

        /// <summary>
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        private async static Task<string> ExecuteRequestAsync(string method, string url, string body, StringDictionary headers)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.ToUpper();
            request.AllowAutoRedirect = true;
            request.Timeout = 50000;

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    if (key.ToLower() == "content-type")
                        request.ContentType = headers[key];
                    else if (key.ToLower() == "user-agent")
                        request.UserAgent = headers[key];
                    else if (key.ToLower() == "accept")
                        request.Accept = headers[key];
                    else if (key.ToLower() == "content-length")
                        // nothing
                        continue;
                    else
                        request.Headers.Add(key, headers[key]);
                }
            }

            if (request.Method == "POST" | request.Method == "PUT" | request.Method == "PATCH")
            {
                // write body as UTF-8 encoding
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(body);

                // set content length
                request.ContentLength = bytes.Length;

                using (Stream requestStream = await request.GetRequestStreamAsync())
                    requestStream.Write(bytes, 0, bytes.Length);
            }

            try
            {
                using (var asyncResponse = await request.GetResponseAsync())
                {
                    HttpWebResponse response = asyncResponse as HttpWebResponse;

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
                var text = ReadResponse(e.Response.GetResponseStream());
                throw new JsonClientException(text, (e.Response as HttpWebResponse).StatusCode);
                //throw new ApplicationException(string.Format("Bad response {0}. {1}", e.Status, text), e);
            }
            catch (Exception e)
            {
                throw e;
            }
        } 
        #endregion
    }
}
