using System.Text.RegularExpressions;

namespace Creating_XML.core
{
    class RegexHelper
    {
        public static bool IsOnlyNumbers(string text)
        {
            return new Regex("[^0-9]+").IsMatch(text);
        }

        /// <summary>
        /// Remove double spaces + Trim.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveExtraSpace(string text)
        {
            return Regex.Replace(text.Trim(), @"\s+", " ");
        }
    }
}
