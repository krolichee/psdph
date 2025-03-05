using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public WeekData(int week, WeekConfig weekDowsConfig, Blob mainBlobPrototype) {
            Week = week;
            MainBlob = mainBlobPrototype.Clone();
            Prototype prototype = MainBlob.getChildren<Prototype>().First(p => p.LayerName == weekDowsConfig.PrototypeLayerName);
            foreach (var item in weekDowsConfig.DowPrototypeLayernameDict)
                DowBlobList.Add(new DowBlobPair(item.Key, prototype.Blob.Clone()));
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

        public DowBlobPair()
        {
        }

        public DowBlobPair(DayOfWeek dow, Blob blob)
        {
            Dow = dow;
            Blob = blob;
        }
    }
}
