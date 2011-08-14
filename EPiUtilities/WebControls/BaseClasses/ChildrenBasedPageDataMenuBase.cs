using System.Web;
using EPiServer.Core;
using EPiUtilities.Extensions;

namespace EPiUtilities.WebControls.BaseClasses
{
    /// <summary>
    /// Base class for menu controls which display children collections. 
    /// </summary>
    public abstract class ChildrenBasedPageDataMenuBase : TemplatedSelectedPageDataItemListControlBase
    {
        /// <summary>
        /// The root page for the menu.  
        /// </summary>
        public PageReference MenuRoot { get; set; }

        /// <summary>
        /// If true, pages marked not visible in menu will be included.
        /// Default value is false. 
        /// </summary>
        public bool ShowPagesNotVisibleInMenu { get; set; }

        /// <summary>
        /// If true, the item selected check will follow shortcut pages
        /// and check their ancestors instead. 
        /// Enable this if you have created a menu with shortcut pages.
        /// 
        /// If false, the item selected check will check if items are 
        /// a shortcut to current page or is an ancestor of current page.
        /// </summary>
        public bool SelectedCheckFollowsShortcutsToAncestors { get; set; }

        /// <summary>
        /// A reference to the current page in EPiServer. Is used to 
        /// determine if items are selected or not. 
        /// </summary>
        protected PageData CurrentPage
        {
            get { return HttpContext.Current.Handler.CurrentPage(); }
        }

        protected void AddItemCheckSelected(PageData item, int itemNumber)
        {
            if (IsSelected(item))
                if (SelectedItemTemplate != null)
                    AddItemTemplate(SelectedItemTemplate, item, itemNumber, true);
                else if (ItemTemplate != null)
                    AddItemTemplate(ItemTemplate, item, itemNumber, true);

            if (!Added && ItemTemplate != null)
                AddItemTemplate(ItemTemplate, item, itemNumber);
        }

        /// <summary>
        /// Whether or not the page is considered selected.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        protected bool IsSelected(PageData page)
        {
            return SelectedCheckFollowsShortcutsToAncestors ? CurrentPage.IsOrIsDescendantOfFollowShortcutToAncestors(page) : CurrentPage.IsOrIsDescendantOfOrShortcut(page);
        }
    }
}
