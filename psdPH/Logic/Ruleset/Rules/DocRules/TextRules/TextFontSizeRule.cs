using Photoshop;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public class TextFontSizeRule : TextRule, DocRule
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

}
