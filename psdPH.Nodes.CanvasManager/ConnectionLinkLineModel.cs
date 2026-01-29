using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace psdPH.Nodes.CanvasManager
{
    public class ConnectionLinkLineModel
    {

        static Dictionary<Line, ConnectionLinkLineModel> models = new Dictionary<Line, ConnectionLinkLineModel>();
        public static ConnectionLinkLineModel Get(Line line) => models[line];
        public static void Register(Line line, Action deleteAction, LinkLineEffect linkLineEffect)
        {
            var cllm = new ConnectionLinkLineModel(line,deleteAction,linkLineEffect);
            models.Add(line, cllm);
        }
        Line Line;
        LinkLineEffect defaultEffect;
        ICommand DeleteCommand { get; }
        public void Delete()
        {
            DeleteCommand.Execute(null);
            Clear(Line);
        }
        public static void Clear(Line line) => models.Remove(line);
        public bool Selected
        {
            set
            {
                ConnectionLineDrawer.Paint(Line, value? LinkLineEffect.Selected:defaultEffect);
            }
        }
        ConnectionLinkLineModel(Line line, Action deleteAction, LinkLineEffect linkLineEffect)
        {
            
            defaultEffect = linkLineEffect;
            Line = line;
            ConnectionLineDrawer.Paint(Line, defaultEffect);
            
            DeleteCommand = new RelayCommand((_) => deleteAction());
        }
    }
}
