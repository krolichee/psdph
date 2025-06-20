using Photoshop;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void Apply(Document doc)
        {
            throw new NotImplementedException();

            doc = doc.OpenSmartLayer(LayerName);

            //применение

            doc.Close(PsSaveOptions.psSaveChanges);
        }
        public override bool IsMatching(Document doc) => LayerDescriptor.DoesDocHas(doc);
        public override MatchingResult IsMatchingRouted(Document doc)
        {
            MatchingResult result = new MatchingResult(this, IsMatching(doc));
            if (!result)
                return result;
            doc = doc.OpenSmartLayer(LayerName);
            matchChildren(result, doc);
            doc.Close(PsSaveOptions.psSaveChanges);
            return result;
        }

    }
}
