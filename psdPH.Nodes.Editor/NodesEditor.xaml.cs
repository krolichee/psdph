using psdPH.Localization;
using psdPH.Logic.Compositions;
using psdPH.Nodes.CanvasManager;
using psdPH.Nodes.Nodes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace psdPH.Nodes.Editor
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class NodesEditor : UserControl
    {
        Composition[] Children;
        Dictionary<string, Action> ObjectBundle
        {
            get
            {
                var result = new Dictionary<string, Action>();
                foreach (var item in Children)
                {
                    var caption = LocalizationService.Localize((item as object).GetType());
                    Action creationCommand = () => NodeCanvasManager.Instance().AddNode(new ObjectNode(item));
                    result.Add(caption, creationCommand);
                }
                return result;
            }
        }
        
        public NodesEditor(Composition blob)
        {
            InitializeComponent();
            var canvas = NodeCanvasManager.MakeInstance(blob).Canvas;
            PanManager.Attach(canvas, canvasViewer);
            canvasBorder.Child= canvas;
            
        }
    }
}
