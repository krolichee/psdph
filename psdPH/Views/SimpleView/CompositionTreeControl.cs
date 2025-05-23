using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Views.SimpleView
{
    public class CompositionTreeControl : TreeViewItem
    {
        public CompositionTreeControl(Blob blob) : base()
        {
            Header = blob.ObjName;

            var parameters_stack = new StackPanel();
            foreach (var child in blob.getChildren())
            {
                if (child is Blob)
                {
                    if (!(child as Blob).IsPrototyped())
                        parameters_stack.Children.Add(new CompositionTreeControl(child as Blob));
                }
                else
                    foreach (var parameter in child.Setups)
                    {
                        parameters_stack.Children.Add(parameter.Stack); 
                    }
                
            }
            Items.Add(parameters_stack);
        }
    }
}
