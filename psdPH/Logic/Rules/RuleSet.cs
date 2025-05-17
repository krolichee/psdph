using Photoshop;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace psdPH.Logic
{
    [Serializable]
    [XmlRoot("Ruleset")]
    public class RuleSet
    {
        public event Action Updated;
        [XmlIgnore]
        public Composition composition;
        public ObservableCollection<Rule> Rules = new ObservableCollection<Rule>();

        public void apply(Document doc)
        {

            foreach (var item in Rules)
            {
                item.Apply(doc);
            }
        }

        internal void restoreLinks(Composition composition)
        {
            this.composition = composition;
            foreach (var rule in Rules)
            {
                rule.restoreComposition(composition);
            }

        }
        public RuleSet()
        {
            Rules.CollectionChanged += (_, __) => Updated?.Invoke();
        }
    }


}
