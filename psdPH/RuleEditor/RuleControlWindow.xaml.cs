using psdPH.Logic;
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

namespace psdPH.RuleEditor
{
    /// <summary>
    /// Логика взаимодействия для RuleControlWindow.xaml
    /// </summary>
    public partial class RuleControlWindow : Window, IRuleEditor
    {
        ConditionRule _result;
        RuleControl _rc;
        public RuleControlWindow(Composition composition)
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
            _rc = new RuleControl(composition);
            mainGrid.Children.Add(_rc);
        }

        public ConditionRule GetResultRule()
        {
            return _result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _result = _rc.GetResultRule();
            Close();
        }
    }
}
