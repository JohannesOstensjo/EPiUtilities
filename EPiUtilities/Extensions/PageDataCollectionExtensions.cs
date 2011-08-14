using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Security;
using EPiUtilities.Filters;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="PageDataCollection"/>.
    /// </summary>
    public static class PageDataCollectionExtensions
    {
        /// <summary>
        /// Returns the the collection as a <see cref="PageReferenceCollection"/>.
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static PageReferenceCollection ToPageReferenceCollection(this PageDataCollection pages)
        {
            var retval = new PageReferenceCollection();

            if (pages != null)
            {
                retval.AddRange(pages.Select(page => page.PageLink));
            }

            return retval;
        }

        /// <summary>
        /// Filters the PageDataCollection to only contain pages of the specified type. 
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="pageTypeId"></param>
        /// <returns></returns>
        public static PageDataCollection OfPageType(this PageDataCollection pages, int pageTypeId)
        {
            if (pages != null)
                (new PageTypeFilter(pageTypeId)).Filter(pages);

            return pages;
        }

        /// <summary>
        /// Filters the PageDataCollection to only contain pages of the specified types. 
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="pageTypeIds"></param>
        /// <returns></returns>
        public static PageDataCollection OfPageTypes(this PageDataCollection pages, IEnumerable<int> pageTypeIds)
        {
            if (pages != null)
                (new PageTypeFilter(pageTypeIds)).Filter(pages);

            return pages;
        }

        ///<summary>
        /// Filters the PageDataCollection with the FilterForVisitor filter.
        ///</summary>
        ///<param name="pages"></param>
        ///<returns></returns>
        public static PageDataCollection ForVisitor(this PageDataCollection pages)
        {
            if (pages != null)
                FilterForVisitor.Filter(pages);

            return pages;
        }

        /// <summary>
        /// Filters the collection so it only contains pages visible in menu.
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static PageDataCollection VisibleInMenu(this PageDataCollection pages)
        {
            new FilterCompareTo("PageVisibleInMenu", "true").Filter(pages);
            return pages;
        }

        /// <summary>
        /// Filters the collection so it only contains pages not visible in menu.
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static PageDataCollection NotVisibleInMenu(this PageDataCollection pages)
        {
            new FilterCompareTo("PageVisibleInMenu", "false").Filter(pages);
            return pages;
        }

        /// <summary>
        /// Uses FilterAccess to filter for the specified access.
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="requiredAccess"></param>
        /// <returns></returns>
        public static PageDataCollection FilterAccess(this PageDataCollection pages, AccessLevel requiredAccess)
        {
            new FilterAccess(requiredAccess).Filter(pages);
            return pages;
        }

        /// <summary>
        /// Uses FilterCompareTo to filter the collection.
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static PageDataCollection FilterCompareTo(this PageDataCollection pages, string propertyName, string propertyValue)
        {
            new FilterCompareTo(propertyName, propertyValue).Filter(pages);
            return pages;
        }

        /// <summary>
        /// Sorts the collection with FilterPropertySort.
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public static PageDataCollection PropertySort(this PageDataCollection pages, string sortBy, FilterSortDirection sortDirection)
        {
            new FilterPropertySort(sortBy, sortDirection).Filter(pages);
            return pages;
        }

        /// <summary>
        /// Sorts the collection using FilterSort.
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static PageDataCollection Sort(this PageDataCollection pages, FilterSortOrder sortOrder)
        {
            new FilterSort(sortOrder).Filter(pages);
            return pages;
        }

        /// <summary>
        /// Applies the filter delegate to the collection.
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="filterDelegate"></param>
        /// <returns></returns>
        public static PageDataCollection Filter(this PageDataCollection pages, PageDataCollectionFilterDelegate filterDelegate)
        {
            if (pages != null)
            {
                filterDelegate(pages);
            }

            return pages;
        }

        /// <summary>
        /// Applies the <see cref="PagerFilter"/> on the collection.
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static PageDataCollection FilterForPager(this PageDataCollection pages, int pageSize, int pageNumber)
        {
            new PagerFilter(pageSize, pageNumber).Filter(pages);

            return pages;
        }

        /// <summary>
        /// Adds the referenced page to the collection if it is resolvable. 
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="reference"></param>
        public static void AddIfResolvable(this PageDataCollection pages, PageReference reference)
        {
            if (reference.IsResolvable())
                pages.Add(reference.ToPageData());
        }
    }
}
