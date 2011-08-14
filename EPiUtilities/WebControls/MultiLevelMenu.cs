﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using EPiServer.Core;
using EPiUtilities.Extensions;
using EPiUtilities.WebControls.BaseClasses;

namespace EPiUtilities.WebControls
{
    /// <summary>
    /// A menu control which displays pages of a specified number of levels 
    /// below a menu root. 
    /// </summary>
    [ParseChildren(true)]
    public class MultiLevelMenu : ChildrenBasedPageDataMenuBase
    {
        protected Dictionary<int, bool> AddSeparatorForLevel = new Dictionary<int, bool>();
        protected Dictionary<int, bool> AddedForLevel = new Dictionary<int, bool>();

        /// <summary>
        /// If true, only selected items will have their children listed. 
        /// </summary>
        public bool ExpandSelectedOnly { get; set; }

        /// <summary>
        /// The number of levels to display in the control.
        /// </summary>
        public int NumberOfLevels { get; set; }

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

                    AddLevel(items, 1);

                    AddFooterTemplate();
                }
                else
                    HideOrEmpty();
            }
            else
                HideOrEmpty();
        }

        protected void AddLevel(PageDataCollection items, int level)
        {
            if (items.Count > 0)
            {
                AddLevelStartTemplate(level);

                for (int i = 0; i < items.Count; i++)
                {
                    AddItem(items[i], i + 1, level);
                }

                AddLevelEndTemplate(level);
            }
        }

        protected void AddItem(PageData item, int itemNumber, int level)
        {
            var children = new PageDataCollection();
            if (!ShowPagesNotVisibleInMenu)
                children.VisibleInMenu();

            bool isSelected = IsSelected(item);

            if (NumberOfLevels > level)
                if (ExpandSelectedOnly)
                {
                    if (isSelected)
                        children = item.ChildrenForVisitor();
                }
                else
                    children = item.ChildrenForVisitor();

            if (isSelected)
            {
                if (SelectedItemTemplate != null)
                    AddItemTemplate(SelectedItemTemplate, item, itemNumber, true, level, children.Count > 0);
                else if (ItemTemplate != null)
                    AddItemTemplate(ItemTemplate, item, itemNumber, true, level, children.Count > 0);
            }
            else
                if (ItemTemplate != null)
                    AddItemTemplate(ItemTemplate, item, itemNumber, false, level, children.Count > 0);

            AddLevel(children, level + 1);

            if (isSelected)
            {
                if (SelectedItemEndTemplate != null)
                    AddItemTemplate(SelectedItemEndTemplate, item, itemNumber, true, level, children.Count > 0);
                else if (ItemEndTemplate != null)
                    AddItemTemplate(ItemEndTemplate, item, itemNumber, true, level, children.Count > 0);
            }
            else
                if (ItemEndTemplate != null)
                    AddItemTemplate(ItemEndTemplate, item, itemNumber, false, level, children.Count > 0);

            if (!AddedForLevel.ContainsKey(level))
                AddedForLevel.Add(level, false);

            if (AddedForLevel[level])
            {
                if (!AddSeparatorForLevel.ContainsKey(level))
                    AddSeparatorForLevel.Add(level, true);
                else
                    AddSeparatorForLevel[level] = true;

                AddedForLevel[level] = false;
            }
        }

        protected void AddItemTemplate(ITemplate template, PageData item, int itemNumber, bool selected, int level, bool hasChildren)
        {
            if (!AddSeparatorForLevel.ContainsKey(level))
                AddSeparatorForLevel.Add(level, false);

            if (AddSeparatorForLevel[level])
            {
                AddSeparatorTemplate();
                AddSeparatorForLevel[level] = false;
            }

            AddTemplate(new PageDataMultiLevelItemTemplateContainer(item, itemNumber, selected, level, hasChildren), template);

            if (!AddedForLevel.ContainsKey(level))
                AddedForLevel.Add(level, true);
            else
                AddedForLevel[level] = true;
        }

        protected void AddLevelStartTemplate(int level)
        {
            if (LevelStartTemplate != null)
                AddTemplate(new LevelHeaderFooterTemplateContainer(level), LevelStartTemplate);
        }

        protected void AddLevelEndTemplate(int level)
        {
            if (LevelEndTemplate != null)
                AddTemplate(new LevelHeaderFooterTemplateContainer(level), LevelEndTemplate);
        }

        /// <summary>
        /// The template will be applied at the start of every 
        /// level. 
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(LevelHeaderFooterTemplateContainer))]
        public ITemplate LevelStartTemplate { get; set; }

        /// <summary>
        /// The template will be applied at the end of every level. 
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(LevelHeaderFooterTemplateContainer))]
        public ITemplate LevelEndTemplate { get; set; }

        /// <summary>
        /// The item template will be used for all items if not 
        /// an alternative item template is defined. 
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataMultiLevelItemTemplateContainer))]
        public new ITemplate ItemTemplate { get; set; }

        /// <summary>
        /// This template is applied to an item after its nested
        /// children are added. 
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataMultiLevelItemTemplateContainer))]
        public ITemplate ItemEndTemplate { get; set; }

        /// <summary>
        /// The selected item template will be used for items which 
        /// are considered selected.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataMultiLevelItemTemplateContainer))]
        public new ITemplate SelectedItemTemplate { get; set; }

        /// <summary>
        /// This template is applied to a selected item after its nested
        /// children are added. 
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty),
         DefaultValue(typeof(ITemplate), null),
         TemplateContainer(typeof(PageDataMultiLevelItemTemplateContainer))]
        public ITemplate SelectedItemEndTemplate { get; set; }
    }
}
