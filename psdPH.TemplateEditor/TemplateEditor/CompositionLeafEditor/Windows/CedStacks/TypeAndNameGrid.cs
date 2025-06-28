using System.Windows;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public static class TypeAndNameGrid
    {
        public static Grid Get(string type, string name)
        {
            var grid = new Grid();
            grid.Children.Add(new Label()
            {
                Content = type,
                Foreground = SystemColors.ActiveBorderBrush,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            grid.Children.Add(new Label()
            {
                Content = name,
                Foreground = SystemColors.ActiveCaptionTextBrush,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            return grid;
        }
    }
}

