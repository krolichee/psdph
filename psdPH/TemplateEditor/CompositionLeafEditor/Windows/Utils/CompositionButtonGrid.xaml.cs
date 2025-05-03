using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    /// <summary>
    /// Логика взаимодействия для CompositionButtonGrid.xaml
    /// </summary>
    public partial class CompositionButtonGrid : UserControl
    {
        public CompositionButtonGrid(string type, string name)
        {

            InitializeComponent();
            typeLabel.Content = type;
            nameLabel.Content = name;
        }
    }
}
