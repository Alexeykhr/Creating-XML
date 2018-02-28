using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Creating_XML.src
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
