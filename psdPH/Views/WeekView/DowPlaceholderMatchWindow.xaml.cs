using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
            Owner = TopmostWindow.Get();
            WindowStartupLocation =WindowStartupLocation.CenterOwner;
            InitializeComponent();
            Closed += Window_Closed;
            Blob root = prot.Parent as Blob;
            var placeholders = root.getChildren<PlaceholderLeaf>()
            .Where(cmp => cmp.PrototypeLayerName == prot.LayerName);
            var phNames = placeholders.Select(p => p.LayerName).ToArray();
            var days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Skip(1).Append(DayOfWeek.Sunday);
            int i = 0;
            foreach (var day in days)
                stackPanel.Children.Add(new StringChoiceControl(phNames, $"{day} заполнитель", i++) { Tag = day });
        }
        private void Window_Closed(object sender, EventArgs e)
        {
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
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
