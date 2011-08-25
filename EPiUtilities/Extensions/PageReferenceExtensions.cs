using EPiServer;
using EPiServer.Core;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="PageReference"/>.
    /// </summary>
    public static class PageReferenceExtensions
    {
        /// <summary>
        /// Checks whether the reference is null or empty. 
        /// Uses <see cref="PageReference"/>s own static method. 
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this PageReference reference)
        {
            return PageReference.IsNullOrEmpty(reference);
        }

        /// <summary>
        /// Returns the <see cref="PageData"/> this reference points to, or null if the reference is invalid. 
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static PageData ToPageData(this PageReference reference)
        {
            if (reference.IsNullOrEmpty())
                return null;

            return DataFactory.Instance.GetPage(reference);
        }

        /// <summary>
        /// Returns true if the reference can be resolved to a <see cref="PageData"/>. 
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static bool IsResolvable(this PageReference reference)
        {
            if (!reference.IsNullOrEmpty())
                return reference.ToPageData() != null;

            return false;
        }

        /// <summary>
        /// Returns the children of the referenced page. Returns an empty collection
        /// if the reference is not resolvable.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static PageDataCollection Children(this PageReference reference)
        {
            if (!reference.IsResolvable())
                return new PageDataCollection();

            return DataFactory.Instance.GetChildren(reference);
        }

        /// <summary>
        /// Returns the children of the referenced page filtered for visitor. Returns 
        /// an empty collection if the reference is not resolvable.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static PageDataCollection ChildrenForVisitor(this PageReference reference)
        {
            if (!reference.IsResolvable())
                return new PageDataCollection();

            return reference.Children().ForVisitor();
        }
    }
}
