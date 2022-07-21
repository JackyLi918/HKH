using System;
using System.Text;
using System.Text.RegularExpressions;
using HKH.Common;

namespace System//HKH.Common
{
    /// <summary>
    /// Summary description for StringExtension.
    /// </summary>
    public static class StringExtension
    {
        #region Methods

        public static bool EqualsIgnoreCase(this string str1, string str2)
        {
            return str1.Equals(str2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// reverse string
        /// </summary>
        /// <param name="srcStr"></param>
        /// <returns></returns>
        public static string Reverse(this string srcStr)
        {
            char[] cArray = srcStr.ToCharArray();
            Array.Reverse(cArray);
            return new string(cArray);
        }

        /// <summary>
        /// Get count of char in a string
        /// </summary>
        /// <param name="src"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int IncludeCount(this string src, char c)
        {
            return Regex.Matches(src, c.ToString()).Count;
        }

        /// <summary>
        /// Swap char
        /// </summary>
        /// <param name="srcStr"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static string SwapChar(this string srcStr, int p1, int p2)
        {
            if ((p1 == p2) || ((p1 < 0) || (p2 < 0)))
                return srcStr;

            if ((p1 >= srcStr.Length) || (p2 >= srcStr.Length))
                return srcStr;

            char[] vChars = srcStr.ToCharArray();
            vChars[p1] = (char)(vChars[p2] | (vChars[p2] = vChars[p1]) & 0);
            return new string(vChars);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcStr"></param>
        /// <returns></returns>
        public static string RemoveAllWhiteSpaces(this string srcStr)
        {
            return srcStr.Replace(Constants.WhiteSpace, string.Empty);
        }

        /// <summary>
        /// remove all Html tags
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string RemoveHtml(this string strHtml)
        {
            return Regex.Replace(strHtml, "<[^>]*>|</[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// remove special Html tags
        /// </summary>
        /// <param name="strHtml"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string RemoveHtml(this string strHtml, string tag)
        {
            return Regex.Replace(strHtml, "<" + tag + "[^>]*>|</[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// substring Html visible chars, keep Html tags and style
        /// </summary>
        /// <param name="strHtml"></param>
        /// <param name="count"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string HtmlSubString(this string strHtml, int count, string endStr)
        {
            if (count < 1)
                return string.Empty;

            if (strHtml.Length <= count || strHtml.RemoveHtml().Length <= count)
                return strHtml;

            StringBuilder result = new StringBuilder();
            int n = 0;
            bool isHtmlTag = false;
            bool isEscapeCode = false;
            foreach (char c in strHtml.ToCharArray())
            {
                if (c == '<')
                    isHtmlTag = true;
                else if (c == '&')
                    isEscapeCode = true;
                else if (c == ';' && isEscapeCode)
                    isEscapeCode = false;
                else if (c == '>' && isHtmlTag)
                {
                    n--;    //set isHtmlTag false will cause n++ in next line. here only adjust it.
                    isHtmlTag = false;
                }

                if (!(isHtmlTag || isEscapeCode))
                    n++;

                result.Append(c);

                if (n >= count)
                    break;
            }

            //get all Html tags
            string tempResult = Regex.Replace(result.ToString(), @"(>)[^<>]*(<?)", "$1$2");

            //remove the tags that have closed with <tag />.
            tempResult = Regex.Replace(tempResult, @"</?(area|base|basefont|body|br|col|colgroup|dd|dt|frame|head|hr|html|img|input|isindex|li|link|meta|option|p|param|tbody|td|tfoot|th|thead|tr)[^<>]*/?>", "", RegexOptions.IgnoreCase);

            //remove the tags that have closed with <tag>...</tag>
            tempResult = Regex.Replace(tempResult, @"<([a-zA-Z]+)[^<>]*>(.*?)</\1>", "$2", RegexOptions.IgnoreCase);

            //get all tags that is unclosed and close them.
            MatchCollection matchs = Regex.Matches(tempResult, @"<([a-zA-Z]+)[^<>]*>");
            for (int i = matchs.Count - 1; i > -1; i--)
            {
                result.Append("</");
                result.Append(matchs[i].Result("$1"));
                result.Append(">");
            }

            result.Append(endStr);

            return result.ToString();
        }

        /// <summary>
        /// get a html tag and its content
        /// </summary>
        /// <param name="strHtml"></param>
        /// <param name="tag"></param>
        /// <param name="attr"></param>
        /// <param name="closed"></param>
        /// <returns></returns>
        public static string GetHtmlTag(this string strHtml, string tag, string attr = "", bool closed = true)
        {
            Regex reg = null;
            if (closed)
                reg = new Regex(string.Format(@"(?is)\s*<{0}{1}[^>]*?>(((?:(?<Open>)<{0}[^>]*?>|(?<-Open>)</{0}>|(?:(?!</?{0}).)*)*)(?(Open)(?!)))</{0}>\s*", tag, attr), RegexOptions.IgnoreCase);
            else
                reg = new Regex(string.Format(@"(?is)\s*<{0}{1}[^>]*?>\s*", tag, attr), RegexOptions.IgnoreCase);
            Match m = reg.Match(strHtml);

            return m.Success ? m.Value : string.Empty;
        }

        /// <summary>
        /// get an attribute value from a html tag
        /// </summary>
        /// <param name="strHtml"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string GetHtmlAttr(this string strHtml, string attr)
        {
            Regex reg = new Regex(string.Format("{0}[ ]*=[ ]*\"([\\S| ]*)\"", attr));
            Match m = reg.Match(strHtml);

            return m.Success ? m.Groups[1].Value : string.Empty;
        }

        /// <summary>
        /// only used by both chinese and english
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="count"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string GetFixedWidthString(this string strText, int count, string endStr)
        {
            if (strText.Length <= count)
            {
                return strText;
            }
            else
            {
                int realCount = count - (string.IsNullOrEmpty(endStr) ? 0 : endStr.Length);

                int index = 0;
                double iCount = 0;
                //chinese
                Regex regex = new Regex("^[\u4e00-\u9fa5]{0,}$");

                foreach (char ch in strText)
                {
                    if (iCount <= realCount)
                        index++;

                    //looks as 0.75 chinese word if upper case
                    if (ch >= 65 && ch <= 90)
                    {
                        iCount += 0.75;
                    }
                    //looks as 0.5 chinese word if lower case
                    else if ((ch >= 0 && ch < 65) || (ch > 90 && ch <= 255))
                    {
                        iCount += 0.5;
                    }
                    //looks as 1 if chinese
                    else if (regex.IsMatch(ch.ToString()))
                    {
                        iCount += 1;
                    }
                    if (iCount >= count)
                    {
                        break;
                    }
                }


                if (iCount >= count)
                {
                    return strText.Substring(0, index - 1) + endStr;
                }
                else
                {
                    return strText;
                }
            }
        }

        public static string ToBinString(this string input)
        {
            var ba = Encoding.Unicode.GetBytes(input);
            return ToHexString(ba, null);
        }

        public static byte[] ToByteArray(this string input)
        {
            input = input.Trim();
            if (input.Length > 2 && input.ToLower().StartsWith("0x"))
                input = input.Substring(2, input.Length - 2);

            int count = input.Length;
            byte[] ba = new byte[count / 2];
            for (int i = 0; i < count; i += 2)
                ba[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);

            return ba;
        }

        public static string ToHexString(this byte[] input, string prefix = "0x")
        {
            StringBuilder output = new StringBuilder(BitConverter.ToString(input));
            output.Replace("-", "");

            if (!string.IsNullOrWhiteSpace(prefix))
                output.Insert(0, prefix);

            return output.ToString();
        }

        #endregion

        #region Format validate

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string str)
        {
            return Regex.IsMatch(str, @"^[+-]?\d+(\.\d+)?$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(this string email)
        {
            return Regex.IsMatch(email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        /// <remarks>
        /// (0086)(0311)88888888-6666
        /// 0086-0311-88888888-6666
        /// </remarks>
        public static bool IsPhone(this string phone)
        {
            return Regex.IsMatch(phone, @"^((\(00\d{1,3}\))|(00\d{1,3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// <remarks>
        /// (0086)13832103210
        /// 0086-13832103210
        /// </remarks>
        public static bool IsMobile(this string mobile)
        {
            return Regex.IsMatch(mobile, @"^((\(00\d{1,3}\))|(00\d{1,3}\-))?1\d{10}$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsEnglishWord(this string word)
        {
            return Regex.IsMatch(word, @"^[A-Za-z]+$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsChineseWord(this string word)
        {
            return Regex.IsMatch(word, @"^[\u0391-\uFFE5]+$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(this string url)
        {
            return Regex.IsMatch(url, @"^(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
        }

        /// <summary>
        /// only for chinese
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool IsIdCard(this string idCard)
        {
            return Regex.IsMatch(idCard, @"^\d{15}(\d{2}[Xx|\d])?$");
        }

        #endregion

        #region Safe Convert

        public static int SafeToInt(this string src)
        {
            return SafeToInt(src, 0);
        }

        public static int SafeToInt(this string src, int defaultValue)
        {
            int result = 0;
            return int.TryParse(src, out result) ? result : defaultValue;
        }

        public static double SafeToDouble(this string src)
        {
            return SafeToDouble(src, 0.0);
        }

        public static double SafeToDouble(this string src, double defaultValue)
        {
            double result = 0.0;
            return double.TryParse(src, out result) ? result : defaultValue;
        }

        public static DateTime SafeToDateTime(this string src)
        {
            return SafeToDateTime(src, DateTime.MinValue);
        }

        public static DateTime SafeToDateTime(this string src, DateTime defaultValue)
        {
            DateTime result = defaultValue;
            return DateTime.TryParse(src, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Parse Json data string to C# DateTime
        /// </summary>
        /// <param name="jsonDate"></param>
        /// <returns></returns>
        public static DateTime JsonToDateTime(this string jsonDate)
        {
            return JsonToDateTime(jsonDate, DateTimeKind.Local);
        }

        /// <summary>
        /// Parse Json data string to C# DateTime
        /// </summary>
        /// <param name="jsonDate"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static DateTime JsonToDateTime(this string jsonDate, DateTimeKind kind)
        {
            Regex regex = new Regex(@"^/Date\(([0-9]+)(\+[0-9]+)?\)/$");

            if (!regex.IsMatch(jsonDate))
                throw new FormatException("Bad format of Json date.");

            long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
            DateTime utcDateTime = new DateTime(long.Parse(regex.Replace(jsonDate, "$1")) * 10000 + InitialJavaScriptDateTicks, DateTimeKind.Utc);

            DateTime dateTime;
            switch (kind)
            {
                case DateTimeKind.Unspecified:
                    dateTime = DateTime.SpecifyKind(utcDateTime.ToLocalTime(), DateTimeKind.Unspecified);
                    break;
                case DateTimeKind.Local:
                    dateTime = utcDateTime.ToLocalTime();
                    break;
                default:
                    dateTime = utcDateTime;
                    break;
            }

            return dateTime;
        }

        public static bool SafeToBool(this string src)
        {
            bool result = false;
            return bool.TryParse(src, out result) ? result : false;
        }

        #endregion
    }
}

/*
 *      //F(n)=(1/��5)*{[(1+��5)/2]^n - [(1-��5)/2]^n}
        private int GetFibonacci(int i)
        {
            double y = Math.Sqrt(5);
            double a = 1 / y;
            double b = (1 + y) / 2;
            double c = (1 - y) / 2;

            return Convert.ToInt32(a * (Math.Pow(b, i) - Math.Pow(c, i)));
        }
 */
