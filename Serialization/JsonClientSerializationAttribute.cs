using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Json.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class JsonClientSerializationAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exclude"></param>
        public JsonClientSerializationAttribute(JsonClientSerializationExclude exclude = JsonClientSerializationExclude.Any) 
        {
            this.Exclude = exclude;
        }
        /// <summary>
        /// 
        /// </summary>
        public JsonClientSerializationExclude Exclude { get; private set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum JsonClientSerializationExclude
    {
        Any,
        Post,
        Get,
        Patch,
        Put
    }
}
