using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes.UI
{
    class LineViewModel
    {

        NodeLet from;
        NodeLet to;

        public LineViewModel(NodeLet from, NodeLet to)
        {
            this.from = from;
            this.to = to;
            DeleteLinkCommand = new RelayCommand(DeleteLink);
        }
        public RelayCommand DeleteLinkCommand;

        void DeleteLink(object _)
        {
            NodeCanvasDispatcher.Instance.DeleteLink(from,to);
        }
    }
}
