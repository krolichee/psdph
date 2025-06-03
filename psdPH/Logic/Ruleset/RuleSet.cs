using Photoshop;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace psdPH.Logic
{
    [Serializable]
    [XmlRoot("Ruleset")]
    public class RuleSet
    {public void AddRule(Rule rule)
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
        public ObservableCollection<Rule> Rules = new ObservableCollection<Rule>();

        protected Rule[] CoreRules => Rules.Where(item => (item is CoreRule)).ToArray();
        protected Rule[] NonCoreRules => Rules.Where(item => !(item is CoreRule)).ToArray();

        public void NonCoreApply(Document doc)
        {
            foreach (var item in NonCoreRules)
                item.Apply(doc);
        }
        public void CoreApply()
        {
            foreach (CoreRule item in CoreRules)
                item.CoreApply();
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
