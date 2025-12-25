using psdPH.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.LetViews
{
    public class LetViewsLocalizator : Localizator
    {
        public override void RegisterLocalizations()
        {
            EnumLocalization.RegisterLocalization(
                new Dictionary<AlignmentLetViewStrings, string>
                {
                    {AlignmentLetViewStrings.DefaultCaption,"Расположение" }
                }
                );
        }
    }
}
