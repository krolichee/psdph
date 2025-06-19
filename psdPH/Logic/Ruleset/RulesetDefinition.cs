using psdPH.Logic;
using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic.Ruleset
{
    public class RulesetDefinition
    {
        public CompositionRule[] Rules;
        public CompositionCondition[] Conditions;
        public RulesetDefinition(CompositionRule[] rules, CompositionCondition[] conditions)
        {
            Rules = rules;
            Conditions = conditions;
        }
    }
}
