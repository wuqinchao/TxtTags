using Microsoft.VisualBasic;
using System;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace TxtTags
{
    public static class StringExtension
    {
        /// <summary>
        /// 获取指定长度的随机数字[0-9]字符串
        /// </summary>
        /// <param name="len">要求的长度</param>
        /// <returns></returns>
        public static string RandomNumber(int len)
        {
            Random rand = new System.Random(DateTime.Now.Millisecond);
            string output = "";
            while (output.Length < len)
            {
                output += rand.Next(0, 9).ToString();
                rand.NextDouble();
                rand.Next(100, 1999);
            }
            return output;
        }
        /// <summary>
        /// 获取指定长度的随机英文与数字字符串(区分大小写)
        /// </summary>
        /// <param name="len">要求的长度</param>
        /// <returns></returns>
        public static string Random(int len)
        {
            Random rand = new System.Random(DateTime.Now.Millisecond);
            string output = "";
            for (int j = 0; j < len; j++)
            {
                int i = rand.Next(1, 4);
                int ch;
                switch (i)
                {
                    case 1:
                        ch = rand.Next(0, 9);
                        output = output + ch.ToString();
                        break;
                    case 2:
                        ch = rand.Next(65, 90);
                        output = output + Convert.ToChar(ch).ToString();
                        break;
                    case 3:
                        ch = rand.Next(0, 9);
                        output = output + ch.ToString();
                        break;
                    case 4:
                        ch = rand.Next(97, 122);
                        output = output + Convert.ToChar(ch).ToString();
                        break;
                    default:
                        ch = rand.Next(97, 122);
                        output = output + Convert.ToChar(ch).ToString();
                        break;
                }
                rand.NextDouble();
                rand.Next(100, 1999);
            }
            return output;
        }
        /// <summary>
        /// 截取字符串左侧的指定长度
        /// </summary>
        /// <remarks>
        /// 如原字符串为空或长度小于指定长度,则直接返回原字符串
        /// </remarks>
        /// <param name="input">原字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>截取后的字符串</returns>
        public static string Left(this string input, int len)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= len)
            {
                return input;
            }
            else
            {
                return input.Substring(0, len);
            }
        }
        /// <summary>
        /// 截取字符串右侧的指定长度
        /// </summary>
        /// <remarks>
        /// 如原字符串为空或长度小于指定长度,则直接返回原字符串
        /// </remarks>
        /// <param name="input">原字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>截取后的字符串</returns>
        public static string Right(this string input, int len)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= len)
            {
                return input;
            }
            else
            {
                return input.Substring(input.Length - len, len);
            }
        }
        /// <summary>
        /// 将16进制字符串形式转为字节数组
        /// <remarks>
        /// 字符串长度需为2的整数倍,否则最后一个字符会被忽略
        /// 16进制不可加0x前缀
        /// </remarks>
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] HexStrToBytes(this string str)
        {
            byte[] b = new byte[str.Length / 2];
            for (int nIndex = 0; nIndex < b.Length; nIndex++)
            {
                string strTmp = str.Substring(nIndex * 2, 2);
                b[nIndex] = Convert.ToByte(strTmp, 16);
            }
            return b;
        }
        /// <summary>
        /// 将字节数组转为16进制字符串形式一个字节占两个字符,无0x前缀
        /// </summary>
        /// <param name="b">字节数组</param>
        /// <returns>字符串</returns>
        public static string BytesToHexStr(this byte[] b, int len = 0, int start = 0)
        {
            if (b == null || b.Length == 0 || start >= b.Length) return "";
            if (len == 0) len = b.Length - start;
            if (start + len > b.Length) return "";
            StringBuilder strBuilder = new StringBuilder();
            for (int nIndex = start; nIndex < start + len; nIndex++)
            {
                strBuilder.Append(b[nIndex].ToString("X2"));
            }
            return strBuilder.ToString();
        }
        public static string BytesToDebugHexStr(this byte[] b, int len = 0, int start = 0)
        {
            if (b == null || b.Length == 0 || start >= b.Length) return "";
            if (len == 0) len = b.Length - start;
            if (start + len > b.Length) return "";
            StringBuilder strBuilder = new StringBuilder();
            for (int nIndex = start; nIndex < start + len; nIndex++)
            {
                strBuilder.Append(b[nIndex].ToString("X2"));
                if (nIndex < start + len - 1)
                {
                    strBuilder.Append(" ");
                }
            }
            return strBuilder.ToString();
        }
        /// <summary>
        /// 检查字符串是否匹配正则表达示
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <param name="pattern">正则表达示</param>
        /// <returns>如果匹配返回true,否则返回false</returns>
        public static bool IsMatch(this string input, string pattern)
        {
            if (input == null)
                return false;
            return new System.Text.RegularExpressions.Regex(pattern).IsMatch(input);
        }
        public static bool IsValidIPv4(this string ip)
        {
            string pattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            return Regex.IsMatch(ip, pattern);
        }
        /// <summary>
        /// 字符串是否为null或string.Empty
        /// </summary>
        /// <param name="input">待检查的字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
        /// <summary>
        /// 将字符串转换为字节数组形式,原字符串的编码与操作系统当前相同
        /// </summary>
        /// <param name="input">待处理的字符串</param>
        /// <param name="target">目标字节数组的encoding</param>
        /// <returns>字节数组结果</returns>
        public static byte[] Encode(this string input, Encoding target)
        {
            if (input.IsNullOrEmpty()) return null;
            byte[] src = System.Text.Encoding.Default.GetBytes(input);
            return Encoding.Convert(Encoding.Default, target, src);
        }
        /// <summary>
        /// 将字符串转换为字节数组形式
        /// </summary>
        /// <param name="input">待处理的字符串</param>
        /// <param name="from">字符串的encoding</param>
        /// <param name="target">目标字节数组的encoding</param>
        /// <returns>字节数组结果</returns>
        public static byte[] Encode(this string input, Encoding from, Encoding target)
        {
            if (input.IsNullOrEmpty()) return null;
            byte[] src = from.GetBytes(input);
            return Encoding.Convert(from, target, src);
        }
        /// <summary>
        /// 将字节数组转换为字符串形式
        /// </summary>
        /// <remarks>
        /// 如果input为空,则返回string.Empty, 目标字符串的编码与当前操作系统相同
        /// </remarks>
        /// <param name="input">待处理的字节数组</param>
        /// <param name="from">字节数组的encoding</param>
        /// <returns>字符串结果</returns>
        public static string Decode(this byte[] input, Encoding from)
        {
            if (input == null)
                return string.Empty;
            byte[] dst = Encoding.Convert(from, Encoding.Default, input);
            return System.Text.Encoding.Default.GetString(dst);
        }
        /// <summary>
        /// 将字节数组转换为字符串形式
        /// </summary>
        /// <remarks>
        /// 如果input为空,则返回string.Empty
        /// </remarks>
        /// <param name="input">待处理的字节数组</param>
        /// <param name="from">字节数组的encoding</param>
        /// <param name="target">目标字符串的encoding</param>
        /// <returns>字符串结果</returns>
        public static string Decode(this byte[] input, Encoding from, Encoding target)
        {
            if (input == null)
                return string.Empty;
            byte[] dst = Encoding.Convert(from, target, input);
            return target.GetString(dst);
        }
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 判断值是否在指定范围之类
        /// </summary>
        /// <typeparam name="T">实现IComparable接口的类型</typeparam>
        /// <param name="t">等比较的数据</param>
        /// <param name="lowerBound">下限</param>
        /// <param name="upperBound">上限</param>
        /// <param name="includeLowerBound">是否包含下限,(.net2.0以上版本)默认为false</param>
        /// <param name="includeUpperBound">是否包含上限,(.net2.0以上版本)默认为false</param>
        /// <returns>如果在范围之类返回true,否则返回false</returns>
        public static bool IsBetween<T>(this T t, T lowerBound, T upperBound, bool includeLowerBound = false, bool includeUpperBound = false) where T : IComparable<T>
        {
            if (t == null) return false;

            var lowerCompareResult = t.CompareTo(lowerBound);
            var upperCompareResult = t.CompareTo(upperBound);

            return (includeLowerBound && lowerCompareResult == 0) ||
                (includeUpperBound && upperCompareResult == 0) ||
                (lowerCompareResult > 0 && upperCompareResult < 0);
        }


        /// <summary>
        /// 繁体到简体
        /// </summary>
        public static string ToSimplified(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }
            else
            {
                return Strings.StrConv(input, VbStrConv.SimplifiedChinese);
            }
        }
        /// <summary>
        /// 简体到繁体
        /// </summary>
        public static string ToTraditional(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }
            else
            {
                return Strings.StrConv(input, VbStrConv.TraditionalChinese);
            }
        }
        /// <summary>
        /// 替换（全角<字母/数字> → 半角）
        /// 正则表达式 + lambda表达式 + 字符计算
        /// </summary>
        /// <param name="input">带有全角的字母数字的字符串</param>
        /// <returns></returns>
        public static string ConvertSBCToDBC(this string input)
        {
            // 全角0-9，Unicode字符表示：\uff10-\uff19
            // 全角A-Z，Unicode字符表示：\uff21-\uff3a
            // 全角a-z，Unicode字符表示：\uff41-\uff5a
            // 全角与半角差值都为：65248
            return Regex.Replace(input, "[\uff10-\uff19\uff21-\uff3a\uff41-\uff5a]", new MatchEvaluator(p => ((char)(char.Parse(p.Value) - 65248)).ToString()));
        }
    }
}
