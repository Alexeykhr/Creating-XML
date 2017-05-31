using System;

namespace XML.classes
{
    class Methods
    {
        public static string ReplaceComma(string text)
        {
            return text.Replace(",", ".");
        }

        public static string ReplaceDot(string text)
        {
            return text.Replace(".", ",");
        }

        public static bool IsWebSite(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }
}
