using EPiServer.Core;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="PageReferenceCollection"/>.
    /// </summary>
    public static class PageReferenceCollectionExtensions
    {
        /// <summary>
        /// Returns the PageDataCollection which the PageReferenceCollection represents.
        /// Any non resolvable references will be skipped without errors.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static PageDataCollection ToPageDataCollection(this PageReferenceCollection references)
        {
            var retval = new PageDataCollection();

            if (references != null)
                foreach (PageReference reference in references)
                    retval.AddIfResolvable(reference);

            return retval;
        }
    }
}
