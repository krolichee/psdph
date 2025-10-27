using psdPH.Logic;
using psdPH.Setups;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.Utils.Setups
{
    public class QuestionableSetups
    {
        //TODO Инкапсулировать работу со списком! Какой ужас
        public static List<Setup> Setups = new List<Setup>();
        public static void Ask()
        {
            if (Setups.Count != 0)
            {
                MessageBox.Show("Некоторые настройки требуют уточнения", "Подтверждение",
                MessageBoxButton.OK, MessageBoxImage.Information,
                MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                var si_w = new SetupsInputWindow(Setups.ToArray());
                si_w.Topmost=true;
                si_w.ShowDialog();
            }
            Setups.Clear();
        }
    }
}
