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
        private string _statusDescription;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <param name="statusDescription"></param>
        public FormClientException(string message
            , System.Net.HttpStatusCode statusCode
            , string statusDescription) : base(message)
        {
            _statusCode = statusCode;
            _statusDescription = statusDescription;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <param name="statusDescription"></param>
        /// <param name="innerException"></param>
        public FormClientException(string message
            , System.Net.HttpStatusCode statusCode
            , string statusDescription
            , Exception innerException) : base(message, innerException)
        {
            _statusCode = statusCode;
            _statusDescription = statusDescription;
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
        /// <summary>
        /// 
        /// </summary>
        public string StatusDescription
        {
            get
            {
                return _statusDescription;
            }
        }
        #endregion
    }
}
