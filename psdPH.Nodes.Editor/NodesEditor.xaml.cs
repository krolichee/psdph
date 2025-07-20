using psdPH.Logic.Compositions;
using psdPH.Nodes.CanvasManager;
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
        public NodesEditor(Composition blob)
        {
            InitializeComponent();
            var canvas = NodeCanvasManager.MakeInstance(blob).Canvas;
            PanManager.Attach(canvas, canvasViewer);
            canvasBorder.Child= canvas;
            
        }
    }
}
