using System;
using System.Linq;

namespace XML.classes
{
    class Methods
    {
        public const string NAME = "XML";

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

        public static string FirstCharToUpper(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            return str.First().ToString().ToUpper() + str.Substring(1);
        }

        public static string GetPickPictures(string text)
        {
            string[] pictures = text.Trim().Split('\n');
            string outPictures = "";

            foreach (string picture in pictures)
            {
                if (!string.IsNullOrWhiteSpace(picture))
                    outPictures += picture.Trim() + Environment.NewLine;
            }

            return outPictures.Trim();
        }
    }
}
