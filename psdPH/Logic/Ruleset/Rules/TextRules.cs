using Photoshop;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    [XmlInclude(typeof(TextAnchorRule))]
    [XmlInclude(typeof(TextFontSizeRule))]
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
            set => TextLeafLayerName = value?.LayerName;
        }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&TextLeafLayerName!=null;
        }
    };
    [Serializable]
    [XmlRoot("TextFontSizeRule")]
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
    [Serializable]
    [XmlRoot("TextAnchorRule")]
    public class TextAnchorRule : TextRule
    {
        public PsJustification Justification;
        public TextAnchorRule(Composition composition) : base(composition) { }
        public TextAnchorRule() : base(null) { }
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
}
