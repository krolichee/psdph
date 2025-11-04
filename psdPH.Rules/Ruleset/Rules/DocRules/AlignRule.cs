using psdPH.Logic.Compositions;
using psdPH.Utils.Setups;
using static psdPH.Photoshop.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using System.Xml.Serialization;
using static psdPH.Photoshop.LayerWr;
using psdPH.Setups;
using psdPH.Compositions;
using psdPH.Photoshop;

namespace psdPH.Logic.Ruleset.Rules
{
    public class AlignRule : AreaRule
    {
        [XmlIgnore]
        public Alignment Alignment;
        [XmlIgnore]
        public ConsiderFx ConsiderFx;
       
        public AlignOptions AlignOptions
        {
            get => new AlignOptions(Alignment, ConsiderFx); set
            {
                Alignment = value.Alignment;
                ConsiderFx = value.ConsiderFx;
            }
        }
        void InitRegistrations()
        {
            SetupsRegistry.Register<AlignRule>(new AlignRuleSetupSource());
        }
        public AlignRule(Composition composition) : base(composition) {
            InitRegistrations();
        }
        public AlignRule() : base(null) {
            InitRegistrations();
        }
        public override void Apply(DocumentWr doc)
        {
            getLayerWr(doc).AlignLayer(AreaLeaf.GetLayerWr(doc), AlignOptions);
        }
        protected override DtoConverter DtoConverter => new AlignRuleDtoConverter();
    }
    public class AlignRuleDto : LayerRuleDto
    {
        public Alignment Alignment;
        public ConsiderFx ConsiderFx;
    }
    public class AlignRuleDtoConverter : LayerRuleDtoConverter
    {
        public override object GetDto(object _obj)
        {
            var dto = new AlignRuleDto();
            UpdateDto(_obj, dto);
            return dto;
        }
        protected override void UpdateDto(object _obj, object _dto)
        {
            var obj = _obj as AlignRule;
            var dto = _dto as AlignRuleDto;
            base.UpdateDto(_obj, _dto);
            dto.ConsiderFx = obj.ConsiderFx;
            dto.Alignment = obj.Alignment;
        }
    }
    public class AlignRuleSetupSource : AreaRuleSetupSource
    {
        protected Setup getAlignmentSetup(AlignRule areaRule)
        {
            var alignment_config = new ReflectionConfig(areaRule, nameof(areaRule.Alignment), "с выравниванием");
            return new AlignmentSetup(alignment_config);
        }
        protected Setup getConsiderFxSetup(AlignRule areaRule)
        {
            var considerfx_config = new ReflectionConfig(areaRule, nameof(areaRule.ConsiderFx), "по границам");
            return EnumChooseSetup.EnumChoose(considerfx_config, typeof(ConsiderFx));

        }
        public override Setup[] GetSetups(object obj)
        {
            return new Setup[] {
                getSubjectLayerSetup(obj as LayerRule),
                getAreaLeafSetup(obj as LayerRule) ,
                getAlignmentSetup(obj as AlignRule),
                getConsiderFxSetup(obj as AlignRule) };
        }
    }

}
