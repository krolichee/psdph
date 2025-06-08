using psdPH.Logic.Compositions;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    public abstract class TextCondition : Condition
    {
        public string TextLeafLayerName;
        protected TextCondition(Composition composition) : base(composition) { }

        [XmlIgnore]
        public TextLeaf TextLeaf
        {
            protected get => Composition.getChildren<TextLeaf>().First(t => t.LayerName == TextLeafLayerName);
            set => TextLeafLayerName = value?.LayerName;
        }
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                var textLeafConfig = new SetupConfig(this, nameof(this.TextLeafLayerName), "поля");
                var textLeavesNames = Composition.getChildren<TextLeaf>().Select(t => t.LayerName).ToArray();
                result.Add(Setup.Choose(textLeafConfig, textLeavesNames));
                return result.ToArray();
            }
        }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&TextLeafLayerName!=null;
        }

    }
    
}
