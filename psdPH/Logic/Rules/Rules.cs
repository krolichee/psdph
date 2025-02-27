using Photoshop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using static psdPH.Logic.Parameter;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{
    [Serializable]
    [XmlRoot("Ruleset")]
    public class RuleSet
    {

        [XmlIgnore]
        public Composition composition;
        public List<Rule> Rules = new List<Rule>()  ;

        public void apply(Document doc)
        {
            foreach (var item in Rules)
            {
                item.apply(doc);
            }
        }

        internal void restoreLinks(Composition composition)
        {
            foreach (var rule in Rules)
            {
                rule.restoreComposition(composition);
            }
        }
    }
    [XmlInclude(typeof(ConditionRule))]
    [XmlInclude(typeof(ChangingRule))]
    [XmlInclude(typeof(TextRule))]
    [XmlInclude(typeof(TextAnchorRule))]
    [XmlInclude(typeof(TextFontSizeRule))]
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
        abstract public void apply(Document doc);

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

        protected ConditionRule(Composition composition) : base(composition){}
        public override void restoreComposition(Composition composition)
        {
            base.restoreComposition(composition);
            Condition.Composition = composition;
        }

        abstract protected void _apply(Document doc);
        public override void apply(Document doc)
        {
            if (Condition.IsValid())
                _apply(doc);
        }
    };

    public enum ChangeMode
    {
        Rel,
        Abs
    }
    public abstract class ChangingRule : ConditionRule
    {
        public ChangeMode Mode;
        public string LayerName;

        protected ChangingRule(Composition composition) : base(composition) {}
    };
    [Serializable]
    [XmlRoot("TranslateRule")]
    public class TranslateRule : ChangingRule
    {
        public Point Shift;

        public TranslateRule(Composition composition) : base(composition){}

        public int X { get => (int)Shift.X; set { Shift.X = (double)value; } }
        public int Y { get => (int)Shift.Y; set { Shift.Y = (double)value; } }
        public string LeafLayerName;
        [XmlIgnore]
        public LayerComposition Leaf
        {
            get
            {
                return Composition.getChildren<TextLeaf>().Where(t => t.LayerName == LeafLayerName).ToArray()[0];
            }
            set
            {
                LeafLayerName = value.LayerName;
            }
        }
        [XmlIgnore]
        public override Parameter[] Parameters
        {
            get
            {
                var result = new List<Parameter>();
                var modeConfig = new ParameterConfig(this, nameof(this.Mode), "");
                var xConfig = new ParameterConfig(this, nameof(this.X), "x");
                var yConfig = new ParameterConfig(this, nameof(this.Y), "y");
                var layerNameConfig = new ParameterConfig(this,nameof(this.LayerName),"слоя");
                Composition.getChildren();
                result.Add(Parameter.Choose(layerNameConfig, Composition.getChildren<LayerComposition>().Select(l=>l.LayerName).ToArray()));
                result.Add(Parameter.EnumChoose(modeConfig, typeof(ChangeMode)));
                result.Add(Parameter.IntInput(xConfig));
                result.Add(Parameter.IntInput(yConfig));
                return result.ToArray();
            }
        }
        public override string ToString() =>"положение";

        protected override void _apply(Document doc)
        {
           // PsLayerKind.
            doc.GetLayerByName(LayerName);
            (doc.ActiveLayer as ArtLayer).Translate(Shift.X, Shift.Y);
        }
        public TranslateRule():base(null) { }
    }
    public abstract class TextRule : ChangingRule
    {
        
        public string TextLeafLayerName;
        [XmlIgnore]
        public override Parameter[] Parameters
        {
            get
            {
                TextLeaf[] textLeaves = Composition.getChildren<TextLeaf>();
                var result = new List<Parameter>();
                var textLeafConfig = new ParameterConfig(this, nameof(this.TextLeaf), "поля");
                result.Add(Parameter.Choose(textLeafConfig, textLeaves));
                return result.ToArray();
            }
        }

        protected TextRule(Composition composition) : base(composition){}

        [XmlIgnore]
        public TextLeaf TextLeaf
        {
            get
            {
                return Composition.getChildren<TextLeaf>().Where(t => t.LayerName == TextLeafLayerName).ToArray()[0];
            }
            set
            {
                TextLeafLayerName = value.LayerName;
            }
        }
        
    };
    [Serializable]
    [XmlRoot("TextFontSizeRule")]
    public class TextFontSizeRule : TextRule
    {

        public int FontSize;

        public TextFontSizeRule(Composition composition) : base(composition)
        {
        }
        public TextFontSizeRule() : base(null) { }
        [XmlIgnore]
        public override Parameter[] Parameters
        {
            get
            {
                List<Parameter> result = base.Parameters.ToList();
                var modeConfig = new ParameterConfig(this, nameof(this.Mode), "");
                var fontSizeConfig = new ParameterConfig(this, nameof(this.FontSize), "");
                result.Add(Parameter.EnumChoose(modeConfig,typeof(ChangeMode)));
                result.Add(Parameter.IntInput(fontSizeConfig));
                return result.ToArray();
            }
        }
        protected override void _apply(Document doc)
        {
            if (Mode == ChangeMode.Rel)
                (doc.ActiveLayer as ArtLayer).TextItem.Size += FontSize;
            else
                (doc.ActiveLayer as ArtLayer).TextItem.Size = FontSize;
        }
        public override string ToString() => "размер шрифта";
    };
    [Serializable]
    [XmlRoot("TextAnchorRule")]
    public class TextAnchorRule : TextRule
    {
        
        public PsJustification Justification;

        public TextAnchorRule(Composition composition) : base(composition){}
        public TextAnchorRule() : base(null) { }
        [XmlIgnore]
        public override Parameter[] Parameters
        {
            get
            {
                List<Parameter> result = base.Parameters.ToList();
                var justificationConfig = new ParameterConfig(this, nameof(this.Justification), "установить");
                result.Add(Parameter.Choose(justificationConfig, new PsJustification[] { 
                    PsJustification.psRight,
                    PsJustification.psLeft,
                    PsJustification.psCenter
                }.Select(o=>new EnumWrapper(o)).ToArray(),(o)=>(o as EnumWrapper).Value));
                return result.ToArray();
            }
        }
        public override string ToString() => "выравнивание шрифта";

        protected override void _apply(Document doc)
        {
            (doc.ActiveLayer as ArtLayer).TextItem.Justification = Justification;
        }
    }
    public static class UIName
    {
        public static string ToString(object value)
        {
            return value.ToString();
        }
    }
    //------------------
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            return EnumLocalization.GetLocalizedDescription(value);
        }
        public static string ToString(this Enum value)
        {
            return value.GetDescription();
        }
    }


}
