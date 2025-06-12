using Photoshop;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    [UIName("Текст")]
    public class TextLeaf : LayerComposition
    {
        public string Text = string.Empty;
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                var textConfig = new SetupConfig(this, nameof(this.Text),"текст " + LayerName);
                result.Add(Setup.RichStringInput(textConfig));
                return result.ToArray();
            }
        }
        override public void Apply(Document doc)
        {
            ArtLayer layer = ArtLayerWr(doc).ArtLayer;
            layer.TextItem.Contents = Text?.Replace("\n", "\r");
        }
        public override bool IsMatching(Document doc)
        {
           return LayerDescriptor.Layer(LayerName,PsLayerKind.psTextLayer).DoesDocHas(doc);
        }
    }

}

