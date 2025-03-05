using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
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
using System.Windows.Shapes;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для DowPlaceholderMatchWindow.xaml
    /// </summary>
    public partial class DowPlaceholderMatchWindow : Window
    {
        Dictionary<DayOfWeek, string> dowLayerDictionary = new Dictionary<DayOfWeek, string>();
        public DowPlaceholderMatchWindow(Prototype prot)
        {
            InitializeComponent();
            Blob root = prot.Parent as Blob;
            var placeholders = root.getChildren<PlaceholderLeaf>()
            .Where(cmp => cmp.PrototypeLayerName == prot.LayerName);
            var phNames = placeholders.Select(p => p.LayerName).ToArray();
            var days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Skip(1).Append(DayOfWeek.Sunday);
            int i = 0;
            foreach (var day in days)
                stackPanel.Children.Add(new StringChoiceControl(phNames, $"{day} заполнитель", i++) { Tag = day });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (StringChoiceControl scc in stackPanel.Children)
                dowLayerDictionary.Add((DayOfWeek)scc.Tag, scc.getResultString());
            Close();
        }
        public Dictionary<DayOfWeek, string> GetResultDict()
        {
            return dowLayerDictionary;
        }
    }
}
