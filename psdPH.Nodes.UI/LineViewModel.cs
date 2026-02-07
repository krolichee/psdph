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

        NodeLetLink link;

        public LineViewModel(NodeLetLink link)
        {
            this.link = link;
            DeleteLinkCommand = new RelayCommand(DeleteLink);
        }
        public RelayCommand DeleteLinkCommand;

        void DeleteLink(object _)
        {
            NodeCanvasDispatcherGlobal.Instance.DeleteLink(link);
        }
    }
}
