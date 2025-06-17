using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic.Ruleset.Rules.RulesetAffectingRule
{
    public class SkipOtherRule : ConditionRule,IRulesetAffectingRule
    {
        public override string ToString() => "пропустить последующие правила";
        public SkipOtherRule(Composition composition) : base(composition) { }
        public SkipOtherRule() : base(null) { }

        public override Setup[] Setups => new Setup[0];

        protected override void _apply(Document doc) {  }
    }
}
