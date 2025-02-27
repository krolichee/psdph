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
        PrototypeLeaf _result;
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
            string[] layers_names = PhotoshopDocumentExtension.GetLayersNames(
                doc.GetLayersByKinds(config.Kinds));
            stackPanel.Children.Add(ln_sc_w = new StringChoiceControl(layers_names, "Выберите слой прототипа"));


            string[] rel_layers_names = PhotoshopDocumentExtension.GetLayersNames(
                doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psNormalLayer, PsLayerKind.psSolidFillLayer }));
            stackPanel.Children.Add(rln_sc_w = new StringChoiceControl(rel_layers_names, "Выберите опорный слой"));
            _result = config.Composition as PrototypeLeaf;
        }

        public Composition getResultComposition()
        {
            return _result;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string rel_layer_name = rln_sc_w.getResultString();
            string layer_name = ln_sc_w.getResultString();
                if (_result == null)
                    _result = new PrototypeLeaf(layer_name, rel_layer_name);
                else
                {
                    _result.LayerName = layer_name;
                    _result.RelativeLayerName = rel_layer_name;
                }
            _result = new PrototypeLeaf(layer_name, rel_layer_name);
            Close();
        }
    }
}
