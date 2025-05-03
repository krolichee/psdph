using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace psdPH.Utils.CedStack
{
    /// <summary>
    /// Логика взаимодействия для CEDStack.xaml
    /// </summary>
    public partial class CEDStackUI : UserControl
    {
        public CEDStackHandler handler;
        public StackPanel StackPanel => stackPanel;
        public Button AddButton => addButton;
        public static CEDStackUI CreateCEDStack(CEDStackHandler handler)
        {
            CEDStackUI result = new CEDStackUI(handler);
            handler.Initialize(result);
            return result;
        }
        protected CEDStackUI(CEDStackHandler handler)
        {
            this.handler = handler;
            InitializeComponent();
        }

        public CEDStackUI()
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addButton.ContextMenu.IsOpen = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            handler.Refresh();
        }
    }
}
