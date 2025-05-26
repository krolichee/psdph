using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    abstract public class CEDStackControl<T> : Button
    {
        abstract public ICommand DeleteCommand();
        abstract public ICommand EditCommand();
        protected void setContextMenu(FrameworkElement control, T @object)
        {
            control.ContextMenu = new ContextMenu();
            control.ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Удалить",
                Command = this.DeleteCommand(),
                CommandParameter = @object
            }
                );
        }

    }
}
