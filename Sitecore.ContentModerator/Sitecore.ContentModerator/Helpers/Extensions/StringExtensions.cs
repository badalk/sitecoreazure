using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace Sitecore.ContentModerator.Helpers.Extensions
{
    public static class StringExtensions
    {
        static readonly Regex AcceptedCharacters = new Regex(@"[^a-zA-Z0-9_]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly char[] StringSplitCharacters = new[] { ',', ';', ':', '|' };
        private static Regex emailRegex = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsEmpty(this string str)
        {
            return str.IsEmptyOrNull();
        }
        public static bool IsEmptyOrNull(this string str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrEmpty(str.Trim());
        }

        public static bool IsNotEmpty(this string str)
        {
            return str.IsNotEmptyAndNotNull();
        }
        public static bool IsNotEmptyAndNotNull(this string str)
        {
            return (!String.IsNullOrEmpty(str) && !String.IsNullOrEmpty(str.Trim()));
        }

        public static T ToObject<T>(this string obj)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(obj);
        }

        public static string RemoveNonXmlCharacters(this string str)
        {
            return AcceptedCharacters.Replace(str, string.Empty);
        }
        public static double ToDouble(this string str)
        {
            double num = 0;
            if (str.IsNotEmpty())
            {
                if (double.TryParse(str, out num))
                    return num;
            }
            return num;
        }
        public static DateTime ToDateTime(this string str)
        {
            var dateTime = DateTime.MinValue;

            if (str.IsNotEmpty() && DateTime.TryParse(str, out dateTime))
            {
                return dateTime;
            }

            return dateTime;
        }
        public static int ToInt(this string str)
        {
            var num = 0;
            if (str.IsNotEmpty())
            {
                if (int.TryParse(str, out num))
                    return num;
            }
            return num;
        }

        public static long ToLong(this string str)
        {
            long num = 0;
            if (str.IsNotEmpty())
            {
                if (long.TryParse(str, out num))
                    return num;
            }
            return num;
        }

        public static string[] SplitUp(this string str, bool keepEmpty)
        {
            return str.SplitUp(StringSplitCharacters, keepEmpty);
        }

        public static string[] SplitUp(this string str, char[] splitChar)
        {
            return str.SplitUp(splitChar, false);
        }

        public static string[] SplitUp(this string str, char[] splitChar, bool keepEmpty)
        {
            return keepEmpty ? str.Split(splitChar) : str.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
        }

        static readonly IDictionary<string, bool> BoolMapping = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
				{
					{"yes",true},
					{"no",false},
					{"1",true},
					{"0",false},
					{"true",true},
					{"false",false},
					{"yup",true},
					{"nope",false},
					{"y",true},
					{"n",false},
					{"t",true},
					{"f",false}
				};

        public static bool ToBool(this string str)
        {
            bool val;

            if (str.IsNotEmpty() && bool.TryParse(str.Trim(), out val))
                return val;

            return false;
        }

        public static bool ToBoolean(this string str)
        {
            bool val;

            if (str.IsNotEmpty() && bool.TryParse(str.Trim(), out val))
                return val;

            if (str.IsEmpty())
                return false;

            return BoolMapping.TryGetValue(str.Trim(), out val) ? val : bool.Parse(str);
        }

        public static bool IsSameAs(this string str1, string str2)
        {
            if (str1.IsEmpty() || str2.IsEmpty())
                return false;

            return String.Compare(str1.Trim(), str2.Trim(), StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public static bool Has(this string str1, string str2)
        {
            return str1.IndexOf(str2, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        public static string StripHtml(this string html)
        {
            if (String.IsNullOrEmpty(html)) return "";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            return HttpUtility.HtmlDecode(doc.DocumentNode.InnerText);
        }
    }
}