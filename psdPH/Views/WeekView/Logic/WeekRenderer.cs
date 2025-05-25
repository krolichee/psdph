using Photoshop;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Views.WeekView.Logic
{
    class WeekRenderer
    {
        public static void renderWeek(WeekData weekData, Document doc)
        {
            WeekConfig weekConfig = weekData.WeekConfig;

            var preparedBlob = weekData.Prepare();
            preparedBlob.Apply(doc);
        }
    }
}
