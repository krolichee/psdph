using Photoshop;
using psdPH;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Rules;
using System;
using System.Linq;

namespace psdPHTest.Logic.Parameters
{
    public class TextAssignRule : TextRule,CoreRule
    {
        public TextAssignRule(Composition composition) : base(composition) { }
        public string ParameterName;
        public StringParameter Parameter
        {
            protected get => Composition.Parameters.FirstOrDefault(p => p.Name == ParameterName && p is StringParameter) as StringParameter;
            set => ParameterName = value?.Name;
        }
        public override Setup[] Setups => throw new NotImplementedException();

        protected override void _apply(Document doc)
        {
            TextLeaf.Text = Parameter.Value;
        }
        public void CoreApply()
        {
            _apply(null);
        }
    }
}
