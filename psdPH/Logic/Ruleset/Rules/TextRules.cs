using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    public abstract class TextRule : LayerRule
    {

        public string TextLeafLayerName;
        protected Setup getTextLeafSetup()
        {
            TextLeaf[] textLeaves = Composition.GetChildren<TextLeaf>();
            var textLeafConfig = new SetupConfig(this, nameof(this.TextLeaf), "поля");
            return Setup.Choose(textLeafConfig, textLeaves);
        }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>() { getTextLeafSetup() };
                return result.ToArray();
            }
        }

        protected TextRule(Composition composition) : base(composition) { }

        [XmlIgnore]
        public TextLeaf TextLeaf
        {
           protected get =>Composition.GetChildren<TextLeaf>().FirstOrDefault(t => t.LayerName == TextLeafLayerName);
            set => TextLeafLayerName = LayerName = value?.LayerName;
        }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&TextLeafLayerName!=null;
        }
    };
    public class TextFontSizeRule : TextRule
    {
        protected static int notSetFontSize => 0;
        public int FontSize = notSetFontSize;
        public TextFontSizeRule(Composition composition) : base(composition) { }
        public TextFontSizeRule() : base(null) { }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = base.Setups.ToList();
                var modeConfig = new SetupConfig(this, nameof(this.ChangeMode), "");
                var fontSizeConfig = new SetupConfig(this, nameof(this.FontSize), "");
                result.Add(Setup.EnumChoose(modeConfig, typeof(ChangeMode)));
                result.Add(Setup.IntInput(fontSizeConfig));
                return result.ToArray();
            }
        }
        protected override void _apply(Document doc)
        {

            if (ChangeMode == ChangeMode.Rel)
                doc.GetLayerByName(LayerName).TextItem.Size += FontSize;
            else
                doc.GetLayerByName(LayerName).TextItem.Size = FontSize;
        }
        public override string ToString() => "размер шрифта";
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&FontSize!=notSetFontSize;
        }
    };
    public class TextJustifRule : TextRule
    {
        public PsJustification Justification;
        public TextJustifRule(Composition composition) : base(composition) { }
        public TextJustifRule() : base(null) { }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = base.Setups.ToList();
                var justificationConfig = new SetupConfig(this, nameof(this.Justification), "установить");
                result.Add(Setup.Choose(justificationConfig, new PsJustification[] {
                    PsJustification.psRight,
                    PsJustification.psLeft,
                    PsJustification.psCenter
                }.Cast<object>().ToArray(), FieldFunctions.EnumWrapperFunctions));
                return result.ToArray();
            }
        }
        public override string ToString() => "выравнивание шрифта";

        protected override void _apply(Document doc)
        {
            doc.GetLayerByName(LayerName).TextItem.Justification = Justification;
        }
    }
    public class TextLeafTextRule : TextRule
    {
        public string StringName;
        bool predicate(Parameter p) => p.Name == StringName && p is StringParameter;
        [XmlIgnore]
        public StringParameter StringParameter
        {
            protected get
            {
                return Composition.ParameterSet.AsCollection().FirstOrDefault(predicate) as StringParameter;
            }
            set
            {
                StringName = value?.Name;
            }
        }
        public TextLeafTextRule(Composition composition) : base(composition) { }
        public TextLeafTextRule() : base(null) { }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                List<Setup> result = new List<Setup>();
                Parameter[] stringParameters = Composition.ParameterSet.GetByType<StringParameter>().ToArray();
                var stringConfig = new SetupConfig(this, nameof(this.StringParameter), "из");
                result.Add(getTextLeafSetup());
                result.Add(Setup.Choose(stringConfig, stringParameters));
                return result.ToArray();
            }
        }
        public override string ToString() => "установить текст";

        protected override void _apply(Document doc)
        {
            doc.GetLayerByName(LayerName).TextItem.Contents = StringParameter.Text;
        }
        public override bool IsSetUp()
        {
            return base.IsSetUp() && StringName != null;
        }
    }

}
