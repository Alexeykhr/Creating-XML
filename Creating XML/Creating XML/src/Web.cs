using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creating_XML.src
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
