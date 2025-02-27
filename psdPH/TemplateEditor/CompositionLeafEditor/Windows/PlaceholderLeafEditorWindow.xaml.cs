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
    /// Логика взаимодействия для PlaceholderLeafEditorWindow.xaml
    /// </summary>
    public partial class PlaceholderLeafEditorWindow : Window, ICompositionEditor
    {
        PlaceholderLeaf _result;
        public PlaceholderLeafEditorWindow(Document doc,CompositionEditorConfig config, Composition root)
        {
            InitializeComponent();
            PrototypeLeaf[] prototypes = root.getChildren().Where(
                c=>c.GetType()==typeof(PrototypeLeaf)).Cast<PrototypeLeaf>().ToArray();
            if (prototypes.Length == 0)
            {
                MessageBox.Show("В документе нет прототипов!", "Ошибка");
                //this.Visibility = Visibility.Hidden;
                this.Activated += (object _, EventArgs __)=>  { Close(); };
                return;
            }
            string[] prototype_names = prototypes.Select(p=>p.LayerName).ToArray();
            string[] layer_names = doc.GetLayersNames(LayerListing.Recursive);
            foreach (var item in prototype_names)
                prototypeCB.Items.Add(item);
            foreach (var item in layer_names)
                layerCB.Items.Add(item);
        }

        public Composition getResultComposition()
        {
            return _result;
        }
    }
}
