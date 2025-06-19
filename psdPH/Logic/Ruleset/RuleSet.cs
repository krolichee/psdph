using Photoshop;
using psdPH.Logic.Ruleset.Rules;
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
        public ObservableCollection<CompositionRule> Rules = new ObservableCollection<CompositionRule>();
        public void AddRule(CompositionRule rule)
        {
            rule.RestoreComposition(Composition);
            Rules.Add(rule);
        }
        public void AddRules(CompositionRule[] rules)
        {
            foreach (var rule in rules)
                AddRule(rule);
        }
        public event Action Updated;
        [XmlIgnore]
        public Composition Composition;
        public void Apply<T>(Document doc)
        {
            foreach (var item in Rules)
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
