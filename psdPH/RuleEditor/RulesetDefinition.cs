using psdPH.Logic;
using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.RuleEditor
{
    public class RulesetDefinition
    {
        public Rule[] Rules;
        public Condition[] Conditions;
        public RulesetDefinition(Rule[] rules, Condition[] conditions)
        {
            Rules = rules;
            Conditions = conditions;
        }
    }
}
