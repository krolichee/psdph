using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static psdPH.TemplateEditor.StructureDicts;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    class CompositionStackElement:Button
    {
        public CompositionStackElement(Composition composition,ICommand command)
        {
            var grid = new Grid();
            grid.Children.Add(new Label() { Content = composition.UIName, Foreground = SystemColors.ActiveBorderBrush, HorizontalAlignment = HorizontalAlignment.Left });
            grid.Children.Add(new Label() { Content = composition.ObjName, Foreground = SystemColors.ActiveCaptionTextBrush, HorizontalAlignment = HorizontalAlignment.Center });
            
            var button = new Button();
            button.Height = 28;
            button.Content = grid;

            //TODO
            CreateComposition editor_func;
            StructureDicts.EditorDict.TryGetValue(composition.GetType(),out editor_func);
            ICompositionShapitor editor = editor_func();
            button.Command = command;
            button.CommandParameter = composition;
        }
    }
}
