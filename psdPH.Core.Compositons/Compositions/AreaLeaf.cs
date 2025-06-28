using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Photoshop;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Serialization;

namespace psdPH.Compositions
{
    [Serializable]
    [UIName("Зона")]
    public class AreaLeaf : LayerComposition
    {
        public override void Apply(DocumentWr doc)
        {
            this.GetLayerWr(doc).Visible = false;
        }
        public override bool IsMatching(DocumentWr doc)
        {
            return LayerDescriptor.Layer(LayerName).DoesDocHas(doc);
        }
        public AreaLeaf():base()
        {

        }
    }

}

