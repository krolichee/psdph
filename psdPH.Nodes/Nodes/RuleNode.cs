using psdPH.Logic.Ruleset.Rules;
using psdPH.Photoshop;
using psdPH.Setups;
using psdPH.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace psdPH.Nodes.Nodes
{
    public class RuleNode : Node
    {
        public override string ToString() => LocalizationService.Localize(rule.GetType());
        Rule rule;
        public override List<Setup> Inputs => rule.Setups.ToList();
        public override List<Setup> Outputs => new List<Setup>();
        public RuleNode() : base() { }
        public RuleNode(Rule rule):base()
        {
            this.rule = rule;
        }
        protected override void _apply()
        {
            rule.Apply(PhotoshopWrapper.GetActiveDocument());
        }
    }
}
