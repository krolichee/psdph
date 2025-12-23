using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Photoshop.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using System.Windows;
using System.Xml.Serialization;
using psdPH.Photoshop;

namespace psdPH.Logic.Compositions
{
    public class PrototypeBlob:LayerBlob
    {
        public LayerDescriptor RelativeLayerDescriptor;
        [XmlIgnore]
        public string RelativeLayerName { get => RelativeLayerDescriptor.LayerName; set => RelativeLayerDescriptor = LayerDescriptor.Layer(value); }
        public override void Apply(DocumentWr doc) { }
        public Vector GetRelativeLayerAlightmentVector(DocumentWr docWr)
        {
            var options = new AlignOptions(Alignment.Create("up", "left"),ConsiderFx.NoFx);
            var layerWr = LayerDescriptor.GetLayerWr(docWr);
            var relLayerWr = RelativeLayerDescriptor.GetLayerWr(docWr);

            var bringToRelativeVector = relLayerWr.GetAlightmentVector(layerWr, options);
            return bringToRelativeVector;
        }
        public override bool IsMatching(DocumentWr doc)
        {
            return RelativeLayerDescriptor.IsInDoc(doc)
                && base.IsMatching(doc);
        }
        public PlaceholderLeaf[] Placeholders => Hierarchy.GetSiblings<PlaceholderLeaf>().Where(p=>p.PrototypeBlob==this).ToArray();
    }
}
