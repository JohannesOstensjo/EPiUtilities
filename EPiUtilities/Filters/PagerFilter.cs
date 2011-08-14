using System;
using EPiServer.Core;
using EPiServer.Filters;

namespace EPiUtilities.Filters
{
    /// <summary>
    /// A filter which filters the collection to only contain the pages
    /// in a given page given a page size and page number.
    /// </summary>
    public class PagerFilter : IPageFilter 
    {
        private readonly int _pageSize;
        private readonly int _pageNumber;

        /// <summary>
        /// Creates a filter which filters a PageDataCollection for a pager
        /// given the size and number of the page. 
        /// pageNumber must be 1 or larger, it is not zero based. 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        public PagerFilter(int pageSize, int pageNumber)
        {
            if (_pageSize < 1)
                throw new ArgumentException("pageSize must be larger than zero.", "pageSize");

            if (_pageNumber < 1)
                throw new ArgumentException("pageNumber must be larger than zero.", "pageNumber");

            _pageSize = pageSize;
            _pageNumber = pageNumber;
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
        /// Filters the collection, leaving only the pages which 
        /// correspond to the given page number and size.
        /// </summary>
        /// <param name="pages"></param>
        public void Filter(PageDataCollection pages)
        {
            for (int i = pages.Count - 1; i > -1; i--)
            {
                if (i >= _pageNumber * _pageSize)
                    pages.RemoveAt(i);

                if (i < (_pageNumber - 1) * _pageSize)
                    pages.RemoveAt(i);
            }
        }

        /// <summary>
        /// Not implemented, as this filter only applies to collection contexts.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public bool ShouldFilter(PageData page)
        {
            throw new NotImplementedException();
        }
    }
}
