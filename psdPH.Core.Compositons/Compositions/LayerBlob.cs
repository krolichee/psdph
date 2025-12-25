using Photoshop;
using psdPH.Compositions;
using psdPH.Photoshop;
using System;

namespace psdPH.Logic.Compositions
{
    public class LayerBlob:LayerComposition
    {
        public LayerBlob():base() { }

        public LayerBlob(string layername) : base(layername) { }

        public override void Apply(DocumentWr doc)
        {
            throw new NotImplementedException();

           // doc = doc.OpenSmartLayer(LayerName);

            //применение

            //doc.Close(PsSaveOptions.psSaveChanges);
        }
        public override bool IsMatching(DocumentWr doc) => LayerDescriptor.IsInDoc(doc);
        public override MatchingResult IsMatchingRouted(DocumentWr doc)
        {
            MatchingResult result = new MatchingResult(this, IsMatching(doc));
            if (!result)
                return result;
            doc = doc.OpenSmartLayer(LayerDescriptor);
            var childrenMatch = Hierarchy.Children.Match(doc);
            if (childrenMatch != null)
                result = childrenMatch.Value;
            doc.Doc.Close(PsSaveOptions.psSaveChanges);
            return result;
        }

    }
}
