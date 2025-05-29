using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Views.WeekView.Logic
{
    public abstract class DateFormat
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
    class AffectFormat : DateFormat
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

        protected override string affect(string s) => s.ToUpper();
    }
    class Lower : AffectFormat
    {
        public Lower(DateFormat dateFormat) : base(dateFormat) { }
        protected override string affect(string s) => s.ToLower();
    }
    class FirstUpper : AffectFormat
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
    }
    public abstract class DayFormat : DateFormat { }
    public abstract class DowFormat : DateFormat { }
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
}
