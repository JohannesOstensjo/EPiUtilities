using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using EPiServer.Core;
using EPiUtilities.Extensions;

namespace EPiUtilities.WebControls
{
    /// <summary>
    /// A templated PageData list with paging. 
    /// </summary>
    [ParseChildren(true)]
    public class PagedPageDataList : PageDataList
    {
        private int _pageSize;

        /// <summary>
        /// The number of items on each page when paging.
        /// Defaults to 10.
        /// 
        /// The MaxCount property has the same behavior as in a 
        /// non paged list, it limits the total number of items.
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize > 0)
                    return _pageSize;

                return 10;
            }
            set { _pageSize = value; }
        }

        private string _pagingQueryParameterKey;

        /// <summary>
        /// The query string parameter the control uses to determine
        /// which page is the current page. 
        /// Defaults to "p". 
        /// </summary>
        public string PagingQueryParameterKey
        {
            get
            {
                if (!_pagingQueryParameterKey.NullOrEmpty())
                    return _pagingQueryParameterKey;

                return "p";
            }
            set { _pagingQueryParameterKey = value; }
        }

        /// <summary>
        /// Tries to get the current page number from the query parameter value.
        /// Defaults to 1. 
        /// </summary>
        public int CurrentPageNumber
        {
            get
            {
                int i;

                if (Int32.TryParse(HttpContext.Current.Request.QueryString[PagingQueryParameterKey] ?? "", out i))
                    if (i > 0)
                        return i;

                return 1;
            }
        }

        /// <summary>
        /// The text to use for the previous item.
        /// Defaults to &lt;&lt;.
        /// </summary>
        public string PrevText { get; set; }

        /// <summary>
        /// The text to use for the next item. 
        /// Defaults to &gt;&gt;.
        /// </summary>
        public string NextText { get; set; }

        /// <summary>
        /// If true, shows paging at the top of the control. 
        /// Default is false.
        /// </summary>
        public bool ShowTopPager { get; set; }

        /// <summary>
        /// If true, hides paging at the bottom of the control.
        /// Default is false. 
        /// </summary>
        public bool HideBottomPager { get; set; }

        /// <summary>
        /// Applies a pager filter to the items to limit items to the current page.
        /// </summary>
        /// <param name="items"></param>
        protected override void ApplyPagingFilter(PageDataCollection items)
        {
            items.FilterForPager(PageSize, CurrentPageNumber);
        }

        /// <summary>
        /// Adds a pager to the header if specified. 
        /// </summary>
        /// <param name="itemCount"></param>
        protected override void AddHeaderPager(int itemCount)
        {
            if (ShowTopPager)
                AddPager(itemCount);
        }

        /// <summary>
        /// Adds a pager to the footer if specified.
        /// </summary>
        /// <param name="itemCount"></param>
        protected override void AddFooterPager(int itemCount)
        {
            if (!HideBottomPager)
                AddPager(itemCount);
        }

        /// <summary>
        /// Adds a full set of pager templates to the controls. 
        /// </summary>
        /// <param name="itemCount"></param>
        protected void AddPager(int itemCount)
        {
            var numberOfPages = (int)Math.Ceiling((decimal)itemCount / PageSize);

            AddPagerHeaderTemplate(CurrentPageNumber, itemCount, PageSize);

            if (CurrentPageNumber > 1 && PagerPrevTemplate != null)
            {
                AddPagerItemTemplate(PagerPrevTemplate, CurrentPageNumber - 1, (!PrevText.NullOrEmpty() ? PrevText : "&lt;&lt;"), false);
                AddPagerSeparatorTemplate();
            }

            for (int i = 1; i <= numberOfPages; i++)
            {
                if (i == CurrentPageNumber)
                {
                    if (PagerSelectedItemTemplate != null)
                        AddPagerItemTemplate(PagerSelectedItemTemplate, i, i.ToString(), true);
                    else if (PagerItemTemplate != null)
                        AddPagerItemTemplate(PagerItemTemplate, i, i.ToString(), true);
                }
                else
                    AddPagerItemTemplate(PagerItemTemplate, i, i.ToString(), false);

                if (i < numberOfPages)
                    AddPagerSeparatorTemplate();
            }

            if (CurrentPageNumber < numberOfPages && PagerNextTemplate != null)
            {
                AddPagerSeparatorTemplate();
                AddPagerItemTemplate(PagerNextTemplate, CurrentPageNumber + 1, (!NextText.NullOrEmpty() ? NextText : "&gt;&gt;"), false);
            }

            AddPagerFooterTemplate(CurrentPageNumber, itemCount, PageSize);
        }

        /// <summary>
        /// Adds a pager item template.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="pageNumber"></param>
        /// <param name="text"></param>
        /// <param name="selected"></param>
        protected void AddPagerItemTemplate(ITemplate template, int pageNumber, string text, bool selected)
        {
            AddTemplate(new PagerItemTemplateContainer(GetCurrentUrlWithParameterChanged(PagingQueryParameterKey, pageNumber.ToString()), pageNumber, text, selected), template);
        }

        /// <summary>
        /// Adds a pager header template. 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="itemCount"></param>
        /// <param name="pageSize"></param>
        protected void AddPagerHeaderTemplate(int pageNumber, int itemCount, int pageSize)
        {
            if (HeaderTemplate != null)
                AddTemplate(new PagerHeaderFooterTemplateContainer(pageNumber, itemCount, pageSize), HeaderTemplate);
        }

        /// <summary>
        /// Adds a pager footer template.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="itemCount"></param>
        /// <param name="pageSize"></param>
        protected void AddPagerFooterTemplate(int pageNumber, int itemCount, int pageSize)
        {
            if (FooterTemplate != null)
                AddTemplate(new PagerHeaderFooterTemplateContainer(pageNumber, itemCount, pageSize), FooterTemplate);
        }

        /// <summary>
        /// Adds a pager separator template.
        /// </summary>
        protected void AddPagerSeparatorTemplate()
        {
            if (PagerSeparatorTemplate != null)
                AddTemplate(new SeparatorTemplateContainer(), PagerSeparatorTemplate);
        }

        /// <summary>
        /// Header template for the pager.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PagerHeaderFooterTemplateContainer))]
        public ITemplate PagerHeaderTemplate { get; set; }

        /// <summary>
        /// Footer template for the pager.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PagerHeaderFooterTemplateContainer))]
        public ITemplate PagerFooterTemplate { get; set; }

        /// <summary>
        /// Previous template for the pager.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PagerItemTemplateContainer))]
        public ITemplate PagerPrevTemplate { get; set; }

        /// <summary>
        /// Next template for the pager.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PagerItemTemplateContainer))]
        public ITemplate PagerNextTemplate { get; set; }

        /// <summary>
        /// Item template for the pager.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PagerItemTemplateContainer))]
        public ITemplate PagerItemTemplate { get; set; }

        /// <summary>
        /// Selected item template for the pager.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PagerItemTemplateContainer))]
        public ITemplate PagerSelectedItemTemplate { get; set; }

        /// <summary>
        /// Separator template for the pager.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(SeparatorTemplateContainer))]
        public ITemplate PagerSeparatorTemplate { get; set; }

        private string GetCurrentUrlWithParameterChanged(string key, string value)
        {
            // http://stackoverflow.com/questions/3813934/change-single-url-query-string-value
            var nameValues = HttpUtility.ParseQueryString(HttpContext.Current.Request.QueryString.ToString());
            nameValues.Set(key, value);
            return HttpContext.Current.Request.Url.AbsolutePath + "?" + nameValues;
        }
    }
}
