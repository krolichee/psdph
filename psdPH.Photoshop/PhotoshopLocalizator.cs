using Photoshop;
using psdPH.Localization;
using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Photoshop.LayerWr;

namespace psdPH.Photoshop
{
    [Localizator]
    public class PhotoshopLocalizator
    {
        public static void RegisterLocalizations()
        {
            EnumLocalization.RegisterLocalization(new Dictionary<VAilgnment, string>
            {
                { VAilgnment.Top, "cверху" },
                { VAilgnment.Center, "по центру" },
                { VAilgnment.Bottom, "снизу" },
                { VAilgnment.None, "не выравнивать" }
            });
            EnumLocalization.RegisterLocalization(new Dictionary<HAilgnment, string>
            {
                { HAilgnment.Left, "слева" },
                { HAilgnment.Center, "по центру" },
                { HAilgnment.Right, "справа" },
                { HAilgnment.None, "не выравнивать" }
            });
            EnumLocalization.RegisterLocalization(new Dictionary<PsJustification, string>
            {
                { PsJustification.psLeft, "слева" },
                { PsJustification.psCenter, "по центру" },
                { PsJustification.psRight, "справа" }
            });
            EnumLocalization.RegisterLocalization(new Dictionary<ConsiderFx, string>
            {
                { ConsiderFx.WithFx, "с эффектами" },
                { ConsiderFx.NoFx, "без эффектов" }
            });

        }
    }
}
