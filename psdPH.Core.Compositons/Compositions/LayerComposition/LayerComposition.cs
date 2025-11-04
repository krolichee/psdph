using psdPH.Photoshop;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    public abstract class LayerComposition : Composition
    {
        [XmlIgnore]
        public string LayerName { get => LayerDescriptor.LayerName; set => LayerDescriptor = LayerDescriptor.Layer(value); }
        [XmlIgnore]
        public LayerDescriptor LayerDescriptor;
        public override string ObjName => LayerName;
        public LayerWr GetLayerWr(DocumentWr doc)
        {
            return LayerDescriptor.GetLayerWr(doc);
        }
        public LayerComposition(string layername) : base() 
        { 
            LayerDescriptor = LayerDescriptor.Layer(layername); 
        }
        //Ужасная неочевидная условность, тянущаяся от наследования от DtoSerialized. Фу
        public LayerComposition():base() { }
        //protected LayerWr getLayerWr(Document doc, string layerName) => doc.GetLayerWrByName(layerName);
    }

}

