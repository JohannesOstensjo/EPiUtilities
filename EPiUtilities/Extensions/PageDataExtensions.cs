using EPiServer.Core;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="PageData"/>.
    /// </summary>
    public static class PageDataExtensions
    {
        /// <summary>
        /// Returns the children of the page.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static PageDataCollection Children(this PageData page)
        {
            return page.PageLink.Children();
        }

        /// <summary>
        /// Returns the children of the page filtered with FilterForVisitor.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static PageDataCollection ChildrenForVisitor(this PageData page)
        {
            return page.PageLink.ChildrenForVisitor();
        }

        /// <summary>
        /// Returns the parent of page as <see cref="PageData"/> or null if 
        /// the parent reference is not resolvable.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static PageData Parent(this PageData page)
        {
            if (page.ParentLink.IsResolvable())
                return page.ParentLink.ToPageData();

            return null;
        }

        /// <summary>
        /// Returns true if page is a shortcut to another EPiServer page.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static bool IsShortcutToPage(this PageData page)
        {
            return page.LinkType == PageShortcutType.Shortcut;
        }

        /// <summary>
        /// If page is a shortcut to another EPiServer page, returns that page.
        /// Otherwise returns page itself.
        /// </summary>
        /// <returns></returns>
        public static PageData ToShortcutPageOrSelf(this PageData page)
        {
            if (page != null)
            {
                if (page.IsShortcutToPage())
                {
                    var shortcut = page.PropertyValue("PageShortcutLink", PageReference.EmptyReference);

                    if (shortcut.IsResolvable())
                        return shortcut.ToPageData();
                }

                return page;
            }

            return null;
        }

        /// <summary>
        /// Checks page itself and then ancestors up the tree and returns the 
        /// first page of specified type. Returns null if no page found.
        /// </summary>
        /// <returns></returns>
        public static PageData AncestorOrSelfOfType(this PageData page, int pageTypeId)
        {
            if (page != null)
            {
                if (page.PageTypeID == pageTypeId)
                    return page;

                return page.Parent().AncestorOrSelfOfType(pageTypeId);
            }

            return null;
        }

        /// <summary>
        /// Returns true if the specified property on the page has a value. 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool PropertyHasValue(this PageData page, string propertyName)
        {
            if (page != null)
                if (page.Property[propertyName] != null)
                    return !page.Property[propertyName].IsNull;

            return false;
        }

        /// <summary>
        /// If the specified property has a value and is of type T, returns the value.
        /// Otherwise returns the system default value for T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T PropertyValueOrDefault<T>(this PageData page, string propertyName)
        {
            return page.PropertyValue(propertyName, default(T));
        }

        /// <summary>
        /// If the specified property has a value and is of type T, returns the value.
        /// Otherwise returns the specified default value. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T PropertyValue<T>(this PageData page, string propertyName, T defaultValue)
        {
            if (page.PropertyHasValue(propertyName))
                if (page.Property[propertyName].Value is T)
                    return (T)page.Property[propertyName].Value;

            return defaultValue;
        }

        /// <summary>
        /// Returns true if page is a descendant of ancestorCandidate. Will not compare
        /// page itself. 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ancestorCandidate"></param>
        /// <returns></returns>
        public static bool IsDescendantOf(this PageData page, PageData ancestorCandidate)
        {
            return page.Parent().IsOrIsDescendantOf(ancestorCandidate);
        }

        /// <summary>
        /// Returns true if the page is ancestorCandidate or page is a descendant of 
        /// ancestorCandidate.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ancestorCandidate"></param>
        /// <returns></returns>
        public static bool IsOrIsDescendantOf(this PageData page, PageData ancestorCandidate)
        {
            if (page != null)
            {
                if (page.PageLink.CompareToIgnoreWorkID(ancestorCandidate.PageLink))
                    return true;

                return page.Parent().IsOrIsDescendantOf(ancestorCandidate);
            }

            return false;
        }

        /// <summary>
        /// Returns true if the page is ancestorCandidate, if ancestorCandidate is a shortcut
        /// to page or if page is a descendant of ancestorCandidate.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ancestorCandidate"></param>
        /// <returns></returns>
        public static bool IsOrIsDescendantOfOrShortcut(this PageData page, PageData ancestorCandidate)
        {
            if (page != null)
            {
                if (page.PageLink.CompareToIgnoreWorkID(ancestorCandidate.ToShortcutPageOrSelf().PageLink))
                    return true;

                return page.IsOrIsDescendantOf(ancestorCandidate);
            }

            return false;
        }

        /// <summary>
        /// Returns true if page is a descendant of ancestorCandidate. Will not compare
        /// page itself. 
        /// If ancestorCandidate is a shortcut to another page the linked page will be used
        /// instead. This is useful if you create menu items with shortcuts for instance.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ancestorCandidate"></param>
        /// <returns></returns>
        public static bool IsDescendantOfFollowShortcutToAncestors(this PageData page, PageData ancestorCandidate)
        {
            return page.Parent().IsOrIsDescendantOf(ancestorCandidate.ToShortcutPageOrSelf());
        }


        /// <summary>
        /// Returns true if the page is ancestorCandidate or page is a descendant of 
        /// ancestorCandidate.
        /// If ancestorCandidate is a shortcut to another page the linked page will be used
        /// instead. This is useful if you create menu items with shortcuts for instance.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ancestorCandidate"></param>
        /// <returns></returns>
        public static bool IsOrIsDescendantOfFollowShortcutToAncestors(this PageData page, PageData ancestorCandidate)
        {
            return page.IsOrIsDescendantOf(ancestorCandidate.ToShortcutPageOrSelf());
        }

        /// <summary>
        /// Returns an html anchor with LinkURL as href and PageName as value. 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string ToHtmlAnchorWithLinkUrlAndPageName(this PageData page)
        {
            return page.ToHtmlAnchorWithLinkUrl(page.PageName);
        }

        /// <summary>
        /// Returns an html anchor with LinkURL as href and innerText as value. 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        public static string ToHtmlAnchorWithLinkUrl(this PageData page, string innerText)
        {
            if (page != null)
                return string.Format("<a href=\"{0}\">{1}</a>", page.LinkURL, innerText);

            return "<a href=\"#\">#</a>";
        }
    }
}
