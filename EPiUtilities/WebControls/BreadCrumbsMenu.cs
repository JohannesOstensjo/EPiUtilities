using System;
using System.Web;
using System.Web.UI;
using EPiServer.Configuration;
using EPiServer.Core;
using EPiUtilities.Extensions;
using EPiUtilities.WebControls.BaseClasses;

namespace EPiUtilities.WebControls
{
    /// <summary>
    /// A menu which displays "breadcrumbs", links to the pages between the
    /// site root and the current page. 
    /// Site root can be set, but defaults to the pageStartId set in web.config.
    /// Current page can be set, but defaults to the current EPiServer page.
    /// The current page is always considered selected, the others not. 
    /// </summary>
    [ParseChildren(true)]
    public class BreadCrumbsMenu : TemplatedSelectedPageDataItemListControlBase
    {
        /// <summary>
        /// If true, pages marked not visible in menu will be included.
        /// Default value is false. 
        /// </summary>
        public bool ShowPagesNotVisibleInMenu { get; set; }

        private PageReference _siteRoot;

        /// <summary>
        /// The root of the bread crumbs menu. Defaults to the site start page.
        /// </summary>
        public PageReference SiteRoot
        {
            get
            {
                if (_siteRoot.IsResolvable())
                    return _siteRoot;

                return Settings.Instance.PageStartId.ToPageReference();
            }
            set { _siteRoot = value; }
        }

        private PageReference _currentPage;

        /// <summary>
        /// The current page of the bread crumbs, which is the end of the
        /// menu. Can be set or defaults to the current EPiServer page. 
        /// </summary>
        public PageReference CurrentPage
        {
            get
            {
                if (_currentPage.IsResolvable())
                    return _currentPage;

                return HttpContext.Current.Handler.CurrentPage().PageLink;
            }
            set { _currentPage = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (CurrentPage.IsResolvable())
            {
                var items = GetItems();

                if (items.Count > 0)
                {
                    AddHeaderTemplate();

                    for (int i = 0; i < items.Count; i++)
                    {
                        if (i == items.Count - 1)
                            if (SelectedItemTemplate != null)
                                AddItemTemplate(SelectedItemTemplate, items[i], i + 1, true);
                            else if (ItemTemplate != null)
                                AddItemTemplate(ItemTemplate, items[i], i + 1, true);

                        if (!Added && ItemTemplate != null)
                            AddItemTemplate(ItemTemplate, items[i], i + 1);

                        if (Added)
                        {
                            Added = false;
                            AddSeparator = true;
                        }

                    }

                    AddFooterTemplate();
                }
                else
                    HideOrEmpty();
            }
            else
                HideOrEmpty();
        }

        private PageDataCollection GetItems()
        {
            var items = new PageDataCollection();
            PageData currentItem = CurrentPage.ToPageData();

            items.Add(currentItem);
            AddParentIfRequired(currentItem, items);

            items.ForVisitor();
            if (!ShowPagesNotVisibleInMenu)
                items.VisibleInMenu();

            return items;
        }

        private void AddParentIfRequired(PageData child, PageDataCollection items)
        {
            if (child.PageLink.CompareToIgnoreWorkID(SiteRoot))
                return;

            if (child.ParentLink.IsResolvable())
            {
                if (child.ParentLink.CompareToIgnoreWorkID(Settings.Instance.PageRootId.ToPageReference()))
                    return;

                items.Insert(0, child.ParentLink.ToPageData());

                AddParentIfRequired(child.ParentLink.ToPageData(), items);
            }
        }
    }
}
