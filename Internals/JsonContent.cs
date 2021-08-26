using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    internal class JsonContent : StringContent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        internal JsonContent(object value) 
            : base(Serialize(value), Encoding.UTF8, "application/json")
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Serialize(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
    }
}
