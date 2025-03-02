using Photoshop;
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

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    /// <summary>
    /// Логика взаимодействия для PrototypeEditorWindow.xaml
    /// </summary>
    public partial class PrototypeEditorWindow : Window, ICompositionEditor
    {
        Prototype _result;
        private Document doc;
        private CompositionEditorConfig config;
        private Composition root;
        StringChoiceControl ln_sc_w;
        StringChoiceControl rln_sc_w;

        public PrototypeEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            this.doc = doc;
            this.config = config;
            this.root = root;
            InitializeComponent();

            string[] blobs_names = root.getChildren<Blob>().Select(b => b.LayerName).ToArray();
            stackPanel.Children.Add(ln_sc_w = new StringChoiceControl(blobs_names, "Выберите поддокумент прототипа"));

            string[] rel_layers_names = PhotoshopDocumentExtension.GetLayersNames(
                doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psNormalLayer, PsLayerKind.psSolidFillLayer }));
            stackPanel.Children.Add(rln_sc_w = new StringChoiceControl(rel_layers_names, "Выберите опорный слой"));
            _result = config.Composition as Prototype;
        }

        public Composition GetResultComposition()
        {
            return _result;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _result = new Prototype() { Parent = root };
            string rel_layer_name = rln_sc_w.getResultString();
            string layer_name = ln_sc_w.getResultString();

            _result.LayerName = layer_name;
            _result.RelativeLayerName = rel_layer_name;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}
