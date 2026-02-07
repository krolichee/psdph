using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes.UI
{
    public class NodeCanvasDispatcherGlobal
    {
        static NodeCanvasDispatcher instance;

        public static NodeCanvasDispatcher Instance { get => instance; set => instance = value; }
    }
}
