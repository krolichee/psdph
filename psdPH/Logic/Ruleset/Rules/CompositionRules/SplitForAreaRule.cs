using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Logic.Rules;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Utils.ReflectionSetups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static psdPH.Utils.SplitTextToRatio;

namespace psdPH.Logic.Rules
{
    public class SplitForAreaRule : TextRule, CompositionRule
    {
        public override string ToString() => "разделить текст";
        
        public string AreaLayerName;
        [XmlIgnore]
        public AreaLeaf AreaLeaf
        {
            protected get => Composition.GetChildren<AreaLeaf>().FirstOrDefault((c) => c.LayerName == AreaLayerName); set
            {
                AreaLayerName = value.LayerName;
            }
        }
        public override Setup[] Setups { get
            {
                var result = new List<Setup>();
                result.Add(getTextLeafSetup());
                var areaConfig = new SetupConfig(this,nameof(AreaLeaf), "для зоны");
                result.Add(Setup.Choose(areaConfig,Composition.GetChildren<AreaLeaf>()));
                return result.ToArray();
            } }
        protected override void _apply(Document doc)
        {
            var size = AreaLeaf.ArtLayerWr(doc).GetNoFxBoundsSize();
            var ratio = size.Width / size.Height;
            TextLeaf.Text = Splitter.Split(TextLeaf.Text, ratio);
            if (TextLeaf.Text.Length!=0)
                QuestionableSetups.Setups.AddRange(TextLeaf.Setups);
        }

        public void CompApply()
        {
            Apply(null);
        }

        public SplitForAreaRule(Composition composition) : base(composition) { }
        public SplitForAreaRule():base(null) { }
    }
}
