using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Web.WebControls;
using EPiUtilities.Extensions;
using EPiUtilities.WebControls.BaseClasses;

namespace EPiUtilities.WebControls
{
    /// <summary>
    /// A simple templated PageData list. Fill it by setting the 
    /// Pages property to a PageDataCollection or set the ListRoot
    /// to have the list fill itself with the root's children.
    /// </summary>
    [ParseChildren(true)]
    public class PageDataList : TemplatedPageDataItemListControlBase
    {
        /// <summary>
        /// Holds the total number of items before any paging. 
        /// </summary>
        protected int TotalItemCount;

        /// <summary>
        /// If set, the reference will be used as a root for the list and
        /// it will be populated with the root's first level children 
        /// filtered with FilterForVisitor. 
        /// </summary>
        public PageReference ListRoot { get; set; }

        /// <summary>
        /// Can be used to get or set the pages in the list.
        /// Will be ignored if ListRoot is set. 
        /// </summary>
        public PageDataCollection Pages
        {
            get
            {
                if (DataSource is PageDataCollection)
                    return (PageDataCollection)DataSource;

                return null;
            }
            set { DataSource = value; }
        }

        /// <summary>
        /// Filters added to this event will be run on the items before 
        /// filling the list. These filters run before filters invoked by 
        /// properties on this list. 
        /// </summary>
        public event FilterEventHandler Filter;

        private FilterSortOrder _sortOrder = FilterSortOrder.None;

        /// <summary>
        /// Sort order used for this list. Default is None. 
        /// Will be applied after the Filter event.
        /// </summary>
        public FilterSortOrder SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        /// <summary>
        /// Sort by a property instead of setting SortOrder.
        /// Used together with SortDirection.
        /// </summary>
        public string SortBy;

        /// <summary>
        /// Limits the total number of items in the list.
        /// Is applied after sorting.
        /// A value larger than zero is considered a value, 
        /// zero or less has no effect.
        /// </summary>
        public int MaxCount;

        /// <summary>
        /// The sortdirection when using SortBy.
        /// </summary>
        public FilterSortDirection SortDirection;

        /// <summary>
        /// Override which creates and adds the content of the control. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var items = GetItems();

            if (items.Count > 0)
            {
                AddHeaderPager(TotalItemCount);

                AddHeaderTemplate();

                for (int i = 0; i < items.Count; i++)
                {
                    if (i == 0 && FirstItemTemplate != null)
                        AddItemTemplate(FirstItemTemplate, items[i], i + 1);

                    if (i == 1 && SecondItemTemplate != null)
                        AddItemTemplate(SecondItemTemplate, items[i], i + 1);

                    if (i == 2 && ThirdItemTemplate != null)
                        AddItemTemplate(ThirdItemTemplate, items[i], i + 1);

                    if (i == 3 && FourthItemTemplate != null)
                        AddItemTemplate(FourthItemTemplate, items[i], i + 1);

                    if (!Added && i % 2 == 1 && AlternatingItemTemplate != null)
                        AddItemTemplate(AlternatingItemTemplate, items[i], i + 1);

                    if (!Added && ItemTemplate != null)
                        AddItemTemplate(ItemTemplate, items[i], i + 1);

                    if (Added)
                    {
                        Added = false;
                        AddSeparator = true;
                    }
                }

                AddFooterTemplate();

                AddFooterPager(TotalItemCount);
            }
            else
                HideOrEmpty();
        }

        /// <summary>
        /// Gets the items to display in the control, applying sorting 
        /// and filtering as required. 
        /// </summary>
        /// <returns></returns>
        protected PageDataCollection GetItems()
        {
            var items = new PageDataCollection();

            if (DataSource != null)
                if (DataSource is IEnumerable<PageData>)
                    items = new PageDataCollection((IEnumerable<PageData>)DataSource);

            if (!ListRoot.IsNullOrEmpty())
                items = ListRoot.ChildrenForVisitor();

            if (Filter != null)
                Filter(this, new FilterEventArgs(items));

            if (SortOrder != FilterSortOrder.None)
                items.Sort(SortOrder);

            if (!SortBy.NullOrEmpty())
                new FilterPropertySort(SortBy, SortDirection).Filter(items);

            if (MaxCount > 0)
                new FilterCount(MaxCount).Filter(items);

            TotalItemCount = items.Count;

            ApplyPagingFilter(items);

            return items;
        }

        /// <summary>
        /// An empty method which can be overridden by descendants which 
        /// implements paging.
        /// </summary>
        /// <param name="itemCount"></param>
        protected virtual void AddHeaderPager(int itemCount) { }

        /// <summary>
        /// An empty method which can be overridden by descendants which 
        /// implements paging.
        /// </summary>
        /// <param name="itemCount"></param>
        protected virtual void AddFooterPager(int itemCount) { }

        /// <summary>
        /// An empty method which can be overridden by descendants which 
        /// implements paging.
        /// </summary>
        /// <param name="items"></param>
        protected virtual void ApplyPagingFilter(PageDataCollection items) { }

        /// <summary>
        /// If this template is defined it will be used for every other item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataItemTemplateContainer))]
        public ITemplate AlternatingItemTemplate { get; set; }

        /// <summary>
        /// A special template for the first item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataItemTemplateContainer))]
        public ITemplate FirstItemTemplate { get; set; }

        /// <summary>
        /// A special template for the second item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataItemTemplateContainer))]
        public ITemplate SecondItemTemplate { get; set; }

        /// <summary>
        /// A special template for the third item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataItemTemplateContainer))]
        public ITemplate ThirdItemTemplate { get; set; }

        /// <summary>
        /// A special template for the fourth item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataItemTemplateContainer))]
        public ITemplate FourthItemTemplate { get; set; }
    }
}
