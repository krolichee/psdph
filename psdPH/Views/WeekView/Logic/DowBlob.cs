using psdPH.Logic.Compositions;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace psdPH.Views.WeekView.Logic
{
   public class WeekBlob:Blob{
        public int Week = 0;
        public static WeekBlob FromBlob(Blob blob, int week)
        {
            var result = CloneConverter.Convert<WeekBlob>(blob);
            result.Restore();
            result.Week = week;
            return result;
        }
    }
   public class DowBlob:WeekBlob
    {
        public DayOfWeek Dow=DayOfWeek.Monday;
        public static DowBlob FromBlob(Blob blob,int week,DayOfWeek dow)
        {
            var result = CloneConverter.Convert<DowBlob>(blob);
            result.Restore();
            result.Dow = dow;
            result.Week = week;
            return result;
        }
    }
}
