using System.ComponentModel;
using System.Web.UI;
using EPiServer.Core;

namespace EPiUtilities.WebControls.BaseClasses
{
    /// <summary>
    /// Base class for templated item lists where item is <see cref="PageData"/>.
    /// </summary>
    public abstract class TemplatedPageDataItemListControlBase : TemplatedItemListControlBase
    {
        protected bool AddSeparator;
        protected bool Added;

        protected void AddItemTemplate(ITemplate template, PageData item, int itemNumber)
        {
            AddItemTemplate(template, item, itemNumber, false);
        }

        protected void AddItemTemplate(ITemplate template, PageData item, int itemNumber, bool selected)
        {
            if (AddSeparator)
                AddSeparatorTemplate();
            AddTemplate(new PageDataItemTemplateContainer(item, itemNumber, selected), template);
            Added = true;
        }

        /// <summary>
        /// The item template will be used for all items if not 
        /// an alternative item template is defined. 
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataItemTemplateContainer))]
        public ITemplate ItemTemplate { get; set; }
    }
}
