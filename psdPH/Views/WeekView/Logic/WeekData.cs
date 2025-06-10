using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Utils;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace psdPH.Views.WeekView
{
    [Serializable]
    public class WeekData
    {
        public int Week;
        [XmlIgnore]
        public Blob MainBlob => WeekListData.MainBlob;
        public ParameterSet ParameterSet = new ParameterSet();
        public List<DayParameterSet> DayParsetsList = new List<DayParameterSet>();

        [XmlIgnore]
        public WeekListData WeekListData;
        [XmlIgnore]
        public WeekConfig WeekConfig => WeekListData.WeekConfig;
        [XmlIgnore]
        public Dictionary<DayOfWeek, DayParameterSet> DowParsetDict
        {
            get => DayParsetsList.ToDictionary(p => p.Dow, p => p);
        }
        public void Apply(Document doc)
        {

        }
        public void Restore(WeekListData weekListData)
        {
            this.WeekListData = weekListData;
            var savedParameters = ParameterSet;
            ParameterSet = MainBlob.ParameterSet.Clone();
            ParameterSet.Import(savedParameters);

            Blob dayBlob = WeekConfig.GetDayBlob(MainBlob);
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
        internal Blob Prepare()
        {
            //Объявления функций
            DowLayernamePair whereLayernameIs(string layername, List<DowLayernamePair> pairs)
            {
                return pairs.First(dl_p => dl_p.Layername == layername);
            }
            DayOfWeek getMatchingDow(PlaceholderLeaf p)
            {
                var pairs = WeekConfig.DowPlaceholderLayernameList;
                return whereLayernameIs(p.LayerName, pairs).Dow;
            }
            Dictionary<DayOfWeek, PlaceholderLeaf> getBlobDowPlaceholderDict(Blob blob)
                => getDowPlaceholderDict(blob.GetChildren<PlaceholderLeaf>());
            Dictionary<DayOfWeek, PlaceholderLeaf> getDowPlaceholderDict(PlaceholderLeaf[] placeholders)
                => placeholders.ToDictionary(getMatchingDow, p => p);

            //Присваивание заглушкам заменителей
            WeekData clone = Clone();
            var mainBlob = MainBlob.Clone();
            mainBlob.ParameterSet = ParameterSet.Clone();
            Blob dayBlob = WeekConfig.GetDayBlob(mainBlob);

            Dictionary<DayOfWeek, PlaceholderLeaf> dowPlaceholderDict = getBlobDowPlaceholderDict(mainBlob); 

            foreach (DayParameterSet dowParset in clone.DayParsetsList)
            {
                var ph = dowPlaceholderDict[dowParset.Dow];
                var dayBlob_clone = dayBlob.Clone();
                dayBlob_clone.ParameterSet = dowParset;
                ph.Replacement = dayBlob_clone;
            }
            return mainBlob;
        }
        void applyRules()
        {
            foreach (ParameterSetRule rule in WeekListData.WeekRulesets.WeekRules.Rules)
                rule.SetParameterSet(ParameterSet);
            WeekListData.WeekRulesets.WeekRules.CoreApply();

            foreach (var dayParset in DayParsetsList)
            {
                foreach (ParameterSetRule rule in WeekListData.WeekRulesets.DayRules.Rules)
                    rule.SetParameterSet(dayParset);
                WeekListData.WeekRulesets.DayRules.CoreApply();
            }


        }

        void Initialize()
        {
            ParameterSet = WeekListData.MainBlob.ParameterSet.Clone();

            WeekConfig.FillWeekDate(ParameterSet,Week);
            Blob dayBlob = WeekConfig.GetDayBlob(MainBlob);
            foreach (DowLayernamePair t in WeekConfig.DowPlaceholderLayernameList)
            {
                var dayParset = DayParameterSet.FromParset(dayBlob.ParameterSet, t.Dow, Week);
                WeekConfig.FillDateAndDow(dayParset);
                DayParsetsList.Add(dayParset);
            }
            applyRules();
        }
        public WeekData(int week, WeekListData weekListData)
        {
            WeekListData = weekListData;
            Week = week;            
            this.Restore(weekListData);
            Initialize();
        }
        public WeekData() { }
    }
}
