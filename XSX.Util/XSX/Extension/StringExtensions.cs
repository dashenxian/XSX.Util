using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using XSX.Extension.Collections;

namespace XSX.Extension
{
    public static class StringExtensions
    {
        /// <summary>
        /// 是否非空
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNotNullAndEmpty([NotNullWhen(true)][CanBeNull] this string val)
        {
            return !IsNullOrEmpty(val);
        }
        /// <summary>
        /// 字符串是否为空字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty([NotNullWhen(false)][CanBeNull] this string val)
        {
            return string.IsNullOrEmpty(val);
        }
        /// <summary>
        /// 是否为空或空白字符串
        /// </summary>
        public static bool IsNullOrWhiteSpace([NotNullWhen(false)][CanBeNull] this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        /// <summary>
        /// 确保字符串结束位置有指定的字符，如果没有就添加指定的字符到结束的位置，如果有则直接返回
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ss">开始位置指定的字符</param>
        /// <param name="comparisonType">比较方式</param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, char ss)
        {
            return EnsureEndsWith(str, ss + "", StringComparison.Ordinal);
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
        /// <summary>
        /// 转换路径分隔符与当前系统要求的路径分隔符匹配<see cref="Path.DirectorySeparatorChar"/>.
        /// </summary>
        public static string NormalizeDirectorySeparator(this string str)
        {
            return str.Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);
        }
        /// <summary>
        /// 转换路径分隔符'\'为url分隔符'/'
        /// </summary>
        public static string NormalizeUrlSeparator(this string str)
        {
            return str.Replace('\\', '/');
        }
        /// <summary>
        /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// 移除以指定的数组字符串结束的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="postFixes">查找并移除的字符串数组</param>
        /// <returns>移除后的字符串</returns>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            return str.RemovePostFix(StringComparison.Ordinal, postFixes);
        }
        /// <summary>
        /// 移除以指定的数组字符串结束的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="comparisonType">比较方式</param>
        /// <param name="postFixes">查找并移除的字符串数组</param>
        /// <returns>移除后的字符串</returns>
        public static string RemovePostFix(this string str, StringComparison comparisonType, params string[] postFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return null;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix, comparisonType))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// 移除以指定的数组字符串开始的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="postFixes">查找并移除的字符串数组</param>
        /// <returns>移除后的字符串</returns>
        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            return str.RemovePreFix(StringComparison.Ordinal, preFixes);
        }
        /// <summary>
        /// 移除以指定的数组字符串开始的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="comparisonType">比较方式</param>
        /// <param name="postFixes">查找并移除的字符串数组</param>
        /// <returns>移除后的字符串</returns>
        public static string RemovePreFix(this string str, StringComparison comparisonType, params string[] preFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return null;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix, comparisonType))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Left(this string str, int len)
        {
            Check.NotNull(str, nameof(str));

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }/// <summary>
         /// Gets a substring of a string from end of the string.
         /// </summary>
         /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
         /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Right(this string str, int len)
        {
            Check.NotNull(str, nameof(str));

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>camelCase of the string</returns>
        public static string ToCamelCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
            }

            return (useCurrentCulture ? char.ToLower(str[0]) : char.ToLowerInvariant(str[0])) + str.Substring(1);
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to sentence (by splitting words by space).
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        public static string ToSentenceCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return useCurrentCulture
                ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]))
                : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLowerInvariant(m.Value[1]));
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            Check.NotNull(value, nameof(value));
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            Check.NotNull(value, nameof(value));
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMd5(this string str)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(str);
            var hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes)
                .Replace("-", string.Empty);
            //var sb = new StringBuilder();
            //foreach (var hashByte in hashBytes)
            //{
            //    sb.Append(hashByte.ToString("X2"));
            //}

            //return sb.ToString();
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>PascalCase of the string</returns>
        public static string ToPascalCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToUpper() : str.ToUpperInvariant();
            }

            return (useCurrentCulture ? char.ToUpper(str[0]) : char.ToUpperInvariant(str[0])) + str.Substring(1);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptMd5(this string input)
        {
            using MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(cryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", string.Empty);
        }
        /// <summary>
        /// 取MD5的后16位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptShortMd5(this string input)
        {
            return input.EncryptMd5().Substring(8, 16);
        }
        /// <summary>
        /// 验证MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encryptedValue"></param>
        /// <returns></returns>
        public static bool ValidateMd5(this string input, string encryptedValue)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            return encryptedValue.Length == 16 ? input.EncryptShortMd5().Equals(encryptedValue) : input.EncryptMd5().Equals(encryptedValue);
        }

        /// <summary>
        /// 字符串掩码
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="mask">掩码符</param>
        /// <returns></returns>
        public static string Mask(this string s, char mask = '*')
        {
            if (string.IsNullOrWhiteSpace(s?.Trim()))
            {
                return s;
            }
            s = s.Trim();
            string masks = mask.ToString().PadLeft(4, mask);
            return s.Length switch
            {
                >= 11 => Regex.Replace(s, "(.{3}).*(.{4})", $"$1{masks}$2"),
                10 => Regex.Replace(s, "(.{3}).*(.{3})", $"$1{masks}$2"),
                9 => Regex.Replace(s, "(.{2}).*(.{3})", $"$1{masks}$2"),
                8 => Regex.Replace(s, "(.{2}).*(.{2})", $"$1{masks}$2"),
                7 => Regex.Replace(s, "(.{1}).*(.{2})", $"$1{masks}$2"),
                6 => Regex.Replace(s, "(.{1}).*(.{1})", $"$1{masks}$2"),
                _ => Regex.Replace(s, "(.{1}).*", $"$1{masks}")
            };
        }

        /// <summary>
        /// base64转字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] Base64ToByteArray(this string s)
        {
            return Convert.FromBase64String(s);
        }
        /// <summary>
        /// 字节数组转base64字符
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteArrayToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 替换不合法的文件名字符
        /// </summary>
        /// <param name="fileName">文件名，不含目录</param>
        /// <param name="replacement">替换的字符</param>
        /// <returns></returns>
        public static string MakeValidFileName(this string fileName, string replacement = "_")
        {
            var str = new StringBuilder();
            var invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (var c in fileName)
            {
                if (invalidFileNameChars.Contains(c))
                {
                    str.Append(replacement ?? "");
                }
                else
                {
                    str.Append(c);
                }
            }

            return str.ToString();
        }
        /// <summary>
        /// 把字符串分成两部分，前面的字符串和结尾的数字，一般用于计算下一个编号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultNum">如果字符串不以数字结尾时返回的默认值</param>
        /// <returns></returns>
        public static (string nonNumericPart, int numericValue) GetStrEndNumberAndStr(this string str, int defaultNum = 1)
        {
            if (string.IsNullOrEmpty(str))
            {
                return ("", defaultNum);
            }
            var maxNumericString = str;
            string numericPart = Regex.Match(maxNumericString, @"\d+$").Value; // 提取结尾的数字部分
            string nonNumericPart = maxNumericString.Substring(0, maxNumericString.Length - numericPart.Length); // 提取除数字外的字符部分
            if (!int.TryParse(numericPart, out var numericValue))   // 将数字部分转换为 int 类型
            {
                numericValue = 1;
            }
            return (nonNumericPart, numericValue);
        }
        /// <summary>
        /// byte数组转为hex字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// hex字符串转为byte数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string hexString)
        {
            int length = hexString.Length / 2;
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
    }
}
