using psdPH.Logic.Compositions;
using psdPH.Photoshop;
using psdPH.Setups;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public class VisibleRule : LayerRule
    {
        public override string ToString() => "видимость";
        public bool Toggle = true;
        public override void Apply(DocumentWr doc)
        {
            getLayerWr(doc).Visible = Toggle;
        }
        protected override DtoConverter DtoConverter => new VisibleRuleDtoConverter();

        public VisibleRule(Composition composition) : base(composition) { }
        public VisibleRule() : base(null) {
            SetupsRegistry.Register<VisibleRule>(new VisibleRuleSetups());
            
        }
    }
    public class VisibleRuleDto : LayerRuleDto
    {
        public bool Toggle;
    }
    public class VisibleRuleDtoConverter : LayerRuleDtoConverter
    {
        public override void GetEntity(object _obj, object _dto)
        {
            base.GetEntity(_obj, _dto);
            var obj = _obj as VisibleRule;
            var dto = _dto as VisibleRuleDto;
            dto.Toggle = obj.Toggle;
        }
        public override object GetDto(object _obj)
        {
            return base.GetDto(_obj);
        }
        protected override void UpdateDto(object _obj, object _dto)
        {
            base.UpdateDto(_obj, _dto);
            var obj = _obj as VisibleRule;
            var dto = _dto as VisibleRuleDto;
            obj.Toggle = dto.Toggle;
        }
    }
    public class VisibleRuleSetups : LayerRuleSetupSource
    {
        protected Setup VisibilitySetup(VisibleRule rule)
        {
            var opacityConfig = new ReflectionConfig(rule, nameof(rule.Toggle), "установить");
            return new CheckSetup(opacityConfig);
        }
        public override Setup[] GetSetups(object obj)
        {
            return new Setup[] { getLayerCompositionSetup<LayerComposition>(obj as LayerRule, "для слоя"), VisibilitySetup(obj as VisibleRule) };
        }
    }

}
