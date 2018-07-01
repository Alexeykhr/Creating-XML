using System;

namespace Creating_XML.core
{
    class Web
    {
        /// <summary>
        /// Checking for the correct URL address.
        /// </summary>
        /// <returns></returns>
        public static bool IsCorrectURL(string uriName)
        {
            return Uri.TryCreate(uriName, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
