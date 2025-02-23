using Photoshop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{
    
    public class RuleSet
    {

        [XmlIgnore]
        public Composition composition;
        public List<Rule> Rules;

        public void apply(Document doc)
        {
            foreach (var item in Rules)
            {
                item.apply(doc);
            }
        }
    }
    public abstract class Rule : IParameterable
    {
        [XmlIgnore]
        public RuleSet ruleSet;
        abstract public void apply(Document doc);

        public abstract Parameter[] Parameters { get; }
    }
    

    public abstract class ConditionRule : Rule
    {
        Condition Condition;
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
    };
    public class TranslateRule : ChangingRule
    {
        public Point Shift;
        public int X { get => (int)Shift.X; set { Shift.X = (double)value; } }
        public int Y { get => (int)Shift.Y; set { Shift.Y = (double)value; } }

        public override Parameter[] Parameters
        {
            get
            {
                var result = new List<Parameter>();
                var xConfig = new ParameterConfig(this, nameof(this.X), "x");
                var yConfig = new ParameterConfig(this, nameof(this.Y), "y");
                var modeConfig = new ParameterConfig(this, nameof(this.Mode), "y");
                result.Add(Parameter.IntInput(xConfig));
                result.Add(Parameter.IntInput(yConfig));
                result.Add(Parameter.EnumChoose(modeConfig, typeof(ChangeMode)));
                return result.ToArray();
            }
        }

        protected override void _apply(Document doc)
        {

            (doc.ActiveLayer as ArtLayer).Translate(Shift.X, Shift.Y);
        }
    }
    public abstract class TextRule : ChangingRule
    {
        [XmlIgnore]
        public Composition Composition;
        public string TextLeafLayerName;
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
        public TextRule(Composition composition)
        {
            Composition = composition;
        }
    };
    public class TextFontSizeRule : TextRule
    {

        public int FontSize;

        public TextFontSizeRule(Composition composition) : base(composition)
        {
        }

        public override Parameter[] Parameters
        {
            get
            {
                var result = new List<Parameter>();
                var fontSizeConfig = new ParameterConfig(this, nameof(this.FontSize), "размер шрифта");
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
        public class RuleParameter
        {
            public string Description;
            public Control Control;
            public void Apply(TextRule tr)
            {

            }
        }
    };
    public class TextAnchorRule : TextRule
    {
        
        public PsJustification Justification;

        public TextAnchorRule(Composition composition) : base(composition)
        {
        }

        public override Parameter[] Parameters
        {
            get
            {
                var result = new List<Parameter>();
                var justificationConfig = new ParameterConfig(this, nameof(this.Justification), "x");
                result.Add(Parameter.EnumChoose(justificationConfig, typeof(PsJustification)));
                return result.ToArray();
            }
        }

        protected override void _apply(Document doc)
        {
            (doc.ActiveLayer as ArtLayer).TextItem.Justification = Justification;
        }
    }
    //------------------
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }


}
