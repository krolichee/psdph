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
    /// Логика взаимодействия для EditCompositionWindow.xaml
    /// </summary>
    public class CompositionLeafEditorConfig
        {

        }
public partial class CompositionLeafEditorWindow : Window
    {
        
        private Composition _composition;
        public Composition getResult() {
            return _composition;
        }
        public CompositionLeafEditorWindow(string[] layer_names, CompositionLeafEditorConfig config)
        {
            InitializeComponent();
            foreach (var name in layer_names)
                comboBox.Items.Add(name);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _composition = new TextLeaf(textBox.Text, comboBox.Text);
            this.Close();
        }
    }
}
