using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Utils;
using psdPH.Utils.Setups;
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
        Dictionary<DayOfWeek, Guid> dowLayerDictionary = new Dictionary<DayOfWeek, Guid>();
        SetupsInputWindow si_w;
        public DowPlaceholderMatchWindow(PrototypeBlob prot)
        {
            RootBlob root = prot.Parent as RootBlob;
            var placeholders = prot.Placeholders;
            var days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Skip(1).Append(DayOfWeek.Sunday);
            int i = 0;
            var setups = new List<Setup>();
            foreach (var day in days)
            {
                var kvPair = dowLayerDictionary.First(kv => kv.Key == day);
                var config = new SetupConfig(kvPair, nameof(kvPair.Value), Localization.Localize(day));
                setups.Add(new ChooseSetup(config, placeholders));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
            //DialogResult = true;
            //foreach (StringChoiceControl scc in stackPanel.Children)
            //    dowLayerDictionary.Add((DayOfWeek)scc.Tag, scc.getResultString());
            //Close();
        }
        public Dictionary<DayOfWeek, Guid> GetResultDict()
        {
            return dowLayerDictionary;
        }
        private DowPlaceholderMatchWindow() { }

    }
}
