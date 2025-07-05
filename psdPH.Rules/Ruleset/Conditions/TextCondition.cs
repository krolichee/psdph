using psdPH.Logic.Compositions;
using psdPH.Logic.Ruleset.Conditions;
using psdPH.Setups;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    public abstract class TextCondition : Condition
    {
        [XmlIgnore]
        public string Text;
        protected TextCondition() { }
        public override bool IsSetUp()
        {
            return Text != null;
        }
    }
    public class TextConditionSetupSource : SetupsSource
    {
        public override Setup[] GetSetups(object obj)
        {
            var condition = obj as TextCondition;
            return new[] { new StringInputSetup(new ReflectionConfig(condition, nameof(condition.Text), "текст")) };
        }
    }

}
