using Photoshop;
using psdPH.Utils.Setups;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    [UIName("Группа")]
    public class GroupLeaf : LayerComposition
    {
        public override void Apply(Document doc) { }
        public override bool IsMatching(Document doc)
        {
            return LayerDescriptor.Group(LayerName).DoesDocHas(doc);
        }
        public GroupLeaf():base() {}
    }

}

