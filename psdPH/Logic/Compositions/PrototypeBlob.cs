using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;
using System.Windows;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    public class PrototypeBlob:LayerBlob
    {
        public LayerDescriptor RelativeLayerDescriptor;
        [XmlIgnore]
        public string RelativeLayerName { get => RelativeLayerDescriptor.LayerName; set => RelativeLayerDescriptor = LayerDescriptor.Layer(value); }
        public override void Apply(Document doc) { }
        public PrototypeBlob() : base() { }
        public Vector GetRelativeLayerAlightmentVector(Document doc)
        {
            var options = new AlignOptions(Alignment.Create("up", "left"), Photoshop.LayerWr.ConsiderFx.NoFx);
            var layerWr = LayerDescriptor.GetFromDoc(doc);
            var relLayerWr = RelativeLayerDescriptor.GetFromDoc(doc);

            var bringToRelativeVector = relLayerWr.GetAlightmentVector(layerWr, options);
            return bringToRelativeVector;
        }
        public override bool IsMatching(Document doc)
        {
            return RelativeLayerDescriptor.DoesDocHas(doc)
                && base.IsMatching(doc);
        }
        public PlaceholderLeaf[] Placeholders => Siblings<PlaceholderLeaf>().Where(p=>p.PrototypeBlob==this).ToArray();
    }
}
