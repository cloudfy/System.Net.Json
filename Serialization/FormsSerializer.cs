using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;

namespace System.Net.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    internal static class FormsSerializer
    {
        /// <summary>
        /// Returns a forms serialized string (key=value).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">Request object.</param>
        /// <returns></returns>
        internal static string Serialize<T>(T request)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var prop in request.GetType().GetProperties())
            {
                if (sb.Length > 0)
                    sb.Append("&");

                var value = prop.GetValue(request);
                var name = GetPropertyName(prop);

                if (value != null && value.GetType() == typeof(StringDictionary))
                {
                    // should format 'propertyname[keyname]=value'
                    StringDictionary values = value as StringDictionary;
                    foreach (string key in values.Keys)
                    {
                        sb.Append($"{name.ToLower()}[{key}]={values[key]}");
                    }
                }
                else if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    sb.Append($"{name.ToLower()}={value}");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Returns the name of the PropertyName.
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        private static string GetPropertyName(PropertyInfo pi)
        {
            var jsonInfo = pi.GetCustomAttribute<JsonPropertyAttribute>();

            if (jsonInfo != null)
                return jsonInfo.PropertyName;
            return pi.Name;
        }
    }
}
