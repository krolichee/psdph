using psdPH.Logic.Compositions;
using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Nodes;
using psdPH.Photoshop;
using psdPH.Setups;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static psdPH.Utils.SplitTextToRatio;

namespace psdPH.Logic.Rules
{
    public class SplitForAreaRule : AreaRule
    {
        public string DryText;
        public string WetText;
        public override string ToString() => "разделить текст";
        public override void Apply(DocumentWr doc)
        {
            var size = AreaLeaf.LayerDescriptor.GetLayerWr(doc).GetNoFxBoundsSize();
            var ratio = size.Width / size.Height;
            WetText = Splitter.Split(DryText, ratio);
            if (WetText.Length != 0)
            {
                var textConfig = new ReflectionConfig(this, nameof(this.WetText), "текст для зоны "+LayerComposition.LayerName);
                var textSetup = new RichStringInputSetup(textConfig);
                QuestionableSetups.Setups.Add(textSetup);
            }
        }
        protected override DtoConverter DtoConverter => new SplitForAreaRuleDtoConverter();

        public SplitForAreaRule(Composition composition) : base(composition) { }
        public SplitForAreaRule():base(null) {
            SetupsRegistry.Register<SplitForAreaRule>(new SplitTextForAreaRuleSetupSource());
        }

    }
    public class SplitForAreaRuleDto : LayerCompositionDto
    {
        public string DryText;
    }
    public class SplitForAreaRuleDtoConverter : LayerRuleDtoConverter
    {
        public override void GetEntity(object _obj, object _dto)
        {
            var obj = _obj as SplitForAreaRule;
            var dto = _dto as SplitForAreaRule;
            base.GetEntity(_obj, _dto);
            dto.DryText = obj.DryText;
        }

        public override object GetDto(object _obj)
        {
            var dto = new SplitForAreaRuleDto();
            UpdateDto(_obj,dto);
            throw new NotImplementedException();
        }
        protected override void UpdateDto(object _obj, object _dto)
        {
            var obj = _obj as SplitForAreaRule;
            var dto = _dto as SplitForAreaRuleDto;
            base.UpdateDto(_obj, _dto);
            dto.DryText = obj.DryText;
        }
    }
    public class SplitTextForAreaRuleSetupSource : AreaRuleSetupSource
    {
        protected Setup DryTextSetup(object obj)
        {
            var splitForAreaRule = obj as SplitForAreaRule;
            var config = new ReflectionConfig(splitForAreaRule, nameof(splitForAreaRule.DryText), "текст");
            return new StringInputSetup(config);
        }
        public override Setup[] GetSetups(object obj)
        {
            return new Setup[] { getAreaLeafSetup(obj),DryTextSetup(obj) };
        }
    }
}
