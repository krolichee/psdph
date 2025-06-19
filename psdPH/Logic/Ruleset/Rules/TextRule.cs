using psdPH.Logic.Compositions;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public abstract class TextRule : LayerRule
    {
        protected TextRule(Composition composition) : base(composition) {
            SetupsRegistry.Register<TextRule>(new TextRuleSetupSource());
            DtoConvertersRegistry.Register<TextRule>(new LayerRuleDtoConverter());
        }
    };
    class TextRuleSetupSource : LayerRuleSetupSource
    {
        public override Setup[] GetSetups(object obj)
        {
            return new Setup[] {getLayerCompositionSetup<TextLeaf>(obj as TextRule, "для поля") };
        }
    }


}
