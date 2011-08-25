using System.Web.UI;
using System.Web.UI.WebControls;

namespace EPiUtilities.WebControls.BaseClasses
{
    /// <summary>
    /// Base control for web controls. 
    /// </summary>
    public abstract class WebControlBase : WebControl
    {
        /// <summary>
        /// If true, the control will call DataBind() on itself on PreRender if 
        /// it is not already databound.
        /// </summary>
        public virtual bool AutoBind { get; set; }

        /// <summary>
        /// This property can be used to fill the control with data.
        /// </summary>
        public virtual object DataSource { get; set; }

        /// <summary>
        /// We don't want any other markup than our own from templates.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(HtmlTextWriter writer) { }

        /// <summary>
        /// We don't want any other markup than our own from templates.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer) { }

        private bool _dataBound;

        /// <summary>
        /// Override to mark control as databound. 
        /// </summary>
        public override void DataBind()
        {
            _dataBound = true;

            base.DataBind();
        }

        /// <summary>
        /// Override to mark control as databound. 
        /// </summary>
        protected override void DataBind(bool raiseOnDataBinding)
        {
            _dataBound = true;

            base.DataBind(raiseOnDataBinding);
        }

        /// <summary>
        /// Override which performs AutoBind.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(System.EventArgs e)
        {
            if (AutoBind)
                if (!_dataBound)
                    DataBind();

            base.OnPreRender(e);
        }

        /// <summary>
        /// We don't want to use ViewState. 
        /// 
        /// Why? 
        /// 
        /// Most of the time EPiServer is used as a publishing system, and 
        /// postbacks are rare. ViewState is really only necessary for postbacks,
        /// and even then you can easily databind a second time. ViewState is 
        /// expensive in terms of encoding/decoding as well as bandwidth. 
        /// Putting a large PageDataCollection in ViewState can cripple the 
        /// performance of a web site if you are not careful, so best to just 
        /// disable it. 
        /// </summary>
        public override bool EnableViewState
        {
            get { return false; }
            set { }
        }
    }
}
