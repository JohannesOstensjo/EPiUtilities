using System.Text;
using System.Web;
using System.Xml;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="HttpResponse"/> objects.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Returns the xml value of the XmlDocument as a xml response using UTF8.
        /// Ends the response after writing to the response stream. 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="document"></param>
        public static void ReturnXmlResponse(this HttpResponse response, XmlDocument document)
        {
            // See http://stackoverflow.com/questions/543319/how-to-return-xml-in-asp-net for details.

            response.Clear();
            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;
            document.Save(response.Output);     // This approach makes sure the specified ContentEncoding is used
            response.End();
        }

        /// <summary>
        /// Redirects the response with a 301 redirect. 
        /// This method has the same behavior as the HttpResponse method with the same
        /// name in .NET 4.0.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        public static void RedirectPermanent(this HttpResponse response, string url)
        {
            response.RedirectPermanent(url, true);
        }

        /// <summary>
        /// Redirects the response with a 301 redirect. 
        /// This method has the same behavior as the HttpResponse method with the same
        /// name in .NET 4.0.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <param name="endResponse"></param>
        public static void RedirectPermanent(this HttpResponse response, string url, bool endResponse)
        {
            response.Redirect(url, false);
            response.StatusCode = 301;
            if (endResponse)
                response.End();
        }
    }
}
