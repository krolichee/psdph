using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Views.WeekView.Logic
{
    public abstract class DateFormat:psdPH.ISerializable
    {
        public DateFormat Upper => new Upper(this);
        public DateFormat Lower => new Lower(this);
        public DateFormat FirstUpper => new FirstUpper(this);
        DateTime _sampleDateTime => new DateTime(1970, 1, 9);
        public override string ToString()
        {
            return Format(_sampleDateTime);
        }
        public abstract string Format(DateTime dt);

    }
    public abstract class AffectFormat : DateFormat
    {
        public DateFormat Include;
        protected virtual string affect(string s) => s;
        public override string Format(DateTime dt) =>
            affect(Include.Format(dt));
        protected AffectFormat(DateFormat dateFormat)
        {
            Include = dateFormat;
        }
        public AffectFormat() : this(null) { }
    }
    public class Upper : AffectFormat
    {
        public Upper(DateFormat dateFormat) : base(dateFormat) { }
        protected override string affect(string s) => s.ToUpper();
        public Upper() :base(null){}
    }
    public class Lower : AffectFormat
    {
        public Lower(DateFormat dateFormat) : base(dateFormat) { }
        protected override string affect(string s) => s.ToLower();
        public Lower() : base(null) { }
    }
    public class FirstUpper : AffectFormat
    {
        public FirstUpper(DateFormat dateFormat) : base(dateFormat) { }
        protected override string affect(string s)
        {
            var result = s.ToLower();
            result = result.Remove(0, 1);
            var firstLetter = s[0].ToString().ToUpper();
            result = result.Insert(0, firstLetter);
            return result;
        }
        public FirstUpper() : base(null) { }
    }
    public abstract class DayFormat : DateFormat { }
    public abstract class DowFormat : DateFormat { }
    public class NoZeroDateFormat : DayFormat
    {
        public override string Format(DateTime dt) => dt.ToString("%d");
        public NoZeroDateFormat() {}
    }
    public class WithZeroDateFormat : DayFormat
    {
        public override string Format(DateTime dt) => dt.ToString("dd");
        public WithZeroDateFormat() { }
    }
    public class FullDowFormat : DowFormat
    {
        public override string Format(DateTime dt) => dt.ToString("dddd");
        public FullDowFormat() { }
    }
    public class ShortDowFormat : DowFormat
    {
        public override string Format(DateTime dt) => dt.ToString("ddd");
        public ShortDowFormat() { }
    }
}
