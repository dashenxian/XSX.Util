﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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


        public static string EncryptMD5(this string input)
        {
            using MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(cryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", string.Empty).ToLower();
        }

        public static string EncryptShortMD5(this string input)
        {
            return input.EncryptMD5().Substring(8, 16);
        }

        public static bool ValidateMD5(this string input, string encryptedValue)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            return encryptedValue.Length == 16 ? input.EncryptShortMD5().Equals(encryptedValue) : input.EncryptMD5().Equals(encryptedValue);
        }
    }
}
