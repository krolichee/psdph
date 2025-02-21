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
    /// Логика взаимодействия для LayerChoiceWindow.xaml
    /// </summary>
    public partial class StringChoiceWindow : Window,IStringEditor
    {
        protected string _result = "";
        protected StringChoiceControl scc;
        public StringChoiceWindow(string[] items,string annotation)
        {
            InitializeComponent();
            scc = new StringChoiceControl(items, annotation);
            stackPanel.Children.Insert(0,scc);
        }

        public string getResultString()
        {
            return _result;
        }

        public bool Select(string variant)
        {
           return scc.Select(variant);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _result = scc.getResultString();
            Close();
        }
    }
}
