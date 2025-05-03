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

        protected TextRule(Composition composition) : base(composition) { }

        [XmlIgnore]
        public TextLeaf TextLeaf
        {
            get
            {
                return Composition.getChildren<TextLeaf>().First(t => t.LayerName == TextLeafLayerName);
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

        public TextFontSizeRule(Composition composition) : base(composition) { }
        public TextFontSizeRule() : base(null) { }
        [XmlIgnore]
        public override Parameter[] Parameters
        {
            get
            {
                List<Parameter> result = base.Parameters.ToList();
                var modeConfig = new ParameterConfig(this, nameof(this.ChangeMode), "");
                var fontSizeConfig = new ParameterConfig(this, nameof(this.FontSize), "");
                result.Add(Parameter.EnumChoose(modeConfig, typeof(ChangeMode)));
                result.Add(Parameter.IntInput(fontSizeConfig));
                return result.ToArray();
            }
        }
        protected override void _apply(Document doc)
        {

            if (ChangeMode == ChangeMode.Rel)
                doc.GetLayerByName(LayerName).TextItem.Size += FontSize;
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

        public TextAnchorRule(Composition composition) : base(composition) { }
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
