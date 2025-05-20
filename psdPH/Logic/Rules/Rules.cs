using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Logic.Rules;
using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{

    [XmlInclude(typeof(ConditionRule))]

    [XmlInclude(typeof(LayerRule))]

    [XmlInclude(typeof(TextRule))]
    public abstract class Rule : IParameterable
    {
        [XmlIgnore]
        public Composition Composition;
        public Rule(Composition composition)
        {
            Composition = composition;
        }
        [XmlIgnore]
        public RuleSet ruleSet;
        abstract public void Apply(Document doc);

        public virtual void restoreComposition(Composition composition)
        {
            Composition = composition;
        }

        [XmlIgnore]
        public abstract Parameter[] Parameters { get; }
    }

    [XmlInclude(typeof(Condition))]
    public abstract class ConditionRule : Rule
    {
        public Condition Condition;
        protected ConditionRule(Composition composition) : base(composition)
        {
            Condition = new DummyCondition(Composition);
        }
        public override void restoreComposition(Composition composition)
        {
            base.restoreComposition(composition);
            Condition.restoreComposition(composition);
        }

        abstract protected void _apply(Document doc);
        virtual protected void _else(Document doc) { }
        public override void Apply(Document doc)
        {
            if (Condition.IsValid())
                _apply(doc);
            else
                _else(doc);
        }
    };

    [XmlInclude(typeof(TranslateRule))]
    [XmlInclude(typeof(OpacityRule))]
    [XmlInclude(typeof(VisibleRule))]
    public abstract class LayerRule : ConditionRule
    {
        public ChangeMode ChangeMode = ChangeMode.Abs;
        public string LayerName;
        protected LayerComposition layerComposition;
        [XmlIgnore]
        public LayerComposition LayerComposition
        {
            get => layerComposition; set
            {
                layerComposition = value;
                LayerName = value.LayerName;
            }
        }
        protected Parameter getLayerParameter()
        {
            var layerNameConfig = new ParameterConfig(this, nameof(this.LayerComposition), "для слоя");
            return Parameter.Choose(layerNameConfig, Composition.getChildren<LayerComposition>());
        }
        protected LayerWr getRuledLayerWr(Document doc)=>
            doc.GetLayerWrByName(LayerName);
        protected LayerRule(Composition composition) : base(composition) { }
    };
    [Serializable]
    [XmlRoot("TranslateRule")]
    public class TranslateRule : LayerRule
    {
        public override string ToString() => "положение";
        public Point Shift;
        public TranslateRule(Composition composition) : base(composition) { }

        public int X { get => (int)Shift.X; set { Shift.X = (double)value; } }
        public int Y { get => (int)Shift.Y; set { Shift.Y = (double)value; } }
        public string LeafLayerName;
        [XmlIgnore]
        public override Parameter[] Parameters
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
    }
    public class OpacityRule : LayerRule
    {
        public override string ToString() => "прозрачность";

        public int Opacity;

        public override Parameter[] Parameters
        {
            get
            {
                var result = new List<Parameter>();
                var opacityConfig = new ParameterConfig(this, nameof(this.Opacity), "установить");
                result.Add(getLayerParameter());
                result.Add(Parameter.IntInput(opacityConfig, 0, 100));
                return result.ToArray();
            }
        }
        protected override void _apply(Document doc)
        {
            dynamic layer = getRuledLayerWr(doc);
            layer.Opacity = Opacity;
        }
        public OpacityRule(Composition composition) : base(composition) { }
        public OpacityRule() : base(null) { }
    }

    public class VisibleRule : LayerRule
    {
        public override string ToString() => "видимость";
        public bool Toggle;
        public override Parameter[] Parameters
        {
            get
            {
                var result = new List<Parameter>();
                var opacityConfig = new ParameterConfig(this, nameof(this.Toggle), "установить");
                result.Add(getLayerParameter());
                result.Add(Parameter.Check(opacityConfig));
                return result.ToArray();
            }
        }
        protected override void _else(Document doc)
        {
            dynamic layer = getRuledLayerWr(doc);
            layer.Visible = !Toggle;
        }
        protected override void _apply(Document doc)
        {
            dynamic layer = getRuledLayerWr(doc);
            layer.Visible = Toggle;
        }
        public VisibleRule(Composition composition) : base(composition) { }
        public VisibleRule() : base(null) { }
    }
    public class AlignRule : LayerRule
    {
        public AlignRule(Composition composition) : base(composition) { }
        [XmlIgnore]
        public AreaLeaf AreaLeaf;
        Alignment Alignment;
        public override Parameter[] Parameters => new Parameter[0];
        protected override void _apply(Document doc)
        {
            getRuledLayerWr(doc).AlignLayer(AreaLeaf.ArtLayerWr(doc), Alignment);
        }
    }
    public class FitRule : LayerRule
    {
        public bool BalanceFont=false;
        public FitRule(Composition composition) : base(composition) { }
        [XmlIgnore]
        public AreaLeaf AreaLeaf;
        Alignment Alignment;
        public override Parameter[] Parameters
        {
            get
            {
                Document doc;
                var result = new List<Parameter>();
                result.Add(getLayerParameter());
                var balance_config = new ParameterConfig(this, nameof(BalanceFont), "балансировать шрифт");
                var layerWr = doc.GetLayerWrByName(LayerName);
                if (layerWr is ArtLayerWr)
                    if ((layerWr as ArtLayerWr).ArtLayer.Kind == PsLayerKind.psTextLayer)
                        result.Add(Parameter.Check(balance_config));
                var alingment_config = new ParameterConfig(this, nameof(Alignment), "с выравниванием");
                result.Add(Parameter.StringInput);
            }
            
            }
        protected override void _apply(Document doc)
        {
            getRuledLayerWr(doc).AlignLayer(AreaLeaf.ArtLayerWr(doc), Alignment);
        }
    }

}
