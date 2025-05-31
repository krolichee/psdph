using Photoshop;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    [Serializable]
    [PsdPhSerializable]
    public abstract class Condition : ISetupable,psdPH.ISerializable
    {
        [XmlIgnore]
        public Composition Composition;
        [XmlIgnore]
        public abstract Parameter[] Setups { get; }
        public abstract bool IsValid();

        public void RestoreComposition(Composition composition)
        {
            Composition = composition;
        }

        public Condition(Composition composition)
        {
            Composition = composition;
            KnownTypes.Types.Add(this.GetType());
        }
    }
    public class DummyCondition : Condition
    {
        public override string ToString() => "(безусловно)";
        public DummyCondition(Composition composition) : base(composition) { }
        public DummyCondition() : base(null) { }
        [XmlIgnore]
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
        [XmlIgnore]
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
    public class EmptyTextCondition : TextCondition
    {
        public override string ToString() => "текст пустой";
        public EmptyTextCondition(Composition composition) : base(composition) { }
        public EmptyTextCondition() : base(null) { }

        public override bool IsValid()
        {
            return TextLeaf.Text.Length==0;
        }
    }
    public class NonEmptyTextCondition : TextCondition
    {
        public override string ToString() => "текст не пустой";
        public NonEmptyTextCondition(Composition composition) : base(composition) { }
        public NonEmptyTextCondition() : base(null) { }

        public override bool IsValid()
        {
            return TextLeaf.Text.Length > 0;
        }
    }

    public class FlagCondition : Condition
    {
        public override string ToString() => "значение флага";
        public string FlagName;
        public bool Value=true;
        [XmlIgnore]
        public override Parameter[] Setups
        {
            get
            {
                List<Parameter> result = new List<Parameter>();
                FlagLeaf[] flagLeaves = Composition.getChildren<FlagLeaf>();
                var flagConfig = new ParameterConfig(this, nameof(this.FlagLeaf), "");
                var valueConfig = new ParameterConfig(this, nameof(this.Value),"установлено в");
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
