using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Net
{
    /// <summary>
    /// Basic helper method(s) for Http.
    /// </summary>
    public sealed class HttpHelper
    {
        #region === basic authentication ===
        /// <summary>
        /// Returns a basic authentication header in value "Basic xxxx"
        /// </summary>
        /// <param name="userName">Username of authentication.</param>
        /// <param name="password">Password of authentication.</param>
        /// <returns></returns>
        public static string GetBasicAuthentication(string userName, string password)
        {
            var bytes = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", userName, password));
            return "Basic " + Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// Returns a basic authentication header in value "Basic xxxx"
        /// </summary>
        /// <param name="userName">Username of authentication.</param>
        /// <returns></returns>
        public static string GetBasicAuthentication(string userName)
        {
            var bytes = Encoding.ASCII.GetBytes(string.Format("{0}:", userName));
            return "Basic " + Convert.ToBase64String(bytes);
        } 
        #endregion

        /// <summary>
        /// Returns a dictionary of basic Json header like Accept and Content-type.
        /// </summary>
        /// <returns></returns>
        public static StringDictionary GetJsonHeaders()
        {
            return new StringDictionary
            {
                { "content-type", "application/json" },
                { "accept", "application/json" }
            };
        }
        /// <summary>
        /// Returns a dictionary of basic form headers (x-www-form-urlencoded).
        /// </summary>
        /// <returns></returns>
        public static StringDictionary GetFormHeaders()
        {
            return new StringDictionary
            {
                { "content-type", "application/x-www-form-urlencoded" }
            };
        }
    }
}
