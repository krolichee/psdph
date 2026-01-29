using psdPH.Localization;
using psdPH.Logic.Parameters;
using psdPH.Nodes.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Nodes
{
    [Localizator]
    public static class NodesLocalizator
    {
        public static void RegisterLocalizations()
        {
            ObjectLocalization.RegisterLocalization(
                 new Dictionary<Type, string>
        {
                { typeof(MuxNode),"Выбор" },
                { typeof(ForkNode),"Ветвление" }
        });
            TypeLocalization.RegisterLocalization(
                 new Dictionary<Type, string>
        {
                { typeof(MuxNode),"Выбор" },
                { typeof(ForkNode),"Ветвление" }
        });

        }
    }
}
