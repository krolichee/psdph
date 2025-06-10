using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Views;
using psdPH.Views.WeekView;
using psdPH.Views.WeekView.Logic;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для DayTile.xaml
    /// </summary>
    public partial class DayTile : UserControl
    {
        ParameterSet Parset;
        public DayOfWeek Dow
        {
            get => _dow; set
            {
                _dow = value;
                dowLabel.Content = Localization.LocalizeObj(Dow);
            }
        }
        private DayOfWeek _dow;

        string getParametersText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var par in Parset.AsCollection())
            {
                sb.Append($"{par.Name}: {Localization.LocalizeObj(par.Value)}\n");
            }
            return sb.ToString();
        }
        void refreshPreview()
        {
            previewTextBlock.Text = getParametersText();
        }
        public DayTile(DayParameterSet parset)
        {
            Parset = parset;
            InitializeComponent();
            this.Dow = parset.Dow;
            refreshPreview();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            new ParsetInputWindow(Parset).ShowDialog();
            refreshPreview();
        }
    }
}
