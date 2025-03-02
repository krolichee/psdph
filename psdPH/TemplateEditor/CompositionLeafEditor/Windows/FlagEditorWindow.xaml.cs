using Photoshop;
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
    /// Логика взаимодействия для FlagEditorWindow.xaml
    /// </summary>
    public partial class FlagEditorWindow : Window, ICompositionEditor
    {
        FlagLeaf _result;
        EditorMode _mode;

        public FlagEditorWindow()
        {

            InitializeComponent();
        }
        
        public FlagEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            if (config.Composition == null)
                _mode = EditorMode.Create;
            else
            {
                _mode = EditorMode.Edit;
                flagNameTB.Text = (config.Composition as FlagLeaf).Name;
                _result = config.Composition as FlagLeaf;
            }
        }

        public Composition GetResultComposition()
        {
            return _result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_mode == EditorMode.Create)
                _result = new FlagLeaf(flagNameTB.Text);
            else
                _result.Name = flagNameTB.Text;
            Close();
        }
    }
}
