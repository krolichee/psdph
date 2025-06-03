using Photoshop;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace psdPH.Views.WeekView.Logic
{
    class WeekRenderer
    {
        public static void RenderWeek(WeekData weekData, Document doc)
        {
            var preparedBlob = weekData.Prepare();
            preparedBlob.Apply(doc);
        }
        public static void RenderWeek(WeekData weekData)
        {
            var doc = PhotoshopWrapper.OpenDocument(PsdPhDirectories.ProjectPsd(PsdPhProject.Instance().ProjectName));
            RenderWeek(weekData, doc);
        }
    }
}
