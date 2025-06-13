using System.Windows;
using System.Windows.Controls;

namespace psdPH.Utils.CedStack
{
    /// <summary>
    /// Логика взаимодействия для CEDStack.xaml
    /// </summary>
    public partial class CEDStackUI : UserControl
    {
        public CEDPanelHandler handler;
        public Panel Panel => stackPanel;
        public Button AddButton => addButton;
        public static CEDStackUI CreateCEDStack(CEDPanelHandler handler)
        {
            CEDStackUI result = new CEDStackUI(handler);
            handler.Initialize(result);
            return result;
        }
        protected CEDStackUI(CEDPanelHandler handler)
        {
            this.handler = handler;
            InitializeComponent();
        }

        public CEDStackUI()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (addButton.ContextMenu.Items.Count!=0)
                addButton.ContextMenu.IsOpen = true;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            handler?.Refresh();
        }
    }
}
