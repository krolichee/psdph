using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Utils;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace psdPH.Views.WeekView
{
    [Serializable]
    public class WeekData:ViewData
    {
        public int Week;
        public List<DayParameterSet> DayParsetsList = new List<DayParameterSet>();
        public ParameterSet ParameterSet = new ParameterSet();
        [XmlIgnore]
        public WeekListData WeekListData;
        [XmlIgnore]
        public WeekConfig WeekConfig => WeekListData.WeekConfig;
        [XmlIgnore]
        public Dictionary<DayOfWeek, DayParameterSet> DowParsetDict
        {
            get => DayParsetsList.ToDictionary(p => p.Dow, p => p);
        }

        public override RootBlob RootBlob => WeekListData.RootBlob;

        public void Restore(WeekListData weekListData)
        {
            this.WeekListData = weekListData;

            var blobParameterSet = RootBlob.ParameterSet.Clone();
            blobParameterSet.Import(ParameterSet);
            ParameterSet = blobParameterSet;

            Composition dayBlob = WeekConfig.GetDayPrototype(RootBlob);
            for (int i = 0; i < DayParsetsList.Count; i++)
            {
                var savedParset = DayParsetsList[i];
                var dayParset = DayParameterSet.FromParset(dayBlob.ParameterSet, savedParset.Dow, savedParset.Week);
                dayParset.Import(savedParset);
                DayParsetsList[i] = dayParset;
            }

        }
        public WeekData Clone()
        {
            WeekData result = CloneConverter.Clone(this) as WeekData;
            result.Restore(WeekListData);
            return result;
        }
        internal RootBlob Prepare()
        {
            throw new NotImplementedException();
            ////Объявления функций
            //DowGuidPair whereLayernameIs(string layername, List<DowGuidPair> pairs)
            //{
            //    return pairs.First(dl_p => dl_p.Guid == layername);
            //}
            //DayOfWeek getMatchingDow(PlaceholderLeaf p)
            //{
            //    var pairs = WeekConfig.DowPlaceholderLayernameList;
            //    return whereLayernameIs(p.LayerName, pairs).Dow;
            //}
            //Dictionary<DayOfWeek, PlaceholderLeaf> getBlobDowPlaceholderDict(RootBlob blob)
            //    => getDowPlaceholderDict(blob.GetChildren<PlaceholderLeaf>());
            //Dictionary<DayOfWeek, PlaceholderLeaf> getDowPlaceholderDict(PlaceholderLeaf[] placeholders)
            //    => placeholders.ToDictionary(getMatchingDow, p => p);

            ////Присваивание заглушкам заменителей
            //WeekData clone = Clone();
            //var mainBlob = RootBlob.Clone();
            //mainBlob.ParameterSet = ParameterSet.Clone();
            //RootBlob dayBlob = WeekConfig.GetDayBlob(mainBlob);

            //Dictionary<DayOfWeek, PlaceholderLeaf> dowPlaceholderDict = getBlobDowPlaceholderDict(mainBlob); 

            //foreach (DayParameterSet dowParset in clone.DayParsetsList)
            //{
            //    var ph = dowPlaceholderDict[dowParset.Dow];
            //    var dayBlob_clone = dayBlob.Clone();
            //    dayBlob_clone.ParameterSet = dowParset;
            //    ph.Replacement = dayBlob_clone;
            //}
            //return mainBlob;
        }
        void applyRules(RuleSet ruleSet, ParameterSet parameterSet)
        {
            throw new NotImplementedException();
            //foreach (ParameterSetRule rule in ruleSet.Rules)
            //{
            //    rule.SetParameterSet(parameterSet);
            //    rule.Composition = null;
            //}
            //ruleSet.Apply<ParameterSetRule>(null);
        }
        public void ApplyRules()
        {
            var dayRules = WeekListData.WeekRulesets.DayRules;
            foreach (var dayParset in DayParsetsList)
                applyRules(dayRules, dayParset);


            var weekRules = WeekListData.WeekRulesets.WeekRules;
            applyRules(weekRules, ParameterSet);
        }
        public void FillDates()
        {
            WeekConfig.FillWeekDate(ParameterSet, Week);
            foreach (var parset in DayParsetsList)
                WeekConfig.FillDateAndDow(parset);
        }
        void initialize()
        {
            ParameterSet = WeekListData.RootBlob.ParameterSet.Clone();
            Composition dayBlob = WeekConfig.GetDayPrototype(RootBlob);
            foreach (DowGuidPair t in WeekConfig.DowPlaceholderLayernameList)
            {
                var dayParset = DayParameterSet.FromParset(dayBlob.ParameterSet, t.Dow, Week);
                DayParsetsList.Add(dayParset);
            }
            FillDates();
            ApplyRules();
        }
        public WeekData(int week, WeekListData weekListData)
        {
            WeekListData = weekListData;
            Week = week;            
            this.Restore(weekListData);
            initialize();
        }
        public WeekData() { }
    }
}
