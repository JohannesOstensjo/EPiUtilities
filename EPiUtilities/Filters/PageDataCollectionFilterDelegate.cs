using EPiServer.Core;

namespace EPiUtilities.Filters
{
    /// <summary>
    /// Delegate for EPiServer's standard filter method signature.
    /// </summary>
    /// <param name="pages"></param>
    public delegate void PageDataCollectionFilterDelegate(PageDataCollection pages);
}
