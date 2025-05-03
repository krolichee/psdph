using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public Dictionary<DayOfWeek, Blob> DowBlobsDict
        {
            get => DowBlobList.ToDictionary(p => p.Dow, p => p.Blob);
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
                item.Blob.Restore();
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

        void fillDateAndDow(Blob blob, DateTime dateTime)
        {
            var dateTextLeaf = WeekConfig.GetDateTextLeaf(blob);
            var dowTextLeaf = WeekConfig.GetDowTextLeaf(blob);
            dateTextLeaf.Text = dateTime.ToString("dd").ToLower();
            dowTextLeaf.Text = dateTime.ToString("ddd").ToLower();

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
                var dayBlob = prototype.Blob.Clone();
                fillDateAndDow(dayBlob, WeekTime.GetDateByWeekAndDay(week, item.Key));
                DowBlobList.Add(new DowBlobPair(item.Key, dayBlob));
            }
        }
        public WeekData() { }
    }
    [Serializable]
    public class DowBlobPair
    {
        public DayOfWeek Dow;
        public Blob Blob;

        public DowBlobPair() { }

        public DowBlobPair(DayOfWeek dow, Blob blob)
        {
            Dow = dow;
            Blob = blob;
        }
    }
}
