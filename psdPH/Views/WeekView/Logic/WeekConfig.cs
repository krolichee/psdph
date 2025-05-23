using psdPH.Logic.Compositions;
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
    public abstract class DayDateFormat
    {
        DateTime _sampleDateTime => new DateTime(1970, 1, 9);
        public abstract string Format { get; }
        public override string ToString()
        {
            return _sampleDateTime.ToString(Format);
        }
    }
    public class NoZeroDateFormat : DayDateFormat
    {
        public override string Format => "%d";
    }
    public class WithZeroDateFormat : DayDateFormat
    {
        public override string Format => "dd";
    }
    public class FullDowFormat : DayDateFormat
    {
        public override string Format => "dddd";
    }
    public class ShortDowFormat : DayDateFormat
    {
        public override string Format => "ddd";
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
        public DayDateFormat DayDateFormat;
        internal DayDateFormat DayDowFormat;

        public string TilePreviewTextLeafName;
        public string PrototypeLayerName;
        public string WeekDatesTextLeafName;
        public string DowTextLeafLayerName;
        public string DateTextLeafLayerName;
        internal TextLeaf GetWeekDatesTextLeaf(Blob blob)
        {
            return blob.getChildren<TextLeaf>().First(_ => _.LayerName == WeekDatesTextLeafName);
        }
        internal TextLeaf GetDateTextLeaf(Blob blob)
        {
            return blob.getChildren<TextLeaf>().First(_ => _.LayerName == DateTextLeafLayerName);
        }
        internal TextLeaf GetDowTextLeaf(Blob blob)
        {
            return blob.getChildren<TextLeaf>().First(_ => _.LayerName == DowTextLeafLayerName);
        }
        internal PrototypeLeaf GetDayPrototype(Blob blob)
        {
            return blob.getChildren<PrototypeLeaf>().First(p => p.LayerName == PrototypeLayerName);
        }
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
