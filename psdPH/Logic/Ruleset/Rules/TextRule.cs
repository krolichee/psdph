using psdPH.Logic.Compositions;
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

}
