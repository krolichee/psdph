using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes.UI
{
    public class LetBarViewModel
    {
        readonly Node Node;
        public Let Let => LetView.Let;
        public LetView LetView { get; set; }
        Lazy<NodeLet> nodeLet;
        NodeLet createNodeLet() => new NodeLet(Node, Let);
        public NodeLet NodeLet => nodeLet.Value;
        public bool IsChainable() { throw new NotImplementedException(); }
        public RelayCommand DropLinkOnCommand;

        public LetBarViewModel(LetView letView)
        {
            LetView = letView;
            DropLinkOnCommand = new RelayCommand(DropLinkOn);
            nodeLet = new Lazy<NodeLet>(createNodeLet);
        }
        void DropLinkOn(object _)
        {
            NodeCanvasDispatcherGlobal.Instance.PullLinkTo(NodeLet);
            
        }
        [Obsolete]
        public LetBarViewModel() { }
    }

}
