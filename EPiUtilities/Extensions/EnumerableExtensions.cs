using System.Collections.Generic;
using EPiServer.Core;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see iref="IEnumerable"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts an enumerable of <see cref="PageData"/> to a <see cref="PageDataCollection"/>.
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static PageDataCollection ToPageDataCollection(this IEnumerable<PageData> enumerable)
        {
            return new PageDataCollection(enumerable);
        }

        /// <summary>
        /// Converts an enumerable of <see cref="PageReference"/> to a <see cref="PageDataCollection"/>.
        /// Any non resolvable references are dropped without errors. 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static PageDataCollection ToPageDataCollection(this IEnumerable<PageReference> enumerable)
        {
            return new PageReferenceCollection(enumerable).ToPageDataCollection();
        }
    }
}
