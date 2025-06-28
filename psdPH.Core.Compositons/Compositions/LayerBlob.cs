using Photoshop;
using psdPH.Photoshop;
using System;

namespace psdPH.Logic.Compositions
{
    public class LayerBlob:LayerComposition
    {
        protected void register()
        {
            DtoConvertersRegistry.Register<LayerBlob>(new LayerCompositionDtoConverter());
        }
        public LayerBlob()
        {
            register();
        }

        public LayerBlob(string layername) : base(layername)
        {
            register();
        }

        public override void Apply(DocumentWr doc)
        {
            throw new NotImplementedException();

           // doc = doc.OpenSmartLayer(LayerName);

            //применение

            //doc.Close(PsSaveOptions.psSaveChanges);
        }
        public override bool IsMatching(DocumentWr doc) => LayerDescriptor.DoesDocHas(doc);
        public override MatchingResult IsMatchingRouted(DocumentWr doc)
        {
            MatchingResult result = new MatchingResult(this, IsMatching(doc));
            if (!result)
                return result;
            doc = doc.OpenSmartLayer(LayerDescriptor);
            matchChildren(result, doc);
            doc.Doc.Close(PsSaveOptions.psSaveChanges);
            return result;
        }

    }
}
