using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Net.Serialization
{
    internal static class FormsSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        internal static string Serialize<T>(T request)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var prop in request.GetType().GetProperties())
            {
                if (sb.Length > 0)
                    sb.Append("&");

                var value = prop.GetValue(request);
                if (value != null && value.GetType() == typeof(StringDictionary))
                {
                    // should format 'propertyname[keyname]=value'
                    StringDictionary values = value as StringDictionary;
                    foreach (string key in values.Keys)
                    {
                        sb.Append($"{prop.Name.ToLower()}[{key}]={values[key]}");
                    }
                }
                else if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    sb.Append($"{prop.Name.ToLower()}={value}");
                }
            }
            return sb.ToString();
        }
    }
}
