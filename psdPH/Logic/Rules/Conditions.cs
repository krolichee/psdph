﻿using psdPH.Logic.Compositions;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    [XmlInclude(typeof(TextCondition))]
    [XmlInclude(typeof(MaxRowCountCondition))]
    [XmlInclude(typeof(MaxRowLenCondition))]
    [XmlInclude(typeof(FlagCondition))]
    public abstract class Condition : ISetupable
    {
        [XmlIgnore]
        public Composition Composition;
        [XmlIgnore]
        public abstract Parameter[] Setups { get; }
        public abstract bool IsValid();

        public void restoreComposition(Composition composition)
        {
            Composition = composition;
        }

        public Condition(Composition composition)
        {
            Composition = composition;
        }
    }
    public class DummyCondition : Condition
    {
        public DummyCondition(Composition composition) : base(composition) { }

        public override Parameter[] Setups => new Parameter[0];

        public override bool IsValid() => true;
    }
    public abstract class TextCondition : Condition
    {
        public string TextLeafLayerName;

        protected TextCondition(Composition composition) : base(composition) { }

        [XmlIgnore]
        public TextLeaf TextLeaf
        {
            get => Composition.getChildren<TextLeaf>().First(t => t.LayerName == TextLeafLayerName); 
            set => TextLeafLayerName = value.LayerName;
        }
        public override Parameter[] Setups
        {
            get
            {
                var result = new List<Parameter>();
                var textLeafConfig = new ParameterConfig(this, nameof(this.TextLeaf), "поля");
                TextLeaf[] textLeaves = Composition.getChildren<TextLeaf>();
                result.Add(Parameter.Choose(textLeafConfig, textLeaves));
                return result.ToArray();
            }
        }

    }

    public class MaxRowCountCondition : TextCondition
    {
        public override string ToString() => "количество строк";
        [XmlIgnore]
        public override Parameter[] Setups
        {
            get
            {
                List<Parameter> result = base.Setups.ToList();
                var RowCountConfig = new ParameterConfig(this, nameof(this.RowCount), "превышает");
                result.Add(Parameter.IntInput(RowCountConfig));
                return result.ToArray();
            }
        }

        public int RowCount;

        public MaxRowCountCondition(Composition composition) : base(composition) { }
        public MaxRowCountCondition() : base(null) { }

        public override bool IsValid()
        {
            return TextLeaf.Text.Split(@"\r".ToCharArray()).Length > RowCount;
        }
    }

    public class MaxRowLenCondition : TextCondition
    {
        public override string ToString() => "максимальная длина строки\n среди строк";
        [XmlIgnore]
        public override Parameter[] Setups
        {
            get
            {
                List<Parameter> result = base.Setups.ToList();
                var RowLenghtConfig = new ParameterConfig(this, nameof(this.RowLength), "превышает");
                result.Add(Parameter.IntInput(RowLenghtConfig));
                return result.ToArray();
            }
        }

        public int RowLength;

        public MaxRowLenCondition(Composition composition) : base(composition) { }
        public MaxRowLenCondition() : base(null) { }

        public override bool IsValid()
        {
            return TextLeaf.Text.Split(@"\r".ToCharArray()).Select(s => s.Length).Max() > RowLength;
        }
    }
    public class FlagCondition : Condition
    {
        public override string ToString() => "значение флага";
        public string FlagName;
        public bool Value=true;
        public override Parameter[] Setups
        {
            get
            {
                List<Parameter> result = new List<Parameter>();
                FlagLeaf[] flagLeaves = Composition.getChildren<FlagLeaf>();
                var flagConfig = new ParameterConfig(this, nameof(this.FlagLeaf), "");
                var valueConfig = new ParameterConfig(this, nameof(this.Value),"");
                result.Add(Parameter.Choose(flagConfig, flagLeaves));
                result.Add(Parameter.Check(valueConfig));
                return result.ToArray();
            }
        }

        [XmlIgnore]
        public FlagLeaf FlagLeaf
        {
            get
            {
                return Composition.getChildren<FlagLeaf>().First(t => t.Name == FlagName);
            }
            set
            {
                FlagName = value.Name;
            }
        }
        public override bool IsValid()
        {
            return FlagLeaf.Toggle== Value;
        }
        public FlagCondition(Composition composition) : base(composition) { }
        public FlagCondition() : base(null) { }
    }
}
