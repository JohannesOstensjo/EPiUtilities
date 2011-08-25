using System;
using System.ComponentModel;
using System.Web.UI;
using EPiServer.SpecializedProperties;
using EPiUtilities.WebControls.BaseClasses;

namespace EPiUtilities.WebControls
{
    /// <summary>
    /// A simple templated LinkItem list.
    /// </summary>
    [ParseChildren(true)]
    public class LinkItemList : TemplatedItemListControlBase
    {
        private bool _addSeparator;
        private bool _added;

        /// <summary>
        /// Can be used to get or set the linkitems in the list.
        /// </summary>
        public LinkItemCollection LinkItems
        {
            get
            {
                if (DataSource is LinkItemCollection)
                    return (LinkItemCollection)DataSource;

                return null;
            }
            set { DataSource = value; }
        }

        /// <summary>
        /// Override which creates and adds the content of the control. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var items = new LinkItemCollection();

            if (DataSource != null)
                if (DataSource is LinkItemCollection)
                    items = (LinkItemCollection)DataSource;

            if (items.Count > 0)
            {
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

                    if (!_added && i % 2 == 1 && AlternatingItemTemplate != null)
                        AddItemTemplate(AlternatingItemTemplate, items[i], i + 1);

                    if (!_added && ItemTemplate != null)
                        AddItemTemplate(ItemTemplate, items[i], i + 1);

                    if (_added)
                    {
                        _added = false;
                        _addSeparator = true;
                    }
                }

                AddFooterTemplate();
            }
            else
            {
                HideOrEmpty();
            }
        }

        /// <summary>
        /// Adds an item with the specified template. 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        protected void AddItemTemplate(ITemplate template, LinkItem item, int itemNumber)
        {
            if (_addSeparator)
                AddSeparatorTemplate();
            AddTemplate(new LinkItemItemTemplateContainer(item, itemNumber), template);
            _added = true;
        }

        /// <summary>
        /// The item template will be used for all items if not 
        /// an alternative item template is defined. 
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(LinkItemItemTemplateContainer))]
        public ITemplate ItemTemplate { get; set; }

        /// <summary>
        /// If this template is defined it will be used for every other item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(LinkItemItemTemplateContainer))]
        public ITemplate AlternatingItemTemplate { get; set; }

        /// <summary>
        /// A special template for the first item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(LinkItemItemTemplateContainer))]
        public ITemplate FirstItemTemplate { get; set; }

        /// <summary>
        /// A special template for the second item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(LinkItemItemTemplateContainer))]
        public ITemplate SecondItemTemplate { get; set; }

        /// <summary>
        /// A special template for the third item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(LinkItemItemTemplateContainer))]
        public ITemplate ThirdItemTemplate { get; set; }

        /// <summary>
        /// A special template for the fourth item.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(LinkItemItemTemplateContainer))]
        public ITemplate FourthItemTemplate { get; set; }
    }
}
