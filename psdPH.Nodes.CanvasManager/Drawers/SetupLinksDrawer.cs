using psdPH.Nodes.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Nodes.CanvasManager
{
    class SetupLinksDrawer
    {
        Canvas Canvas;

        public SetupLinksDrawer(Canvas canvas)
        {
            Canvas = canvas;
        }

        class SetupBarsLinks
        {
            public SetupBar From;
            public SetupBar To;

            public SetupBarsLinks(SetupBar from, SetupBar to)
            {
                From = from;
                To = to;
            }
        }
        public void DrawSetupLinks(List<NodeUI> nodeUIs)
        {
            List<SetupBar> setupBars = new List<SetupBar>();
            foreach (var item in nodeUIs.Select(nui => nui.SetupBars))
            {
                setupBars.AddRange(item);
            }
            List<SetupBarsLinks> setupBarsLinks = getSetupBarsLinks(setupBars.ToArray());
            foreach (var item in setupBarsLinks)
            {

                var fromBar = item.From;
                var toBar = item.To;

                var line = LinkLine.Create(fromBar, toBar, Canvas);

                Canvas.Children.Add(line);
            }
        }
        private List<SetupBarsLinks> getSetupBarsLinks(SetupBar[] setupBars)
        {
            var result = new List<SetupBarsLinks>();
            for (int i = 0; i < setupBars.Length; i++)
            {
                SetupBar fromBar = setupBars[i];
                var fromSetupOutputLinks = fromBar.NodeSetup.Node.Links.Where(ol => ol.FromNodeSetup.Equals(fromBar.NodeSetup));

                var toNodeSetups = fromSetupOutputLinks.Select(ol => ol.ToNodeSetup).ToArray();

                for (int j = 0; j < toNodeSetups.Length; j++)
                {
                    var toNodeSetup = toNodeSetups[j];
                    SetupBar toBar = setupBars.First(s => s.NodeSetup.Equals(toNodeSetup));
                    result.Add(new SetupBarsLinks(fromBar, toBar));
                }
            }
            return result;
        }
    }
}
