using System.Web.UI;
using EPiServer;
using EPiServer.Core;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Page"/>.
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// Attempts to get the current EPiServer page from the Page
        /// object. Returns null if current page is not an EPiServer page.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static PageData ToCurrentPage(this Page page)
        {
            if (page is PageBase)
                return ((PageBase)page).CurrentPage;

            return null;
        }
    }
}
