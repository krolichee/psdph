using psdPH.Compositions;
using psdPH.Photoshop;
using psdPH.Utils;
using System;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    public class GroupLeaf : LayerComposition
    {
        public override void Apply(DocumentWr doc) { }
        public override bool IsMatching(DocumentWr doc)
        {
            return LayerDescriptor.Group(LayerName).IsInDoc(doc);
        }

        public GroupLeaf():base() {}
    }

}

