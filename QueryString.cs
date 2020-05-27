using System.Collections.Generic;

namespace System.Net
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class QueryString
    {
        #region === member variables ===
        /// <summary></summary>
        private readonly Dictionary<string, string> Parts; 
        #endregion

        #region === constructor ===
        /// <summary>
        /// 
        /// </summary>
        public QueryString()
        {
            Parts = new Dictionary<string, string>();
        }
        #endregion

        #region === add ===
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            if (value.ToString() == "")
                return;

            if (Parts.ContainsKey(key))
                Parts[key] = value.ToString();
            else
                Parts.Add(key, value.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddWithNull(string key, object value)
        {
            if (Parts.ContainsKey(key))
                Parts[key] = value.ToString();
            else
                Parts.Add(key, value.ToString());
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
            foreach (var key in Parts.Keys)
            {
                if (query != null)
                    query += "&";

                query += key + "=" + Parts[key];
            }
            return prefix + query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("?");
        } 
        #endregion
    }
}
