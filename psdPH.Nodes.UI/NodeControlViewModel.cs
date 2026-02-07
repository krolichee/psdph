using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes.UI
{
    public class NodeControlViewModel
    {
        readonly Node node;

        public NodeControlViewModel(Node node)
        {
            this.node = node;
        }

        public void DeleteNode(object _ = null)
        {
            NodeCanvasDispatcherGlobal.Instance.DeleteNode(node);
        }
    }
}
