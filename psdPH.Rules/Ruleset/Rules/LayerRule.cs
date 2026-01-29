using psdPH.Logic.Compositions;
using psdPH.Photoshop;
using psdPH.Rules;
using psdPH.Serialization;
using psdPH.Setups;
using psdPH.Utils.Setups;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public abstract class LayerRule : CompositionRule
    {
        [XmlIgnore]
        public LayerComposition LayerComposition;

        protected LayerWr getLayerWr(DocumentWr doc) =>
            LayerComposition.LayerDescriptor.GetLayerWr(doc);
        protected LayerRule(Composition composition) : base(composition) { }
        public override bool IsSetUp()
        {
            return LayerComposition != null;
        }
    };
    public class LayerRuleDto:Dto
    {
        public Guid LayerCompositionGuid;
    }
    public class LayerRuleDtoConverter : DtoConverter
    {
        public override void GetEntity(object _obj, object _dto)
        {
            var obj = _obj as LayerRule;
            var dto = _dto as LayerRuleDto;
            void findLayerComposition()
            {
                obj.LayerComposition = obj.Composition.Children.First(c => c.Guid == dto.LayerCompositionGuid) as LayerComposition;
            }
            obj.CompositionChanged += compositionChanged;
            void compositionChanged(Composition composition)
            {
                findLayerComposition();
                obj.CompositionChanged -= compositionChanged;
            }
        }
        public override object GetDto(object _obj)
        {
            var dto = new LayerRuleDto();
            UpdateDto(_obj,dto);
            return dto;
        }
        protected override void UpdateDto(object _obj, object _dto)
        {
            var obj = _obj as LayerRule;
            var dto = _dto as LayerRuleDto;
            dto.LayerCompositionGuid = obj.LayerComposition.Guid;
        }
    }
    public class LayerRuleSetupSource : SetupsSource
    {
        protected Setup getLayerSetup<T>(ReflectionConfig setupConfig) where T : LayerComposition
        {
            var composition = (setupConfig.Obj as LayerRule).Composition;
            var layers = composition.GetChildren<T>().ToArray();
            return new ChooseSetup(setupConfig, layers);
        }
        protected Setup getLayerCompositionSetup<T>(LayerRule layerRule, string desc) where T : LayerComposition
        {
            var composition = layerRule.Composition;
            var layerNameConfig = new ReflectionConfig(layerRule, nameof(layerRule.LayerComposition), desc);
            return getLayerSetup<T>(layerNameConfig);
        }
        public override Setup[] GetSetups(object obj)
        {
            return new Setup[] { getLayerCompositionSetup<LayerComposition>(obj as LayerRule, "для слоя") };
        }
    }
}
