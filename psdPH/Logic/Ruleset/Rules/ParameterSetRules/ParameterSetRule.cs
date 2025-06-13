using Photoshop;
using psdPH.Logic.Parameters;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic.Ruleset.Rules
{
    public abstract class ParameterSetRule : ConditionRule, IParameterSetRule
    {
        public ParameterSet ParameterSet;
        public ParameterSetRule(Composition composition) : base(composition) { }
        public ParameterSetRule() : base(null) { }
        

        protected Setup getParameterSetup<T>(string name,string desc) where T : Parameter
        {
            var flagConfig = new SetupConfig(this, name, desc);
            Parameter[] parameters = ParameterSet.GetByType<T>().ToArray();
            return Setup.Choose(flagConfig, parameters);
        }
        public abstract override Setup[] Setups { get; }

        public void SetParameterSet(ParameterSet parset)
        {
            ParameterSet = parset;
            if (Condition is ParameterSetCondition)
                (Condition as ParameterSetCondition).SetParameterSet(parset);
        }
    }
}
