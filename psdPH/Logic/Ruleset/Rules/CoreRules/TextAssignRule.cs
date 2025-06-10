using Photoshop;
using psdPH;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Logic.Rules;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace psdPHTest.Logic.Parameters
{
    
    public class TextAssignRule : TextRule,CoreRule
    {
        public TextAssignRule(Composition composition) : base(composition) { }
        public string ParameterName;
        [XmlIgnore]
        public StringParameter Parameter
        {
            protected get => Composition.ParameterSet.AsCollection().FirstOrDefault(p => p.Name == ParameterName && p is StringParameter) as StringParameter;
            set => ParameterName = value?.Name;
        }
        public override Setup[] Setups => throw new NotImplementedException();

        protected override void _apply(Document doc)
        {
            TextLeaf.Text = Parameter.Text;
        }
        public void CoreApply()
        {
            _apply(null);
        }
        public TextAssignRule():base(null) { }
    }
}
