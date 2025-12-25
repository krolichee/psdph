using Photoshop;
using psdPH.Photoshop;
using psdPH.Utils;
using System;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    public class TextLeaf : LayerComposition
    {
        string Text = string.Empty;
        override public void Apply(DocumentWr doc)
        {
            ArtLayer layer = (GetLayerWr(doc) as ArtLayerWr).ArtLayer;
            layer.TextItem.Contents = Text?.Replace("\n", "\r");
        }
        public override bool IsMatching(DocumentWr doc)
        {
           return LayerDescriptor.Layer(LayerName,PsLayerKind.psTextLayer).IsInDoc(doc);
        }

        public TextLeaf():base() {  }
    }
}

