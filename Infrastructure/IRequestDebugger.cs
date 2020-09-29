using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestDebugger
    {
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        void OnRequest(WebRequest request);
        /// <summary>
        /// </summary>
        /// <param name="response"></param>
        void OnResponse(WebResponse response);
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="obj"></param>
        void OnSerialization(string value, object obj);
    }
}
