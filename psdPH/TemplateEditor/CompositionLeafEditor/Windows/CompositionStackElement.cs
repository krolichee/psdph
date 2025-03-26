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
        public CompositionStackElement(Composition composition,ICommand editCommand,ICommand deleteCommand)
        {
            var grid = new Grid();
            grid.Children.Add(new Label() { Content = composition.UIName, 
                Foreground = SystemColors.ActiveBorderBrush, HorizontalAlignment = HorizontalAlignment.Left });
            grid.Children.Add(new Label() { Content = composition.ObjName, 
                Foreground = SystemColors.ActiveCaptionTextBrush, HorizontalAlignment = HorizontalAlignment.Center });
            
            Height = 28;
            Content = grid;
            Command = editCommand;
            (ContextMenu = new ContextMenu()).Items.Add(new MenuItem() { Command = deleteCommand, 
                CommandParameter = composition, Header = "Удалить"});
        }
    }
}
