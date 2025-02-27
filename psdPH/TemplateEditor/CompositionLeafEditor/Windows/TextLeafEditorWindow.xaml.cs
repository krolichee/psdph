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
using PsDocWr = psdPH.Logic.PhotoshopDocumentExtension;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Logic;


namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для EditCompositionWindow.xaml
    /// </summary>

    public partial class TextLeafEditorWindow : Window, ICompositionEditor
    {
        private EditorMode _mode = EditorMode.Create;
        protected StringChoiceControl scc;

        private TextLeaf _composition;
        public Composition getResult()
        {
            return _composition;
        }
        public TextLeafEditorWindow(Document doc, TextLeafEditorConfig config)
        {
            InitializeComponent();
            _composition = config.Composition as TextLeaf;
            ArtLayer[] layers = doc.GetLayersByKinds(config.Kinds);
            string[] layer_names = doc.GetLayersNames(layers);
            scc = new StringChoiceControl(layer_names, "Выбор слоя");
            stackPanel.Children.Insert(0, scc);
            if (config.Composition != null)
            {
                _mode = EditorMode.Edit;
                scc.Select((config.Composition as TextLeaf).LayerName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_mode == EditorMode.Create)
                _composition = new TextLeaf(scc.getResultString());
            else
            {
                _composition.LayerName = scc.getResultString();
            }
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
