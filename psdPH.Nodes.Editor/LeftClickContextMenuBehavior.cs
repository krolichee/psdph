using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace psdPH.Nodes.Editor
{
    public class LeftClickContextMenuBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += OnLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseLeftButtonDown -= OnLeftButtonDown;
            base.OnDetaching();
        }

        private void OnLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AssociatedObject.ContextMenu == null)
                return;

            AssociatedObject.ContextMenu.PlacementTarget = AssociatedObject;
            AssociatedObject.ContextMenu.IsOpen = true;
            e.Handled = true; // Чтобы не срабатывал повторный клик
        }
    }
}
