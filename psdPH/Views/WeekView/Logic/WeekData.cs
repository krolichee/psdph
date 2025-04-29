using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace psdPH
{
    [Serializable]
    public class WeekData
    {
        public int Week;
        public Blob MainBlob;
        public List<DowBlobPair> DowBlobList=new List<DowBlobPair>();
        [XmlIgnore]
        WeekListData weekListData;
        [XmlIgnore]
        public Blob RootBlob => weekListData.RootBlob;
        [XmlIgnore]
        public WeekConfig WeekConfig => weekListData.WeekConfig;
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
            this.weekListData = weekListData;
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
            result.Restore(weekListData);
            return result;
        }
        public string getWeekDatesString(int week)
        {
            string result="";
            DateTime monday = WeekTime.GetDateByWeekAndDay(week,DayOfWeek.Monday);
            DateTime sunday = WeekTime.GetDateByWeekAndDay(week,DayOfWeek.Sunday);
            if (monday.Month != sunday.Month)
                result = monday.ToString("dd MMMM") + " - " + sunday.ToString("dd MMMM");
            else
                result = monday.ToString("dd") + " - " + sunday.ToString("dd MMMM");
            return result;
        }
        void fillDateAndDow(Blob blob, DateTime dateTime)
        {
            var dateTextLeaf = WeekConfig.GetDateTextLeaf(blob);
            var dowTextLeaf = WeekConfig.GetDowTextLeaf(blob);
            dateTextLeaf.Text = dateTime.ToString("dd").ToLower();
            dowTextLeaf.Text = dateTime.ToString("ddd").ToLower();
            
        }
        public WeekData(int week,WeekListData weekListData) {
            this.weekListData = weekListData;
            Week = week;
            var mainBlobPrototype = weekListData.RootBlob;
            Restore(weekListData);
            
            var mainBlob = mainBlobPrototype.Clone();
            WeekConfig.GetWeekDatesTextLeaf(mainBlob).Text = getWeekDatesString(week);
            PrototypeLeaf prototype = WeekConfig.GetDayPrototype(mainBlob);
            foreach (var item in WeekConfig.DowPrototypeLayernameDict)
            {
                var dayBlob = prototype.Blob.Clone();
                fillDateAndDow(dayBlob, WeekTime.GetDateByWeekAndDay(week, item.Key));
                DowBlobList.Add(new DowBlobPair(item.Key, dayBlob));
            }
        }
        public WeekData(){}
    }
    [Serializable]
    public class DowBlobPair
    {
        public DayOfWeek Dow;
        public Blob Blob;

        public DowBlobPair(){ }

        public DowBlobPair(DayOfWeek dow, Blob blob)
        {
            Dow = dow;
            Blob = blob;
        }
    }
}
