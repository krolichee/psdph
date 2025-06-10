using psdPH.Logic;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Views.WeekView.Logic
{
    public class WeekRulesets
    {
        public RuleSet DayRules = new RuleSet();
        public RuleSet WeekRules = new RuleSet();
        public void Restore(Blob blob, WeekConfig weekConfig)
        {
            WeekRules.RestoreComposition(blob);
            DayRules.RestoreComposition(weekConfig.GetDayBlob(blob));
        }
        //public void InjectDayRules(DowBlob dayBlob)
        //{
        //    foreach (var item in DayRules.Rules)
        //        dayBlob.RuleSet.AddRule(item.Clone());
        //}
        //internal void InjectWeekRules(WeekData weekData)
        //{
        //    foreach (var item in WeekRules.Rules)
        //        weekData.MainBlob.RuleSet.AddRule(item.Clone());
        //}
        //internal void InjectRules(WeekData weekData)
        //{
        //    foreach (var item in weekData.DowBlobList)
        //        InjectDayRules(item);
        //    InjectWeekRules(weekData);
        //}
    }
}
