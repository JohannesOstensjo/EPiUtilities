using System;
using System.Web.UI;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using EPiUtilities.Extensions;

// These classes are used in templated controls.

namespace EPiUtilities.WebControls
{
    /// <summary>
    /// Base class for all templates.
    /// </summary>
    public class BaseTemplateContainer : Control, INamingContainer
    {
    }

    /// <summary>
    /// Template class used for headers.
    /// </summary>
    public class HeaderTemplateContainer : BaseTemplateContainer
    {
    }

    /// <summary>
    /// Template class used for empty templates.
    /// </summary>
    public class EmptyTemplateContainer : BaseTemplateContainer
    {
    }

    /// <summary>
    /// Template class used for footers.
    /// </summary>
    public class FooterTemplateContainer : BaseTemplateContainer
    {
    }

    /// <summary>
    /// Template class used for separators.
    /// </summary>
    public class SeparatorTemplateContainer : BaseTemplateContainer
    {
    }

    /// <summary>
    /// Template class used for headers and footers in levels in multilevel controls.
    /// </summary>
    public class LevelHeaderFooterTemplateContainer : BaseTemplateContainer
    {
        private readonly int _level;

        /// <summary>
        /// Creates a new <see cref="LevelHeaderFooterTemplateContainer"/>.
        /// </summary>
        /// <param name="level"></param>
        public LevelHeaderFooterTemplateContainer(int level)
        {
            _level = level;
        }

        /// <summary>
        /// The current level.
        /// </summary>
        public int Level
        {
            get { return _level; }
        }
    }

    /// <summary>
    /// Template class used for pager headers.
    /// </summary>
    public class PagerHeaderFooterTemplateContainer : BaseTemplateContainer
    {
        private readonly int _pageNumber;
        private readonly int _itemCount;
        private readonly int _pageSize;

        /// <summary>
        /// Creates a new <see cref="PagerHeaderFooterTemplateContainer"/>.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="itemCount"></param>
        /// <param name="pageSize"></param>
        public PagerHeaderFooterTemplateContainer(int pageNumber, int itemCount, int pageSize)
        {
            _pageNumber = pageNumber;
            _itemCount = itemCount;
            _pageSize = pageSize;
        }

        /// <summary>
        /// The number of the current page being shown.
        /// </summary>
        public int PageNumber
        {
            get { return _pageNumber; }
        }

        /// <summary>
        /// The total number of items. 
        /// </summary>
        public int ItemCount
        {
            get { return _itemCount; }
        }

        /// <summary>
        /// The size of a page.
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
        }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        public int NumberOfPages
        {
            get { return (int) Math.Ceiling((decimal) _itemCount/_pageSize); }
        }

        /// <summary>
        /// Returns the number of the first item being shown. 
        /// </summary>
        public int FromItemNumber
        {
            get { return (_pageNumber * _pageSize) + 1; }
        }

        /// <summary>
        /// Returns the number of the last item being shown. 
        /// </summary>
        public int ToItemNumber
        {
            get
            {
                int toNumber = (_pageNumber * _pageSize) + _pageSize;

                if (toNumber > _itemCount)
                    return _itemCount;

                return toNumber;
            }
        }
    }

    /// <summary>
    /// Template class used for pager items.
    /// </summary>
    public class PagerItemTemplateContainer : BaseTemplateContainer
    {
        private readonly string _url;
        private readonly int _pageNumber;
        private readonly string _text;
        private readonly bool _selected;

        /// <summary>
        /// Creates a new <see cref="PagerItemTemplateContainer"/>.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pageNumber"></param>
        /// <param name="text"></param>
        /// <param name="selected"></param>
        public PagerItemTemplateContainer(string url, int pageNumber, string text, bool selected)
        {
            _url = url;
            _pageNumber = pageNumber;
            _text = text;
            _selected = selected;
        }

        /// <summary>
        /// The url for this item. 
        /// The url is encoded for use in html attributes, do not encode it a second time.
        /// </summary>
        public string Url
        {
            get { return _url.ToHtmlAttributeEncoded(); }
        }

        /// <summary>
        /// The page number this item represents.
        /// </summary>
        public int PageNumber
        {
            get { return _pageNumber; }
        }

        /// <summary>
        /// The text for this item.
        /// </summary>
        public string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Whether this item represents the current page or not. 
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
        }
    }

    /// <summary>
    /// Base item template class.
    /// </summary>
    public abstract class BaseItemTemplateContainer<T> : BaseTemplateContainer
    {
        private readonly T _item;
        private readonly int _itemNumber;
        private readonly bool _selected;

        /// <summary>
        /// Base constructor for <see cref="BaseItemTemplateContainer{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        protected BaseItemTemplateContainer(T item, int itemNumber)
        {
            _item = item;
            _itemNumber = itemNumber;
        }

        /// <summary>
        /// Base constructor for <see cref="BaseItemTemplateContainer{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        /// <param name="selected"></param>
        protected BaseItemTemplateContainer(T item, int itemNumber, bool selected)
        {
            _item = item;
            _itemNumber = itemNumber;
            _selected = selected;
        }

        /// <summary>
        /// The item this template represents. 
        /// </summary>
        public T Item
        {
            get { return _item; }
        }

        /// <summary>
        /// The number in the list this item is, starting at 1.
        /// </summary>
        public int ItemNumber
        {
            get { return _itemNumber; }
        }

        /// <summary>
        /// True if item is considered selected. If you do not specify a 
        /// SelectedItemTemplate in controls which define selected items, 
        /// the ItemTemplate will be used and this property will be true.
        /// Useful if you only want to output a css class on selected items 
        /// for instance. 
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
        }

        /// <summary>
        /// True if this item has an odd number in the item number sequence.
        /// </summary>
        public bool Odd
        {
            get { return ItemNumber % 2 == 1; }
        }

        /// <summary>
        /// True if this item has an even number in the item number sequence.
        /// </summary>
        public bool Even
        {
            get { return ItemNumber % 2 == 0; }
        }
    }

    /// <summary>
    /// Item template with <see cref="PageData"/> object as strongly typed item.
    /// </summary>
    public class PageDataItemTemplateContainer : BaseItemTemplateContainer<PageData>
    {
        /// <summary>
        /// Creates a new <see cref="PageDataItemTemplateContainer"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        public PageDataItemTemplateContainer(PageData item, int itemNumber) : base(item, itemNumber) { }

        /// <summary>
        /// Creates a new <see cref="PageDataItemTemplateContainer"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        /// <param name="selected"></param>
        public PageDataItemTemplateContainer(PageData item, int itemNumber, bool selected) : base(item, itemNumber, selected) { }
    }

    /// <summary>
    /// Item template with additional properties for <see cref="MultiLevelMenu"/>.
    /// </summary>
    public class PageDataMultiLevelItemTemplateContainer : PageDataItemTemplateContainer
    {
        private readonly int _level;
        private readonly bool _hasChildren;

        /// <summary>
        /// Creates a new <see cref="PageDataMultiLevelItemTemplateContainer"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        /// <param name="selected"></param>
        /// <param name="level"></param>
        /// <param name="hasChildren"></param>
        public PageDataMultiLevelItemTemplateContainer(PageData item, int itemNumber, bool selected, int level, bool hasChildren) : base(item, itemNumber, selected)
        {
            _level = level;
            _hasChildren = hasChildren;
        }
        
        /// <summary>
        /// The current level.
        /// </summary>
        public int Level
        {
            get { return _level; }
        }

        /// <summary>
        /// Whether or not this item will have a sublevel with items.
        /// </summary>
        public bool HasChildren
        {
            get { return _hasChildren; }
        }
    }

    /// <summary>
    /// Item template with <see cref="LinkItem"/> object as strongly typed item.
    /// </summary>
    public class LinkItemItemTemplateContainer : BaseItemTemplateContainer<LinkItem>
    {
        /// <summary>
        /// Creates a new <see cref="LinkItemItemTemplateContainer"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        public LinkItemItemTemplateContainer(LinkItem item, int itemNumber) : base(item, itemNumber) { }

        /// <summary>
        /// Creates a new <see cref="LinkItemItemTemplateContainer"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        /// <param name="selected"></param>
        public LinkItemItemTemplateContainer(LinkItem item, int itemNumber, bool selected) : base(item, itemNumber, selected) { }
    }
}
