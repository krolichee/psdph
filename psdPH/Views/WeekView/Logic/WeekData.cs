using Photoshop;
using psdPH.Logic.Compositions;
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
        public Blob MainBlob;
        public List<DowBlobPair> DowBlobList = new List<DowBlobPair>();
        [XmlIgnore]
        public WeekListData WeekListData;
        [XmlIgnore]
        public WeekConfig WeekConfig => WeekListData.WeekConfig;
        [XmlIgnore]
        public Dictionary<DayOfWeek, DayBlob> DowBlobsDict
        {
            get => DowBlobList.ToDictionary(p => p.Dow, p => p.DayBlob);
            set
            {
                var result = new List<DowBlobPair>();
                foreach (var item in value)
                    result.Add(new DowBlobPair(item.Key, item.Value));
                DowBlobList = result;
            }
        }
        public void Apply(Document doc)
        {

        }
        public void Restore(WeekListData weekListData)
        {
            this.WeekListData = weekListData;
            MainBlob.Restore();
            foreach (var item in DowBlobList)
                item.DayBlob.Restore();
        }
        public WeekData Clone()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WeekData));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, this);
            StringReader sr = new StringReader(sb.ToString());
            WeekData result = serializer.Deserialize(sr) as WeekData;
            result.Restore(WeekListData);
            return result;
        }

        

        internal Blob Prepare()
        {
            DowLayernamePair whereLayernameIs(string layername, List<DowLayernamePair> pairs)
            {
                return pairs.First(dl_p => dl_p.Layername == layername);
            }
            DayOfWeek getMatchingDow(PlaceholderLeaf p)
            {
                var pairs = WeekConfig.DowPlaceholderLayernameList;
                return whereLayernameIs(p.LayerName, pairs).Dow;
            }
            WeekData weekData_clone = Clone();
            PlaceholderLeaf[] placeholders = weekData_clone.MainBlob.getChildren<PlaceholderLeaf>();

            Dictionary<DayOfWeek, PlaceholderLeaf> dowPlaceholderDict = placeholders.ToDictionary(getMatchingDow, p => p);

            var dayOfWeekEnumValues = Enum.GetValues(typeof(DayOfWeek)).Cast<Enum>();

            foreach (var item in weekData_clone.DowBlobList)
            {
                var ph = dowPlaceholderDict[item.Dow];
                DayBlob dayBlob = item.DayBlob;
                WeekConfig.FillDateAndDow(dayBlob);
                ph.Replacement = dayBlob;
            }
            WeekConfig.IncludeRules(weekData_clone);
            weekData_clone.MainBlob.CoreApply();
            return weekData_clone.MainBlob;
        }

        public WeekData(int week, WeekListData weekListData)
        {
            this.WeekListData = weekListData;
            Week = week;
            var mainBlobPrototype = weekListData.RootBlob;
            var mainBlob = mainBlobPrototype.Clone();
            MainBlob = mainBlob;
            this.Restore(weekListData);

            
            WeekConfig.GetWeekDatesTextLeaf(mainBlob).Text = WeekDatesStrings.getWeekDatesString(week);
            PrototypeLeaf prototype = WeekConfig.GetDayPrototype(mainBlob);
            foreach (var item in WeekConfig.DowPrototypeLayernameDict)
            {
                var dayBlob = DayBlob.FromBlob(prototype.Blob);
                dayBlob.Week = week;
                dayBlob.Dow = item.Key;
                DowBlobList.Add(new DowBlobPair(item.Key, dayBlob));
            }
        }
        public WeekData() { }
    }
    [Serializable]
    public class DowBlobPair
    {
        public DayOfWeek Dow;
        public DayBlob DayBlob;

        public DowBlobPair() { }

        public DowBlobPair(DayOfWeek dow, DayBlob blob)
        {
            Dow = dow;
            DayBlob = blob;
        }
    }
}
