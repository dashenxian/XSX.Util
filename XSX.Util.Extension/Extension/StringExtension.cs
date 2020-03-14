using System;
using System.Collections.Generic;
using System.Text;

namespace XSX.Util.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 是否非空
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNotNullAndEmpty(this string val)
        {
            return !IsNullOrEmpty(val);
        }
        /// <summary>
        /// 字符串是否为空字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string val)
        {
            return string.IsNullOrEmpty(val);
        }
        /// <summary>
        /// 确保字符串结束位置有指定的字符，如果没有就添加指定的字符到结束的位置，如果有则直接返回
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ss">开始位置指定的字符</param>
        /// <param name="comparisonType">比较方式</param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, string ss)
        {
            return EnsureEndsWith(str, ss, StringComparison.Ordinal);
        }
        /// <summary>
        /// 确保字符串结束位置有指定的字符，如果没有就添加指定的字符到结束的位置，如果有则直接返回
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ss">开始位置指定的字符</param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, string ss, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.EndsWith(ss, comparisonType))
            {
                return str;
            }

            return str + ss;
        }
        /// <summary>
        /// 确保字符串开始位置有指定的字符，如果没有就添加指定的字符到开始的位置，如果有则直接返回
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ss">开始位置指定的字符</param>
        /// <returns></returns>
        public static string EnsureStartsWith(this string str, string ss)
        {
            return EnsureStartsWith(str, ss, StringComparison.Ordinal);
        }
        /// <summary>
        /// 确保字符串开始位置有指定的字符，如果没有就添加指定的字符到开始的位置，如果有则直接返回
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ss">开始位置指定的字符</param>
        /// <param name="comparisonType">比较方式</param>
        /// <returns></returns>
        public static string EnsureStartsWith(this string str, string ss, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.StartsWith(ss, comparisonType))
            {
                return str;
            }

            return ss + str;
        }

    }
}
