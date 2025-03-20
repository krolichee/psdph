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
        public Dictionary<DayOfWeek, Blob> DayBlobsDict
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
        public void Restore()
        {
            MainBlob.Restore();
            foreach (var item in DowBlobList)
                item.Blob.Restore(MainBlob);
        }
        public WeekData Clone()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WeekData));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, this);
            StringReader sr = new StringReader(sb.ToString());
            WeekData result = serializer.Deserialize(sr) as WeekData;
            result.Restore();
            return result;
        }
        string getWeekDatesString(int week)
        {
            DateTime monday = WeekTime.GetDateByWeekAndDay(week,DayOfWeek.Monday);
            DateTime sunday = WeekTime.GetDateByWeekAndDay(week,DayOfWeek.Sunday);
            return monday.ToString("dd.MM") + " - " + sunday.ToString("dd.MM");

        }
        void fillDateAndDow(Blob blob, DateTime dateTime, WeekConfig weekConfig)
        {
            weekConfig.GetDateTextLeaf(blob).Text = dateTime.ToString("dd").ToLower();
            weekConfig.GetDowTextLeaf(blob).Text = dateTime.ToString("ddd").ToLower();
            
        }
        public WeekData(int week, WeekConfig weekConfig, Blob mainBlobPrototype) {
            Week = week;
            MainBlob = mainBlobPrototype.Clone();
            weekConfig.GetWeekDatesTextLeaf(MainBlob).Text = getWeekDatesString(week);
            PrototypeLeaf prototype = MainBlob.getChildren<PrototypeLeaf>().First(p => p.LayerName == weekConfig.PrototypeLayerName);
            foreach (var item in weekConfig.DowPrototypeLayernameDict)
            {
                var dayBlob = prototype.Blob.Clone();
                fillDateAndDow(dayBlob, WeekTime.GetDateByWeekAndDay(week, item.Key), weekConfig);
                DowBlobList.Add(new DowBlobPair(item.Key, dayBlob));
            }
        }
        public WeekData()
        {
        }
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
