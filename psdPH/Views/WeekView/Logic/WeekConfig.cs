using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH
{
    [Serializable]
    public class DowLayernamePair
    {
        public DayOfWeek Dow;
        public string Layername;

        public DowLayernamePair() { }

        public DowLayernamePair(DayOfWeek dow, string layername)
        {
            Dow = dow;
            Layername = layername;
        }
    }
    [Serializable]
    public class WeekConfig
    {
        [XmlIgnore]
        public Dictionary<DayOfWeek, string> DowPrototypeLayernameDict
        {
            get => DowPrototypeLayernameList.ToDictionary(p => p.Dow, p => p.Layername); set
            {
                var result = new List<DowLayernamePair>();
                foreach (var item in value)
                    result.Add(new DowLayernamePair(item.Key, item.Value));
                DowPrototypeLayernameList = result;
            }
        }
        public List<DowLayernamePair> DowPrototypeLayernameList = new List<DowLayernamePair>();
        public string TilePreviewTextLeafName;
        public string DowLabelTextLeafLayerName;
        public string PrototypeLayerName;
        //public void Restore(Prototype prototype)
        //{
        //    Prototype = prototype;
        //    MainBlob = prototype.Parent as Blob;
        //}
        //public void Restore(Blob blob)
        //{
        //    MainBlob = blob;
        //    Prototype = MainBlob.getChildren<Prototype>().First(p => p.LayerName == PrototypeLayerName);
        //}
        //[XmlIgnore]
        //public Prototype Prototype;
        //[XmlIgnore]
        //public Blob MainBlob;
        //public WeekDowsConfig()
        //{

        //}
        //public Parameter[] Parameters
        //{
        //    get
        //    {
        //        Prototype[] prototypes = MainBlob.getChildren<Prototype>();
        //        var result = new List<Parameter>();
        //        var textLeafConfig = new ParameterConfig(this, nameof(this.MainBlob), "Прототип дня");
        //        result.Add(Parameter.Choose(textLeafConfig, prototypes));
        //        return result.ToArray();
        //    }
        //}


    }
}
