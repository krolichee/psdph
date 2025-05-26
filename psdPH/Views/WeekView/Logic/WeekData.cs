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
        public WeekBlob MainBlob;
        public List<DowBlob> DowBlobList = new List<DowBlob>();
        [XmlIgnore]
        public WeekListData WeekListData;
        [XmlIgnore]
        public WeekConfig WeekConfig => WeekListData.WeekConfig;
        [XmlIgnore]
        public Dictionary<DayOfWeek, DowBlob> DowBlobsDict
        {
            get => DowBlobList.ToDictionary(p => p.Dow, p => p);
        }
        public void Apply(Document doc)
        {

        }
        public void Restore(WeekListData weekListData)
        {
            this.WeekListData = weekListData;
            MainBlob.Restore();
            foreach (var item in DowBlobList)
                item.Restore();
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

            foreach (var dayBlob in weekData_clone.DowBlobList)
            {
                var ph = dowPlaceholderDict[dayBlob.Dow];
                WeekConfig.FillDateAndDow(dayBlob);
                ph.Replacement = dayBlob;
            }
            WeekConfig.GetWeekDatesTextLeaf(weekData_clone.MainBlob).Text = WeekConfig.GetWeekDatesString(Week);
            WeekListData.InjectRules(weekData_clone);
            weekData_clone.MainBlob.CoreApply();
            return weekData_clone.MainBlob;
        }

        void InitializeDowBlobList()
        {
            PrototypeLeaf prototype = WeekConfig.GetDayPrototype(MainBlob);
            foreach (var item in WeekConfig.DowPrototypeLayernameDict)
            {
                var dayBlob = DowBlob.FromBlob(prototype.Blob, Week, item.Key);
                DowBlobList.Add(dayBlob);
            }
        }
        public WeekData(int week, WeekListData weekListData)
        {
            this.WeekListData = weekListData;
            Week = week;
            MainBlob = WeekBlob.FromBlob(weekListData.RootBlob, week);
            this.Restore(weekListData);
            InitializeDowBlobList();
        }
        public WeekData() { }
    }
}
