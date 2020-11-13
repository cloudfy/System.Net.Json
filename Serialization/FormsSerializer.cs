using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Json.Internals;
using System.Reflection;
using System.Text;

namespace System.Net.Serialization
{
    /// <summary>
    /// Provides serialization of objects into form method post data.
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
                if (prop.ExcludeSerialization())
                    continue;

                var value = prop.GetValue(request);
                var name = GetPropertyName(prop).ToLower();
                name = Encode(name);

                if (sb.Length > 0 && value != null)
                    sb.Append("&");

                if (value != null && value is StringDictionary)
                {
                    // should format 'propertyname[keyname]=value'
                    var values = value as StringDictionary;
                    foreach (string key in values.Keys)
                    {
                        sb.Append($"{name}[{Encode(key)}]={Encode(values[key])}");
                    }
                }
                else if (value != null && value is Dictionary<string, string>)
                {
                    // should format 'propertyname[keyname]=value'
                    var values = value as Dictionary<string, string>;
                    foreach (string key in values.Keys)
                    {
                        sb.Append($"{name}[{Encode(key)}]={Encode(values[key])}");
                    }
                }

                else if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    sb.Append($"{name}={Encode(value.ToString())}");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Forms url encodes the content.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Encode(string value )
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;
            // Escape spaces as '+'.
            return Uri.EscapeDataString(value).Replace("%20", "+");
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
