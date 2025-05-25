using Photoshop;
using System;
using System.Collections.ObjectModel;
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
        public event Action Updated;
        [XmlIgnore]
        public Composition Composition;
        public ObservableCollection<Rule> Rules = new ObservableCollection<Rule>();

        public void apply(Document doc)
        {

            foreach (var item in Rules)
            {
                item.Apply(doc);
            }
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
