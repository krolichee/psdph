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
using Photoshop;
using psdPH.TemplateEditor;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для EditCompositionWindow.xaml
    /// </summary>

    public partial class TextLeafEditorWindow : Window, ICompositionGenerator
    {

        private Composition _composition;
        public Composition getResult()
        {
            return _composition;
        }
        public TextLeafEditorWindow(PhotoshopWrapper psd, TextLeafEditorConfig config)
        {
            InitializeComponent();
            _composition = config.Composition;
            var layers = psd.GetLayers().ToArray();
            layers = PSDLayer.FilterByKinds(layers, config.Kinds);
            string[] layer_names = layers.Select(l=>l.name).ToArray();
            foreach (var name in layer_names)
                comboBox.Items.Add(name);
            comboBox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _composition = new TextLeaf(textBox.Text, comboBox.Text);
            Close();
        }


        public new bool? ShowDialog()
        {
            return base.ShowDialog();
        }

        public Composition getResultComposition()
        {
            return _composition;
        }
    }
}
