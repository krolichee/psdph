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
    /// Логика взаимодействия для LayerChoiceWindow.xaml
    /// </summary>
    public partial class LayerChoiceWindow : Window
    {
        protected string _result = "";
        public LayerChoiceWindow(string[] items)
        {
           
            InitializeComponent();
            foreach (var item in items)
                comboBox.Items.Add(item);
            comboBox.SelectedIndex = 0;
        }

        public string getResultString()
        {
            return _result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _result = comboBox.Text;
            Close();
        }
    }
}
