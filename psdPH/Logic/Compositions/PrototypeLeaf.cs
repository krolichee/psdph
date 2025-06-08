using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;
using static psdPH.Logic.PhotoshopLayerExtension;

namespace psdPH
{
    [Serializable]
    [UIName("Прототип")]
    public class PrototypeLeaf : Composition
    {
        Blob blob;
        [XmlIgnore]
        public Blob Blob
        {
            get
            {
                if (blob == null)
                    blob =Siblings<Blob>().First(b => b.LayerName == LayerName); return blob;
            }
            set { LayerName = value.LayerName; }
        }
        public string RelativeLayerName;
        public string LayerName;

        public Vector GetRelativeLayerAlightmentVector(Document doc)
        {
            
            var options = new AlignOptions(Alignment.Create("up", "left"), Photoshop.LayerWr.ConsiderFx.NoFx);
            var layerDescriptor = LayerDescriptor.Layer(LayerName);
            var relLayerDescriptor = LayerDescriptor.Layer(RelativeLayerName);
            var layerWr = layerDescriptor.GetFromDoc(doc);
            var relLayerWr =  relLayerDescriptor.GetFromDoc(doc);
            //Здесь ожидается не вектор выравнивания, а вектор приведение к той же разнице,
            //то и с опорным слоем, поэтому аргементы меняются местами
            return relLayerWr.GetAlightmentVector(layerWr,options);
        }
        [XmlIgnore]
        public override Setup[] Setups => new Setup[0];
        public override string ObjName => Blob.LayerName;
        public override void Apply(Document doc)
        {
            //doc.GetLayerByName(Blob.LayerName).Opacity = 0;
        }

        public override bool IsMatching(Document doc)
        {
            return LayerDescriptor.Layer(RelativeLayerName).DoesDocHas(doc)
                && Blob.IsMatching(doc);
        }

        public PrototypeLeaf(Blob blob, string rel_layer_name)
        {
            this.blob = blob;
            LayerName = blob.LayerName;
            RelativeLayerName = rel_layer_name;
        }
        public PrototypeLeaf() { }
    }


}

