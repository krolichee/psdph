using Photoshop;
using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH.Logic.Compositions
{
    public abstract class LayerComposition : Composition
    {
        public string LayerName;
        public override string ObjName => LayerName;
        public ArtLayerWr ArtLayerWr(Document doc)
        {
            return new ArtLayerWr(doc.GetLayerByName(LayerName));
        }
        public LayerComposition(string layername) { LayerName = layername; }
        public LayerComposition() { LayerName = string.Empty; }
        protected LayerWr getLayer(Document doc, string layerName) => doc.GetLayerWrByName(layerName);
    }
    [Serializable]
    [XmlRoot("Image")]
    [UIName("Изображение")]
    public class ImageLeaf : LayerComposition
    {
        public string Path;
        [XmlIgnore]
        public override Parameter[] Setups
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void Apply(Document doc)
        {
            throw new NotImplementedException();
        }

    }
    public enum BlobMode
    {
        Layer,
        Path
    }
    [Serializable]
    [UIName("Текст")]
    public class TextLeaf : LayerComposition
    {
        public string Text = string.Empty;
        [XmlIgnore]
        public override Parameter[] Setups
        {
            get
            {
                var result = new List<Parameter>();
                var textConfig = new ParameterConfig(this, nameof(this.Text), LayerName);
                result.Add(Parameter.RichStringInput(textConfig));
                return result.ToArray();
            }
        }
        override public void Apply(Document doc)
        {
            ArtLayer layer = ArtLayerWr(doc).ArtLayer;
            layer.TextItem.Contents = Text.Replace("\n", "\r");
        }
    }

    [Serializable]
    [UIName("Слой")]
    public class LayerLeaf : LayerComposition
    {
        [XmlIgnore]
        public override Parameter[] Setups => new Parameter[0];

        public override void Apply(Document doc) { }
    }
    [Serializable]
    [UIName("Группа")]
    public class GroupLeaf : LayerComposition
    {
        [XmlIgnore]
        public override Parameter[] Setups => new Parameter[0];
        public override void Apply(Document doc) { }
    }
    [Serializable]
    [UIName("Зона")]
    public class AreaLeaf : LayerComposition
    {
        [XmlIgnore]
        public override Parameter[] Setups => new Parameter[0];
        static Dictionary<PsJustification, HorizontalAlignment> JustificationMatchDict = new Dictionary<PsJustification, HorizontalAlignment>()
            {
                { PsJustification.psLeft,HorizontalAlignment.Left
    },
                { PsJustification.psRight,HorizontalAlignment.Right
    },
                { PsJustification.psCenter,HorizontalAlignment.Center },
            };

        public void Fit(Document doc, LayerComposition layerLeaf, Alignment alignment)
        {
            //TextLeaf textLeaf;
            //if (layerLeaf is TextLeaf)
            //    textLeaf = layerLeaf as TextLeaf;
            //else
            //    throw new NotImplementedException();

            //var dynamicLayer = doc.GetLayerByName(layerLeaf.LayerName);

            //var areaLayer = doc.GetLayerByName(LayerName);

            //areaLayer.Opacity = 0;
            //if (textLeaf.Text == "")
            //    return;
            //textLeaf.Apply(doc);

            //dynamicLayer.AdjustTextLayerTo(areaLayer);

            ////alignment.H = JustificationMatchDict[textLeaf.Justification];

            //dynamicLayer.AlignLayer(areaLayer, alignment);
        }
        public override void Apply(Document doc) { }
    }
    [Serializable]
    [UIName("Заглушка")]
    public class PlaceholderLeaf : LayerComposition, CoreComposition
    {
        [XmlIgnore]
        public PrototypeLeaf Prototype
        {
            get
            {
                return Parent.getChildren<PrototypeLeaf>().First(p => p.LayerName == PrototypeLayerName);
            }
            set
            {
                PrototypeLayerName = value.LayerName;
            }
        }
        public string PrototypeLayerName;
        public override string ObjName => LayerName;

        public override Parameter[] Setups => new Parameter[0];
        Blob _replacement;
        [XmlIgnore]
        public Blob Replacement
        {
            get => _replacement;
            set { _replacement = value; _replacement.LayerName = $"{PrototypeLayerName}_{LayerName}"; }
        }
        public override void Apply(Document doc)
        {
            if (Replacement != null)
            {
                ReplaceWithFiller(doc, Replacement);
                Replacement.Apply(doc);
            }
        }

        public PlaceholderLeaf(string layername, string prototypeLayername)
        {
            LayerName = layername;
            PrototypeLayerName = prototypeLayername;
        }
        public PlaceholderLeaf() { }
        public override void RestoreParents(Composition parent = null)
        {
            base.RestoreParents(parent);
        }

        internal void ReplaceWithFiller(Document doc, Blob blob)
        {
            ArtLayer phLayer = doc.GetLayerByName(LayerName);
            ArtLayer originalLayer = doc.GetLayerByName(PrototypeLayerName);
            originalLayer.Visible = true;
            ArtLayerWr newLayerWr = new ArtLayerWr(doc.CloneSmartLayer(PrototypeLayerName));
            originalLayer.Visible = false;
            var prototypeAVector = Prototype.GetRelativeLayerAlightmentVector(doc);

            var phAVector = newLayerWr.GetAlightmentVector(new ArtLayerWr(phLayer));

            newLayerWr.TranslateV(phAVector+prototypeAVector);
            //ph_layer.Delete();
            phLayer.Opacity = 0;
            newLayerWr.Name = blob.LayerName;

            //Parent.addChild(blob);
            //Parent.removeChild(this);
        }

        public void CoreApply()
        {
            ((CoreComposition)Replacement).CoreApply();
        }
    }

}

