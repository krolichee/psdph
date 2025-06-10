using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public class StructureStackHandler : TemplateStackHandler
    {
        override protected MenuItem CreateAddMenuItem(Type type)
        {
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = new StructureCommand(Context).CreateCommand,
                CommandParameter = type
            };
        }
        override protected void InitializeAddDropDownMenu(Button button)
        {
            ContextMenu contextMenu = new ContextMenu();
            List<MenuItem> items = new List<MenuItem>();
            foreach (var comp_type in StructureDicts.CreatorDict.Keys)
                items.Add(CreateAddMenuItem(comp_type));
            contextMenu.ItemsSource = items;
            button.ContextMenu = contextMenu;
        }

        protected override UIElement createControl(object item)
        {
            return new StructureStackControl((Composition)item, Context);
        }
        protected override object[] getElements() =>
            _root.GetChildren();

        public StructureStackHandler(PsdPhContext context) : base(context)
        {
            _root.ChildrenUpdatedEvent += Refresh;
        }
    }
}
