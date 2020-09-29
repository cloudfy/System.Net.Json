using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Infrastructure;
using System.Net.Json.Serialization;
using System.Reflection;
using System.Text;

namespace System.Net.Json.Internals
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Excludes the property from serialization.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        internal static bool ExcludeSerialization(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetCustomAttribute(typeof(JsonIgnoreAttribute)) != null)
                return true;

            var serializationAttr = propertyInfo.GetCustomAttribute<JsonClientSerializationAttribute>();
            if (serializationAttr != null 
                && serializationAttr.Exclude != JsonClientSerializationExclude.Get)
                return true;

            return false;
        }
    }
}
