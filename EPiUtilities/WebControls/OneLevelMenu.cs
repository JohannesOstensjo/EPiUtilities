using System;
using System.Web.UI;
using EPiServer.Core;
using EPiUtilities.Extensions;
using EPiUtilities.WebControls.BaseClasses;

namespace EPiUtilities.WebControls
{
    /// <summary>
    /// A simple menu control which displays one level of pages below 
    /// the specified menu root. 
    /// </summary>
    [ParseChildren(true)]
    public class OneLevelMenu : ChildrenBasedPageDataMenuBase
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (MenuRoot.IsResolvable())
            {
                PageDataCollection items = MenuRoot.ChildrenForVisitor();

                if (!ShowPagesNotVisibleInMenu)
                    items.VisibleInMenu();

                if (items.Count > 0)
                {
                    AddHeaderTemplate();

                    for (int i = 0; i < items.Count; i++)
                    {
                        AddItemCheckSelected(items[i], i + 1);

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
    }
}
