using Photoshop;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Serialization;

namespace psdPH.Logic
{
    public class TranslateRule : LayerRule
    {
        public override string ToString() => "положение";
        public Point Shift;
        public int X { get => (int)Shift.X; set { Shift.X = (double)value; } }
        public int Y { get => (int)Shift.Y; set { Shift.Y = (double)value; } }
        [XmlIgnore]
        public override Parameter[] Setups
        {
            get
            {
                var result = new List<Parameter>();
                var modeConfig = new ParameterConfig(this, nameof(this.ChangeMode), "");
                var xConfig = new ParameterConfig(this, nameof(this.X), "x");
                var yConfig = new ParameterConfig(this, nameof(this.Y), "y");
                result.Add(getLayerParameter());
                result.Add(Parameter.EnumChoose(modeConfig, typeof(ChangeMode)));
                result.Add(Parameter.IntInput(xConfig));
                result.Add(Parameter.IntInput(yConfig));
                return result.ToArray();
            }
        }
        protected override void _apply(Document doc)
        {
            var layer = getRuledLayerWr(doc);
            Vector shift;
            if (ChangeMode == ChangeMode.Rel)
                shift = new Vector(Shift.X, Shift.Y);
            else
                shift = new Vector(Shift.X - layer.Bounds[0], Shift.Y - layer.Bounds[1]);
            layer.TranslateV(shift);
        }
        public TranslateRule() : base(null) { }
        public TranslateRule(Composition composition) : base(composition) { }
    }

}
