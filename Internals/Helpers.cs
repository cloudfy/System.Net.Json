using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Assert the value to be required and throws an exception is empty.
        /// </summary>
        /// <param name="value">Object value.</param>
        internal static void Assert(object value)
        {
            if (value == null)
                throw new ArgumentNullException(value.GetType().ToString());
        }
        /// <summary>
        /// Assert the value to be required and throws an exception is empty.
        /// </summary>
        /// <param name="value">String of value.</param>
        internal static void Assert(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(value);
        }
    }
}
