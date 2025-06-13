using Photoshop;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public class TranslateRule : LayerRule, DocRule
    {
        public override string ToString() => "положение";
        public Point Shift;
        public int X { get => (int)Shift.X; set { Shift.X = (double)value; } }
        public int Y { get => (int)Shift.Y; set { Shift.Y = (double)value; } }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                var modeConfig = new SetupConfig(this, nameof(this.ChangeMode), "");
                var xConfig = new SetupConfig(this, nameof(this.X), "x");
                var yConfig = new SetupConfig(this, nameof(this.Y), "y");
                result.Add(getLayerParameter());
                result.Add(Setup.EnumChoose(modeConfig, typeof(ChangeMode)));
                result.Add(Setup.IntInput(xConfig));
                result.Add(Setup.IntInput(yConfig));
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
