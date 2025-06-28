using psdPH.Logic.Ruleset.Conditions;
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
    
}
