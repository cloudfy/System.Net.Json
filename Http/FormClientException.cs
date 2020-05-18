using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class FormClientException : Exception
    {
        #region === member variables ===
        /// <summary>Member variable for status code.</summary>
        private System.Net.HttpStatusCode _statusCode;
        #endregion

        #region === constructor ===
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        public FormClientException(string message
            , System.Net.HttpStatusCode statusCode) : base(message)
        {
            _statusCode = statusCode;
        } 
        #endregion

        #region === public properties ===
        /// <summary>
        /// Gets the status code of the exception.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return _statusCode;
            }
        }
        #endregion
    }
}
