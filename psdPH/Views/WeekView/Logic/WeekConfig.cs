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
    public abstract class DateFormat
    {
        
        public DateFormat Upper =>new Upper(this);
        public DateFormat Lower => new Lower(this);
        public DateFormat FirstUpper => new FirstUpper(this);
        DateTime _sampleDateTime => new DateTime(1970, 1, 9);
        public override string ToString()
        {
            return Format(_sampleDateTime);
        }
        
        public abstract string Format(DateTime dt);
        
    }
    class AffectFormat: DateFormat
    {
        public DateFormat _include;
        protected virtual string affect(string s) => s;
        public override string Format(DateTime dt) =>
            affect(_include.ToString());
        protected AffectFormat(DateFormat dateFormat)
        {
            _include = dateFormat;
        }
    }
    class Upper : AffectFormat
    {
        public Upper(DateFormat dateFormat) : base(dateFormat) { }

        protected override string affect(string s)=>s.ToUpper();
    }
    class Lower : AffectFormat
    {
        public Lower(DateFormat dateFormat) : base(dateFormat) { }
        protected override string affect(string s) => s.ToLower();
    }
    class FirstUpper : AffectFormat
    {
        public FirstUpper(DateFormat dateFormat) : base(dateFormat) { }
        protected override string affect(string s) {
            var result = s.ToLower();
            result=result.Remove(0, 1);
            var firstLetter = s[0].ToString().ToUpper();
            result=result.Insert(0, firstLetter);
            return result;
        }
    }
    public abstract class DayFormat : DateFormat{ }
    public abstract class DowFormat : DateFormat{ }
    public class NoZeroDateFormat : DayFormat
    {
        public override string Format(DateTime dt) => dt.ToString("%d");
    }
    public class WithZeroDateFormat : DayFormat
    {
        public override string Format(DateTime dt) => dt.ToString("dd");
    }
    public class FullDowFormat : DowFormat
    {
        public override string Format(DateTime dt) => dt.ToString("dddd");
    }
    public class ShortDowFormat : DowFormat
    {
        public override string Format(DateTime dt) => dt.ToString("ddd");
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
        public DateFormat DayDateFormat;
        internal DateFormat DayDowFormat;

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
