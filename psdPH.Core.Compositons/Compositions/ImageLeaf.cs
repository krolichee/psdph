using Photoshop;
using psdPH.Compositions;
using psdPH.Photoshop;
using System;

namespace psdPH.Logic.Compositions
{
    [Obsolete]
    [Serializable]
    public class ImageLeaf : LayerComposition
    {
        public string Path;

        public override void Apply(DocumentWr doc)
        {
            throw new NotImplementedException();
        }

        public override bool IsMatching(DocumentWr doc)
        {
            return LayerDescriptor.DoesDocHas(doc);
        }
        public ImageLeaf():base()
        {
            
        }
        protected override DtoConverter DtoConverter => new LayerCompositionDtoConverter();
    }

}

