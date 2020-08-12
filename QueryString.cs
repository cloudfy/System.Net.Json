using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Net
{
    /// <summary>
    /// Provides methods to parse and build querystrings.
    /// </summary>
    public sealed class QueryString
    {
        #region === member variables ===
        /// <summary></summary>
        //private readonly Dictionary<string, string> Parts;

        private readonly List<Tuple<string, string>> Parts;
        #endregion

        #region === constructor ===
        /// <summary>
        /// 
        /// </summary>
        public QueryString()
        {
            Parts = new List<Tuple<string, string>>();
        }
        #endregion

        #region === static ===
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static QueryString Parse(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("Url cannot be null");

            string queryPart;
            if (url.Contains("?"))
                queryPart = url.Substring(url.IndexOf("?")+1);
            else
                queryPart = url;

            QueryString returnValue = new QueryString();

            string[] queryParts = queryPart.Split('&');
            foreach (var part in queryParts)
            {
                returnValue.Add(part);
            }

            return returnValue;
        }
        #endregion

        #region === add ===
        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        internal void Add(string part)
        {
            if (part.Contains("="))
            {
                string[] keyValue = part.Split('=');
                this.Add(keyValue[0], keyValue[1]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            if (value.ToString() == "")
                return;

            string valueString = HttpHelper.UrlDecode(
                value.ToString());

            //if (Parts.ContainsKey(key))
            //    Parts[key] = valueString;
            //else
            //Parts.Add(key, valueString);

            Parts.Add(new Tuple<string, string>(key, valueString));
        }
        #endregion

        #region === tostring ===
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string ToString(string prefix = "?")
        {
            string query = null;
            foreach (var part in Parts)
            {
                if (query != null)
                    query += "&";

                query += part.Item1 + "=" + HttpHelper.UrlEncode(part.Item2);
            }
            
            return (prefix + query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("?");
        }
        /// <summary>
        /// Returns true if one or more keys is found.
        /// </summary>
        /// <param name="key">Key to find.</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            foreach (string storedKey in this.Keys)
            {
                if (key.Equals(storedKey, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        #endregion

        #region === properties ===
        /// <summary>
        /// Gets distict list of keys.
        /// </summary>
        public string[] Keys
        {
            get { return Parts
                    .Select(p => p.Item1)
                    .Distinct()
                    .ToArray(); }
        }
        /// <summary>
        /// Gets all values. Dublicates may exist.
        /// </summary>
        public string[] Values
        {
            get { return Parts.Select(p => p.Item2).ToArray(); }
        } 
        #endregion
    }
}
