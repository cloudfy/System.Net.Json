using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;

namespace System.Net.Json
{
    /// <summary>
    /// Provides methods for communicating with Json endpoints.
    /// </summary>
    public static partial class JsonClient
    {
        #region === static methods ===
        /// <summary>Converts a string into a base64 string.</summary>
        /// <param name="input">String to convert.</param>
        /// <returns>string</returns>
        public static string ToBase64(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }
        /// <summary>
        /// Converts an object to a base64 string.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="input">Object to serialize.</param>
        /// <returns></returns>
        public static string ToBase64<T>(T input)
        {
            string jsonValue = JsonConvert.SerializeObject(input);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonValue));
        }
        /// <summary>Convert a Base64 string into a string.</summary>
        /// <param name="output">Base64 string to convert.</param>
        /// <returns></returns>
        public static string FromBase64(string output)
        {
            var bytes = Convert.FromBase64String(output);
            return Encoding.UTF8.GetString(bytes);
        }
        /// <summary>Converts a Base64 string into an object.</summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <param name="output">Base64 string to convert.</param>
        /// <returns></returns>
        public static T FromBase64<T>(string output)
        {
            if (string.IsNullOrEmpty(output))
                throw new ArgumentNullException("output");

            var bytes = Convert.FromBase64String(output);
            string outputString = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(outputString);
        }
        #endregion

        #region === PUT ===
        /// <summary>Provides a serialized PUT method for an API.</summary>
        /// <param name="url">Url of the request.</param>
        /// <param name="body">Body of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <returns>string</returns>
        public static string Put(string url, string body, StringDictionary headers)
        {
            return ExecuteRequest("PUT", url, body, headers);
        }

        /// <summary>Provides a serialized PUT method for an API.</summary>
        /// <param name="url">Url of the request.</param>
        /// <param name="requestBody">Body of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <returns>TResponse</returns>
        public static TResponse Put<TResponse>(string url, string requestBody, StringDictionary headers)
        {
            string responseBody = ExecuteRequest("PUT", url, requestBody, headers);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(responseBody, GetJsonSerializerSettings());
        }

        /// <summary>Provides a serialized PUT method for an API.</summary>
        /// <param name="url">Url of the request.</param>
        /// <param name="request">Value of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <returns>TResponse</returns>
        public static void Put<TRequest>(string url, TRequest request, StringDictionary headers)
        {
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            ExecuteRequest("PUT", url, requestBody, headers);
        }

        /// <summary>Provides a serialized PUT method for an API.</summary>
        /// <param name="url">Url of the request.</param>
        /// <param name="request">Request of the method.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <returns>TResponse</returns>
        public static TResponse Put<TRequest, TResponse>(string url, TRequest request, StringDictionary headers)
        {
            return ExecuteRequest<TRequest, TResponse>("PUT", url, request, headers);
        }
        #endregion

        #region === POST ===
        /// <summary>Provides a serialized POST method for an API.</summary>
        /// <param name="url">Url of the request.</param>
        /// <param name="body">Body of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <returns>string</returns>
        public static string Post(string url, string body, StringDictionary headers)
        {
            return ExecuteRequest("POST", url, body, headers);
        }

        /// <summary>Provides a serialized POST method for an API.</summary>
        /// <param name="url">Url of the request.</param>
        /// <param name="requestBody">Body of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <param name="onRequestDelegate">Delegate to forward to the request.</param>
        /// <returns>TResponse</returns>
        public static TResponse Post<TResponse>(string url, string requestBody, StringDictionary headers, Action<WebRequest> onRequestDelegate = null)
        {
            string responseBody = ExecuteRequest("POST", url, requestBody, headers, onRequestDelegate);
            return JsonConvert.DeserializeObject<TResponse>(responseBody, GetJsonSerializerSettings());
        }
        /// <summary>
        /// Provides a serialized POST method for an API.
        /// </summary>
        /// <typeparam name="TRequest">Type of request body.</typeparam>
        /// <param name="url">Url of the request.</param>
        /// <param name="request">Body of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <param name="onRequestDelegate">Delegate to forward to the request.</param>
        public static void Post<TRequest>(string url, TRequest request, StringDictionary headers, Action<WebRequest> onRequestDelegate = null)
        {
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            ExecuteRequest("POST", url, requestBody, headers, onRequestDelegate);
        }
        /// <summary>
        /// Provides a serialized POST method for an API.
        /// </summary>
        /// <typeparam name="TRequest">Type of request body.</typeparam>
        /// <param name="url">Url of the request.</param>
        /// <param name="request">Body of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <param name="onRequestDelegate">Delegate to forward to the request.</param>
        [Obsolete("PostNoResponse is obsolete, use Post.")]
        public static void PostNoResponse<TRequest>(string url, TRequest request, StringDictionary headers, Action<WebRequest> onRequestDelegate = null)
        {
            Post<TRequest>(url, request, headers, onRequestDelegate);
        }
        /// <summary>Provides a serialized POST method for an API.</summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url">Url of the request.</param>
        /// <param name="request">Body of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <param name="onRequestDelegate">Delegate to forward to the request.</param>
        /// <returns>TResponse</returns>
        public static TResponse Post<TRequest, TResponse>(string url, TRequest request, StringDictionary headers, Action<WebRequest> onRequestDelegate = null)
        {
            return ExecuteRequest<TRequest, TResponse>("POST", url, request, headers);
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
        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, StringDictionary headers)
        {
            return await ExecuteRequestAsync<TRequest, TResponse>("POST", url, request, headers);
        }
        #endregion

        #region === GET ===
        /// <summary></summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns>TResponse</returns>
        public static TResponse Get<TResponse>(string url, StringDictionary headers)
        {
            return ExecuteRequest<Object, TResponse>("GET", url, null, headers);
        }
        /// <summary></summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns>string</returns>
        public static string Get(string url, StringDictionary headers)
        {
            return ExecuteRequest("GET", url, null, headers);
        }
        #endregion

        #region === DEL ===
        /// <summary></summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns>bool</returns>
        public static bool Delete(string url, StringDictionary headers)
        {
            var response = ExecuteRequest("DELETE", url, null, headers);
            return true;
        }
        #endregion

        #region === PATCH ===
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static TResponse Patch<TResponse>(string url, Dictionary<string, object> request, StringDictionary headers)
        {
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var response = ExecuteRequest("PATCH", url, requestBody, headers);

            return JsonConvert.DeserializeObject<TResponse>(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse">Type of response object.</typeparam>
        /// <param name="url">Url of the request.</param>
        /// <param name="requestBody">Body of the request.</param>
        /// <param name="headers">HTTP headers of the request.</param>
        /// <returns></returns>
        public static TResponse Patch<TResponse>(string url, string requestBody, StringDictionary headers)
        {
            var response = ExecuteRequest("PATCH", url, requestBody, headers);

            return JsonConvert.DeserializeObject<TResponse>(response);
        }
        #endregion

        #region === private REQUEST LOGIC ===
        /// <summary></summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="headers"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns>TResponse</returns>
        private static TResponse ExecuteRequest<TRequest, TResponse>(string method, string url, TRequest request, StringDictionary headers)
        {
            try
            {
                string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request,
                    Newtonsoft.Json.Formatting.None,
                    GetJsonSerializerSettings()
                );

                string responseBody = ExecuteRequest(method, url, requestBody, headers);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(responseBody, GetJsonSerializerSettings());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="headers"></param>
        /// <param name="onRequestDelete"></param>
        /// <returns></returns>
        /// <exception cref="WebException"></exception>
        /// <exception cref="Exception"></exception>
        private static string ExecuteRequest(string method, string url, string body, StringDictionary headers, Action<WebRequest> onRequestDelete)
        {
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
                    else if (key.ToLower() == "accept")
                        request.Accept = headers[key];
                    else if (key.ToLower() == "content-length")
                        // nothing
                        continue;
                    else
                        request.Headers.Add(key, headers[key]);
                }
            }
            onRequestDelete?.Invoke(request);

            if (request.Method == "POST" | request.Method == "PUT" | request.Method == "PATCH")
            {
                // write body as UTF-8 encoding
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(body);

                // set content length
                request.ContentLength = bytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(bytes, 0, bytes.Length);
            }

            try
            {
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
                var text = ReadResponse(e.Response.GetResponseStream());
                throw new JsonClientException(text, (e.Response as HttpWebResponse).StatusCode, e);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary></summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="headers"></param>
        /// <returns>string</returns>
        private static string ExecuteRequest(string method, string url, string body, StringDictionary headers)
        {
            return ExecuteRequest(method, url, body, headers, null);
        }
        #endregion

        #region === private statics ===
        /// <summary>
        /// Reads the response stream.
        /// </summary>
        /// <param name="s">Stream to read.</param>
        /// <returns></returns>
        private static string ReadResponse(Stream s)
        {
            using (var reader = new StreamReader(s))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Returns common serialization settings.
        /// </summary>
        /// <returns></returns>
        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings { 
                NullValueHandling = NullValueHandling.Ignore
                
            };
        }
        #endregion
    }
}
