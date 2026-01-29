using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace psdPH.CED
{
    abstract public class CEDElementControl<TElementType> : Button
    {
        abstract public ICommand DeleteCommand();
        abstract public ICommand EditCommand();
        protected void setContextMenu(FrameworkElement control, TElementType @object)
        {
            control.ContextMenu = new ContextMenu();
            control.ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Удалить",
                Command = DeleteCommand(),
                CommandParameter = @object
            }
                );
        }

    }
}
