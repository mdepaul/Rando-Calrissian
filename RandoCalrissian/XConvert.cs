/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/XConvert.git
 * **/
using System;
using System.Collections.Generic;
using System.Text;

namespace MD.RandoCalrissian
{
    /// <summary>
    /// Conversion utilities as extention methods
    /// </summary>
    public static class XConvert
    {
        public static int ToInt32(this byte byteValue)
        {
            return Convert.ToInt32(byteValue);
        }

        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] FromBase64String(this string input)
        {
            return Convert.FromBase64String(input);
        }

        public static string ToSafeBase64String(this byte[] bytes)
        {
            return ToBase64String(bytes).Replace("/", ".").Replace("+", "_").Replace("==", "--");
        }

        public static byte[] FromSafeBase64String(this string input)
        {
            return Convert.FromBase64String(input.Replace(".", "/").Replace("_", "+").Replace("--", "=="));
        }

        public static string ToHex(this byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString().ToUpper();
        }
        public static byte[] FromHex(this string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string Sort(this string value)
        {
            StringBuilder sb = new StringBuilder(value.Length);
            SortedSet<char> set = GetSortedSet(value);
            foreach (var item in set)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }

        public static string Reverse(this string value)
        {
            StringBuilder sb = new StringBuilder("".PadRight(value.Length, ' '), value.Length);
            int pos = value.Length - 1;
            foreach (var theCharacter in value.ToCharArray())
            {
                sb[pos] = theCharacter;
                pos--;
            }
            return sb.ToString();
        }

        private static SortedSet<char> GetSortedSet(string value)
        {
            SortedSet<char> sortedSet = new SortedSet<char>();
            foreach (var theCharacter in value.ToCharArray())
            {
                sortedSet.Add(theCharacter);
            }
            return sortedSet;
        }

        /// <summary>
        /// Encodes all the characters in the specified string into a sequence of bytes.
        /// </summary>
        /// <param name="value">The string </param>
        /// <returns></returns>
        public static byte[] GetBytes(this string value)
        {
            return Encoding.ASCII.GetBytes(value);
        }

    }
}
