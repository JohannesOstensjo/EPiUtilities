using System;
using EPiServer.Core;
using EPiServer.Filters;
using EPiUtilities.Extensions;

namespace EPiUtilities.Filters
{
    /// <summary>
    /// A filter which filters pages on whether a certain property is in a given date interval. 
    /// If the property has no value the page will be removed as well. 
    /// If you need a "later than" or "earlier than" filter just use DateTime.MaxValue or MinValue
    /// as one of the boundaries. 
    /// </summary>
    public class DateIntervalFilter : IPageFilter
    {
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly string _datePropertyName;

        /// <summary>
        /// Creates a filter that will filter the specified property with the specified date range. 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="datePropertyName"></param>
        public DateIntervalFilter(DateTime fromDate, DateTime toDate, string datePropertyName)
        {
            _fromDate = fromDate;
            _toDate = toDate;
            _datePropertyName = datePropertyName;
        }

        /// <summary>
        /// Returns an instance of <see cref="DateIntervalFilter"/> that will filter on the 
        /// start publish property.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DateIntervalFilter StartPublishFilter(DateTime fromDate, DateTime toDate)
        {
            return new DateIntervalFilter(fromDate, toDate, "PageStartPublish");
        }

        /// <summary>
        /// Returns an instance of <see cref="DateIntervalFilter"/> that will filter on the 
        /// stop publish property.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DateIntervalFilter StopPublishFilter(DateTime fromDate, DateTime toDate)
        {
            return new DateIntervalFilter(fromDate, toDate, "PageStopPublish");
        }

        /// <summary>
        /// Returns an instance of <see cref="DateIntervalFilter"/> that will filter on the 
        /// page created property.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DateIntervalFilter PageCreatedFilter(DateTime fromDate, DateTime toDate)
        {
            return new DateIntervalFilter(fromDate, toDate, "PageCreated");
        }

        /// <summary>
        /// Returns an instance of <see cref="DateIntervalFilter"/> that will filter on the 
        /// page changed property.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DateIntervalFilter PageChangedFilter(DateTime fromDate, DateTime toDate)
        {
            return new DateIntervalFilter(fromDate, toDate, "PageChanged");
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
        /// Filters the collection, removing pages that do not have a value in the correct range
        /// for the specified property. 
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
            if (page.PropertyHasValue(_datePropertyName))
                if (page.Property[_datePropertyName].Value is DateTime)
                {
                    var date = page.PropertyValueOrDefault<DateTime>(_datePropertyName);
                    if (date >= _fromDate && date <= _toDate)
                        return false;
                }

            return true;
        }
    }
}
