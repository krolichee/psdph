using Photoshop;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    [UIName("Группа")]
    public class GroupLeaf : LayerComposition
    {
        [XmlIgnore]
        public override Setup[] Setups => new Setup[0];
        public override void Apply(Document doc) { }
        public override bool IsMatching(Document doc)
        {
            return LayerDescriptor.Group(LayerName).DoesDocHas(doc);
        }
    }

}

