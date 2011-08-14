using System.Web;
using EPiServer;
using EPiServer.Core;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see iref="IHttpHandler"/>.
    /// </summary>
    public static class IhttpHandlerExtensions
    {
        /// <summary>
        /// Tries to get the current EPiServer page from the HttpContext. 
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static PageData CurrentPage(this IHttpHandler handler)
        {
            if (handler is PageBase)
                return ((PageBase)handler).CurrentPage;

            return null;
        }
    }
}
