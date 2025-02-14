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
    public interface ICompositionEditorWindowFactory
    {
        CompositionEditorWindow CreateCompositionEditorWindow(List<PSDLayer> layers);
    };
    public class TextLeafEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public CompositionEditorWindow CreateCompositionEditorWindow(List<PSDLayer> layers)
        {
            return new TextLeafEditorWindow(layers);
        }
    }

    public abstract class CompositionLeafEditorConfig
    {
        public Composition Composition;
        static public PsLayerKind[] Kinds;
        public ICompositionEditorWindowFactory Factory;
    }
    public class TextLeafEditorConfig : CompositionLeafEditorConfig
    {
        public TextLeafEditorConfig(TextLeaf textLeaf)
        {
            Factory = new TextLeafEditorWindowFactory();
        }
    }

    public class TextLeafEditorWindow: CompositionEditorWindow
    {

        private Composition _composition;
        public Composition getResult()
        {
            return _composition;
        }
        public TextLeafEditorWindow(List<PSDLayer> layers)
        {
            InitializeComponent();
            List<string> layer_names = new List<string>();
            foreach (var layer in layers)
                foreach (var kind in TextLeafEditorConfig.Kinds)
                    if (layer.kind == kind)
                        layer_names.Add(layer.name);
            foreach (var name in layer_names)
                comboBox.Items.Add(name);
        }
        public TextLeafEditorWindow(List<PSDLayer> layers, CompositionLeafEditorConfig config, Composition composition)
        {
            _composition = composition;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _composition = new TextLeaf(textBox.Text, comboBox.Text);
            this.Close();
        }
    }
}
