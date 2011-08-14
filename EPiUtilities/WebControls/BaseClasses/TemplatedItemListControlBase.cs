﻿using System.ComponentModel;
using System.Web.UI;

namespace EPiUtilities.WebControls.BaseClasses
{
    /// <summary>
    /// Defines header, footer, separator and empty template which is common to
    /// many templated controls.
    /// Does not define item templates as they vary in type and number. 
    /// </summary>
    public abstract class TemplatedItemListControlBase : WebControlBase
    {
        /// <summary>
        /// If true, the control will set itself invisible when there are
        /// no items instead of showing the empty template.
        /// </summary>
        public bool InvisibleIfEmpty { get; set; }

        protected void AddHeaderTemplate()
        {
            if (HeaderTemplate != null)
                AddTemplate(new HeaderTemplateContainer(), HeaderTemplate);
        }

        protected void AddFooterTemplate()
        {
            if (FooterTemplate != null)
                AddTemplate(new FooterTemplateContainer(), FooterTemplate);
        }

        protected void AddSeparatorTemplate()
        {
            if (SeparatorTemplate != null)
                AddTemplate(new SeparatorTemplateContainer(), SeparatorTemplate);
        }

        protected void HideOrEmpty()
        {
            if (InvisibleIfEmpty)
                Visible = false;
            else
                if (EmptyTemplate != null)
                    AddTemplate(new EmptyTemplateContainer(), EmptyTemplate);
        }

        protected void AddTemplate(BaseTemplateContainer container, ITemplate template)
        {
            template.InstantiateIn(container);
            Controls.Add(container);
            container.DataBind();
        }

        /// <summary>
        /// Header template for the control.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(HeaderTemplateContainer))]
        public ITemplate HeaderTemplate { get; set; }

        /// <summary>
        /// Footer template for the control.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(FooterTemplateContainer))]
        public ITemplate FooterTemplate { get; set; }

        /// <summary>
        /// This template will be displayed if there are no items.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(EmptyTemplateContainer))]
        public ITemplate EmptyTemplate { get; set; }

        /// <summary>
        /// The separator template will be displayed in between items.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(SeparatorTemplateContainer))]
        public virtual ITemplate SeparatorTemplate { get; set; }
    }
}
