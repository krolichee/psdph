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
using PsApp = Photoshop.Application;
using PsWr = psdPH.PhotoshopWrapper;
using PsDocWr = psdPH.Logic.PhotoshopDocumentWrapper;


namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для EditCompositionWindow.xaml
    /// </summary>

    public partial class TextLeafEditorWindow : Window, ICompositionEditor
    {

        private Composition _composition;
        public Composition getResult()
        {
            return _composition;
        }
        public TextLeafEditorWindow(Document doc, TextLeafEditorConfig config)
        {
            InitializeComponent();
            _composition = config.Composition;
            ArtLayer[] layers = new PsDocWr(doc).GetLayersByKinds(config.Kinds);
            string[] layer_names = PsDocWr.GetLayersNames(layers);
            foreach (string name in layer_names)
                comboBox.Items.Add(name);
            if (config.Composition!=null)
                comboBox.SelectedItem = (config.Composition as TextLeaf).LayerName;
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
