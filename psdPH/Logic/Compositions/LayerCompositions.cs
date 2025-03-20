using Photoshop;
using psdPH.Logic.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [XmlInclude(typeof(ImageLeaf))]
    [XmlInclude(typeof(TextLeaf))]

    [XmlInclude(typeof(LayerLeaf))]
    [XmlInclude(typeof(GroupLeaf))]

    [XmlInclude(typeof(TextAreaLeaf))]

    public abstract class LayerComposition : Composition
    {
        public string LayerName;
        public override string ObjName => LayerName;
    }
    [Serializable]
    [XmlRoot("Image")]
    public class ImageLeaf : LayerComposition
    {
        public override string UIName => "Изобр.";
        public string Path;

        public override Parameter[] Parameters
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public ImageLeaf()
        {
        }

        public override void Apply(Document doc)
        {
            throw new NotImplementedException();
        }
    }
    [Serializable]
    [XmlRoot("Text")]

    public enum BlobMode
    {
        Layer,
        Path
    }
    
    public class TextLeaf : LayerComposition
    {
        public override string UIName => "Текст";
        public string Text = string.Empty;

        public override Parameter[] Parameters
        {
            get
            {
                var result = new List<Parameter>();
                var toggleConfig = new ParameterConfig(this, nameof(this.Text), LayerName);
                ///TODO
                result.Add(Parameter.StringInput(toggleConfig));
                return result.ToArray();
            }
        }

        public PsJustification Justification { get; set; }

        override public void Apply(Document doc)
        {
            ArtLayer layer = doc.GetLayerByName(LayerName, LayerListing.Recursive);
            layer.TextItem.Contents = Text;
        }
        public TextLeaf(string layer_name) : this()
        {
            LayerName = layer_name;
        }
        public TextLeaf()
        {
        }
    }

    [Serializable]
    [XmlRoot("Layer")]
    public class LayerLeaf : LayerComposition
    {
        public override string UIName => "Слой";

        public override Parameter[] Parameters => new Parameter[0];

        public LayerLeaf(string layername)
        {
            LayerName = layername;
        }
        public LayerLeaf() { }

        public override void Apply(Document doc) { }
    }
    [Serializable]
    [XmlRoot("Group")]
    public class GroupLeaf : LayerComposition
    {
        public override string UIName => "Группа";

        public override Parameter[] Parameters => new Parameter[0];

        public override string ObjName => LayerName;

        public GroupLeaf(string layername)
        {
            LayerName = layername;
        }
        public GroupLeaf() { }

        public override void Apply(Document doc) { }
    }
    [Serializable]
    [XmlRoot("TextArea")]
    public class TextAreaLeaf : LayerComposition
    {
        [XmlIgnore]
        public TextLeaf TextLeaf
        {
            get => Parent.getChildren<TextLeaf>().First(p => p.LayerName == TextLeafLayername); 
            set => TextLeafLayername = value.LayerName;
        }
        public override string UIName => "Текст.поле";
        public string TextLeafLayername;

        public override Parameter[] Parameters => new Parameter[0];

        Vector GetAlightmentVector(Rect targetRect,Rect dynamicRect)
        {
            targetRect
        }
        public override void Apply(Document doc)
        {
            var textLayer = doc.GetLayerByName(TextLeafLayername);
            var areaLayer = doc.GetLayerByName(LayerName);


            var initialAVector = doc.GetAlightmentVector(areaLayer,textLayer);

            textLayer.Translate(initialAVector);
            switch (TextLeaf.Justification)
            {
                case PsJustification.psCenter:
                    GetAlightmentVector()
            }



            PhotoshopDocumentExtension.FitTextLayer(textLayer,areaLayer);


            throw new NotImplementedException();
        }
    }

}

