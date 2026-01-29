using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System.Windows;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для LayerChoiceWindow.xaml
    /// </summary>
    public partial class StringChoiceWindow : Window, IStringEditor
    {
        protected string _result = "";
        protected StringChoiceControl scc;
        public StringChoiceWindow(string[] items, string annotation)
        {
            InitializeComponent();
            scc = new StringChoiceControl(items, annotation);
            stackPanel.Children.Insert(0, scc);
        }

        public string GetResultString()
        {
            return _result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _result = scc.getResultString();
            Close();
        }


    }
}
