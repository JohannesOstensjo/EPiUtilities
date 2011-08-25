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
        /// <summary>
        /// Indicates whether a separator should be added before adding a new item. 
        /// </summary>
        protected bool AddSeparator;

        /// <summary>
        /// Indicates whether an item was added during an iteration. 
        /// </summary>
        protected bool Added;

        /// <summary>
        /// Adds an item with the specified template. Assumes selected is false.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        protected void AddItemTemplate(ITemplate template, PageData item, int itemNumber)
        {
            AddItemTemplate(template, item, itemNumber, false);
        }

        /// <summary>
        /// Adds an item with the specified template.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="item"></param>
        /// <param name="itemNumber"></param>
        /// <param name="selected"></param>
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
