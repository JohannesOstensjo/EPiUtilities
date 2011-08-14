using System.ComponentModel;
using System.Web.UI;
using EPiServer.Core;

namespace EPiUtilities.WebControls.BaseClasses
{
    /// <summary>
    /// Base class for templated item lists where item is <see cref="PageData"/> and 
    /// items may also be selected.
    /// </summary>
    public abstract class TemplatedSelectedPageDataItemListControlBase : TemplatedPageDataItemListControlBase
    {
        /// <summary>
        /// The selected item template will be used for items which 
        /// are considered selected.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataItemTemplateContainer))]
        public ITemplate SelectedItemTemplate { get; set; }
    }
}
