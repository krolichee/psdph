using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System.Windows;

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
        public string GetResultString()
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
