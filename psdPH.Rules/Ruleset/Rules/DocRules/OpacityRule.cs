using psdPH.Logic.Compositions;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Photoshop;
using psdPH.Setups;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public class OpacityRule : LayerRule, DocRule
    {
        
        public override string ToString() => "прозрачность";

        public int Opacity=notSetOpacity;
        public override void Apply(DocumentWr doc)
        {
            dynamic layer = getLayerWr(doc);
            layer.Opacity = Opacity;
            
        }
        public OpacityRule(Composition composition) : base(composition) { }
        public OpacityRule() : base(null) { }
        protected const int notSetOpacity = -1;
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&Opacity!=notSetOpacity;
        }
        protected override DtoConverter DtoConverter => new OpacityRuleDtoConverter();
    }
    public class OpacityRuleDto:LayerRuleDto
    {
        public int Opacity;
    }
    public class OpacityRuleDtoConverter : LayerRuleDtoConverter
    {
        public override object GetDto(object _obj)
        {
            var dto = new OpacityRuleDto();
            UpdateDto(_obj, dto);
            return dto;
        }
        protected override void UpdateDto(object _obj, object _dto)
        {
            var obj = _obj as OpacityRule;
            var dto = _dto as OpacityRuleDto;
            base.UpdateDto(_obj, _dto);
            dto.Opacity = obj.Opacity;
        }
    }
    public class OpacityRuleSetups : LayerRuleSetupSource
    {
        protected Setup getOpacitySetup(OpacityRule opacityRule)
        {
            var config = new ReflectionConfig(opacityRule, nameof(opacityRule.Opacity), "прозрачность");
            return new IntSetup(config, 0, 100);
        }
        public override Setup[] GetSetups(object obj)
        {
            return new Setup[]
            {
                getLayerCompositionSetup<LayerComposition>(obj as LayerRule,"для слоя"),
                getOpacitySetup(obj as OpacityRule)
            };
        }
    }
}
