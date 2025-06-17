using Photoshop;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Logic.Ruleset.Rules.RulesetAffectingRule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace psdPH.Logic
{
    [Serializable]
    public class RuleSet: ISerializable
    {
        public ObservableCollection<Rule> Rules = new ObservableCollection<Rule>();
        public void AddRule(Rule rule)
        {
            rule.RestoreComposition(Composition);
            Rules.Add(rule);
        }
        public void AddRules(Rule[] rules)
        {
            foreach (var rule in rules)
                AddRule(rule);
        }
        public event Action Updated;
        [XmlIgnore]
        public Composition Composition;

        Rule[] skipRules()
        {
            for (int i = 0; i < Rules.Count; i++)
              if(Rules[i] is SkipOtherRule)
                    if ((Rules[i] as ConditionRule).Condition.IsValid())
                        return Rules.Take(i+1).ToArray();
            return Rules.ToArray();
        }
        public void Apply<T>(Document doc)
        {
            var rules = skipRules();
            foreach (var item in rules)
                if (item is T)
                    item.Apply(doc);
        }

        public void RestoreComposition(Composition composition)
        {
            this.Composition = composition;
            foreach (var rule in Rules)
            {
                rule.RestoreComposition(composition);
            }
        }

        public RuleSet()
        {
            Rules.CollectionChanged += (_, __) => Updated?.Invoke();
        }
    }


}
