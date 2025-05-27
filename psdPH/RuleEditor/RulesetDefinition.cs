using psdPH.Logic;
using psdPH.Logic.Rules;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.RuleEditor
{
    public abstract class RulesetDefinition
    {
        protected readonly Composition _root;
        public abstract Rule[] Rules{ get;}
        public abstract Condition[] Conditions { get; }
        public RulesetDefinition(Composition root)
        {
            _root = root;
        }
    }
}
