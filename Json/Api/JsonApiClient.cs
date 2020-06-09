using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net.Json.Infrastructure;
using System.Threading.Tasks;

namespace System.Net.Json.Api
{
    /// <summary>
    /// Provides an abstract class for implementing an API client consuming an JSON/REST end-point.
    /// </summary>
    public abstract class JsonApiClient : IApiClient
    {
        #region === JSON HTTP ===
        /// <summary>
        /// Performs a HTTP GET.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Url of action request.</param>
        /// <returns></returns>
        protected virtual T Get<T>(string url)
        {
            try
            {
                return JsonClient.Get<T>(url, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                return default;
            }
        }

        /// <summary>
        /// Performs a HTTP GET asynchroniously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual async Task<T> GetAsync<T>(string url)
        {
            try
            {
                return await JsonClient.GetAsync<T>(url, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                return default;
            }
        }

        /// <summary>
        /// Performs a HTTP POST.
        /// </summary>
        /// <typeparam name="T">Object type of request.</typeparam>
        /// <typeparam name="R">Object type of response.</typeparam>
        /// <param name="url">Url of action request.</param>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        protected virtual R Post<T, R>(string url, T value)
        {
            try
            {
                return JsonClient.Post<T, R>(url, value, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                throw e;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual async Task<R> PostAsync<T, R>(string url, T request)
        {
            try
            {
                return await JsonClient.PostAsync<T, R>(url, request, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                return default;
            }
        }

        /// <summary>
        /// Performs a HTTP POST.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="url">Url of action request.</param>
        /// <param name="value">Value.</param>
        protected virtual void Post<T>(string url, T value)
        {
            try
            {
                JsonClient.PostNoResponse<T>(url, value, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;
            }
        }
        #endregion

        /// <summary>
        /// Returns a dictionary of headers for the requests.
        /// </summary>
        /// <returns></returns>
        protected abstract StringDictionary GetHeaders();

        /// <summary>
        /// Trap and handle the exception.
        /// </summary>
        /// <param name="e">Exception to handle.</param>
        /// <returns>True to throw exception in implementation.</returns>
        protected virtual bool HandleException(Exception e)
        {
            return true;
        }

        //protected internal abstract void MustTest();

        //// visible to inherited classes.
        //protected internal virtual void Test()
        //{

        //}
        //// // visible to inherited classes.
        //protected virtual void Test2()
        //{

        //}
        //// not visible to interited classes (new assembly)
        //internal virtual void Test3()
        //{

        //}
    }
}
