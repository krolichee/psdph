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
        public override Parameter[] Setups
        {
            get
            {
                var result = new List<Parameter>();
                var textLeafConfig = new ParameterConfig(this, nameof(this.TextLeafLayerName), "поля");
                var textLeavesNames = Composition.getChildren<TextLeaf>().Select(t => t.LayerName).ToArray();
                result.Add(Parameter.Choose(textLeafConfig, textLeavesNames));
                return result.ToArray();
            }
        }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&&TextLeafLayerName!=null;
        }

    }
    
}
