using System;
using System.Linq;
using System.Windows.Controls;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    /// <summary>
    /// Логика взаимодействия для StringChoiceControl.xaml
    /// </summary>
    public partial class StringChoiceControl : UserControl
    {
        protected string _result = "";
        public StringChoiceControl(string[] items, string annotation, int default_index = 0)
        {
            if (!items.Any())
                throw new ArgumentException();
            InitializeComponent();
            foreach (var item in items)
                comboBox.Items.Add(item);
            comboBox.SelectedIndex = default_index;
            annotationLabel.Content = annotation;
            _result = items[default_index];
        }

        public string getResultString()
        {
            return _result;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _result = comboBox.SelectedValue as string;
        }
    }
}
