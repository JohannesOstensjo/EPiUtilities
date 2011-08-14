using EPiServer.Core;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see ref="int"/>.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Returns a <see cref="PageReference"/> with the int as id. 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static PageReference ToPageReference(this int i)
        {
            return new PageReference(i);
        }
    }
}
