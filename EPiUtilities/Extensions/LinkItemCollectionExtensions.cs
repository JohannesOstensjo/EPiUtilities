using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="LinkItemCollection"/>.
    /// </summary>
    public static class LinkItemCollectionExtensions
    {
        /// <summary>
        /// Returns a <see cref="PageDataCollection"/> with the links that resolve to EPiServer pages.
        /// Other links are dropped without error.
        /// </summary>
        /// <param name="linkItemCollection"></param>
        /// <returns></returns>
        public static PageDataCollection ToPageDataCollection(this LinkItemCollection linkItemCollection)
        {
            var retval = new PageDataCollection();

            if (linkItemCollection != null)
                foreach (var linkItem in linkItemCollection)
                {
                    var url = new UrlBuilder(linkItem.Href);

                    if (PermanentLinkMapStore.ToMapped(url))
                        retval.AddIfResolvable(PermanentLinkUtility.GetPageReference(url));
                }

            return retval;
        }
    }
}
