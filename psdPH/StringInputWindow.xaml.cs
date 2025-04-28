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
    /// Логика взаимодействия для StringInputWindow.xaml
    /// </summary>
    public partial class StringInputWindow : Window, IStringEditor
    {
        public StringInputWindow(string annotation)
        {
            InitializeComponent();
            Title = annotation;
            tb.Focus();
        }
        public string getResultString()
        {
            return tb.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tb.Text != "")
                DialogResult = true;
            Close();
        }
    }
}
