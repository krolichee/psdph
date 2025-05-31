using System;
using System.Windows;
using System.Windows.Controls;


namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    [Obsolete]
    public class BlockingStack : StackPanel
    {
        public void Add(FrameworkElement control)
        {
            Children.Add(control);
            _refresh();
        }
        void _refresh()
        {
            for (int i = 0; i < Children.Count-1; i++)
            {
                (Children[i] as FrameworkElement).IsEnabled = false;
            }
            for (int i = 1; i < Children.Count; i++)
            {
                var gap = -(Children[i - 1] as Control).Height*0.95;
                (Children[i] as FrameworkElement).Margin = new Thickness(0,gap,0,0);
            }
        }
    }
}
