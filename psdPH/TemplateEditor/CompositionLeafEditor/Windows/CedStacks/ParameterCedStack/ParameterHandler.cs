using psdPH.Logic.Parameters;
using psdPH.Utils.CedStack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.CedStacks.ParameterCedStack
{
    public class ParameterHandler:CEDStackHandler
    {

        protected ParameterSet ParameterSet;
        override protected MenuItem CreateAddMenuItem(Type type)
        {
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = new ParameterCommand(ParameterSet).CreateCommand,
                CommandParameter = type
            };
        }
        override protected void InitializeAddDropDownMenu(Button button)
        {
            ContextMenu contextMenu = new ContextMenu();
            List<MenuItem> items = new List<MenuItem>();
            foreach (var ruleType in ParameterDicts.CreatorDict.Keys)
                items.Add(CreateAddMenuItem(ruleType));
            contextMenu.ItemsSource = items;
            button.ContextMenu = contextMenu;
           
        }

        protected override FrameworkElement createControl(object item)
        {
            return new ParameterControl(item as Parameter, ParameterSet);
        }

        protected override object[] getElements()
        {
            return ParameterSet.AsCollection().ToArray();
        }
        protected override IList Items => this.ParameterSet.Parameters as IList;

        public ParameterHandler(ParameterSet parameterSet)
        {
            ParameterSet = parameterSet;
            ParameterSet.Updated += Refresh;
        }
    }
}
