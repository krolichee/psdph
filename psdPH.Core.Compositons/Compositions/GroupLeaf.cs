using psdPH.Compositions;
using psdPH.Photoshop;
using psdPH.Utils;
using System;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    [UIName("Группа")]
    public class GroupLeaf : LayerComposition
    {
        protected override DtoConverter DtoConverter => new LayerCompositionDtoConverter();

        public override void Apply(DocumentWr doc) { }
        public override bool IsMatching(DocumentWr doc)
        {
            return LayerDescriptor.Group(LayerName).DoesDocHas(doc);
        }

        public GroupLeaf():base() {}
    }

}

