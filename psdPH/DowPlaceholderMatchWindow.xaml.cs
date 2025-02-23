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
        public DowPlaceholderMatchWindow(PrototypeLeaf prot)
        {
            Blob root = prot.Parent as Blob;
            InitializeComponent();            
            List<PlaceholderLeaf> placeholders = new List<PlaceholderLeaf>();
            foreach (PlaceholderLeaf cmp in root.getChildren<PlaceholderLeaf>())
                if (cmp.PrototypeLayerName == prot.LayerName)
                    placeholders.Add(cmp as PlaceholderLeaf);
            List<string> ph_names = new List<string>();
            foreach (var item in placeholders)
                ph_names.Add(item.LayerName);
            List<DayOfWeek> days = (Enum.GetValues(typeof(DayOfWeek)) as DayOfWeek[]).ToList();
            foreach (DayOfWeek day in days)
                stackPanel.Children.Add(new StringChoiceControl(ph_names.ToArray(), $"{WeekGaleryConfig.RuNames[day]} заполнитель") { Tag = day});
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (StringChoiceControl scc in stackPanel.Children)
                dowLayerDictionary.Add((DayOfWeek)scc.Tag, scc.getResultString());
            Close();
        }
        public Dictionary<DayOfWeek, string> getResultDict()
        {
            return dowLayerDictionary;
        }
    }
}
