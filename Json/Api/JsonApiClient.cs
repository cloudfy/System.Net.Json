﻿using System;
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
        /// 
        /// </summary>
        /// <param name="url"></param>
        protected virtual bool Delete(string url)
        {
            try
            {
                return JsonClient.Delete(url, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                return false;
            }
        }
        /// <summary>
        /// Performs a HTTP GET.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Url of action request.</param>
        /// <returns></returns>
        protected virtual T Get<T>(string url) where T : class
        {
            try
            {
                return JsonClient.Get<T>(url, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                return null;
            }
        }

        /// <summary>
        /// Performs a HTTP GET asynchroniously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual async Task<T> GetAsync<T>(string url) where T : class
        {
            try
            {
                return await JsonClient.GetAsync<T>(url, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                return null;
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
        protected virtual R Post<T, R>(string url, T value) where R : class where T : class
        {
            try
            {
                return JsonClient.Post<T, R>(url, value, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                return null;
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
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual R Put<T, R>(string url, T value) where R : class where T : class
        {
            try
            {
                return JsonClient.Put<T, R>(url, value, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                return null;
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
        protected virtual async Task<R> PutAsync<T, R>(string url, T request)
        {
            try
            {
                return await JsonClient.PutAsync<T, R>(url, request, this.GetHeaders());
            }
            catch (Exception e)
            {
                if (HandleException(e))
                    throw e;

                return default;
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
        protected internal virtual bool HandleException(Exception e)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected internal virtual bool TryHandleException(Exception ex, out Exception e)
        {
            e = ex;
            return true;
        }
    }
}
