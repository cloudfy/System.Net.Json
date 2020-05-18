using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Json
{
    /// <summary>
    /// Represents errors that occur during Json requests.
    /// </summary>
    public class JsonClientException : Exception
    {
        #region === member variables ===
        /// <summary>Member variable for status code.</summary>
        private System.Net.HttpStatusCode _statusCode;
        #endregion

        #region === constructor ===
        /// <summary>
        /// Returns a new instace of the exception class.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <param name="statusCode">Status code from the request.</param>
        public JsonClientException(string message, System.Net.HttpStatusCode statusCode) : base(message)
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

