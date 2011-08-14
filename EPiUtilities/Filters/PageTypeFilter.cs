using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Filters;

namespace EPiUtilities.Filters
{
    /// <summary>
    /// A filter class that can filter <see cref="PageDataCollection"/>s to only contain 
    /// pages of certain type(s).
    /// </summary>
    public class PageTypeFilter : IPageFilter
    {
        private readonly List<int> _pageTypeIds;

        /// <summary>
        /// Creates a new filter that will filter PageDataCollections to only contain 
        /// pages of the specified type.
        /// </summary>
        /// <param name="pageTypeId"></param>
        public PageTypeFilter(int pageTypeId)
        {
            _pageTypeIds = new List<int> { pageTypeId };
        }

        /// <summary>
        /// Creates a new filter that will filter PageDataCollections to only contain 
        /// pages of the specified type(s).
        /// </summary>
        /// <param name="pageTypeIds"></param>
        public PageTypeFilter(IEnumerable<int> pageTypeIds)
        {
            _pageTypeIds = new List<int>(pageTypeIds);
        }

        /// <summary>
        /// Event handler that calls the filter in this filter class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Filter(object sender, FilterEventArgs e)
        {
            Filter(e.Pages);
        }

        /// <summary>
        /// Filters the specified collection, removing pages of types not specified in
        /// the constructor. 
        /// </summary>
        /// <param name="pages"></param>
        public void Filter(PageDataCollection pages)
        {
            for (int i = pages.Count - 1; i > -1; i--)
            {
                if (ShouldFilter(pages[i]))
                    pages.RemoveAt(i);
            }
        }

        /// <summary>
        /// Returns true if the filter will remove this page when filtering a collection.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public bool ShouldFilter(PageData page)
        {
            return !_pageTypeIds.Contains(page.PageTypeID);
        }
    }
}
