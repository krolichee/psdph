using Photoshop;
using psdPH.Logic.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Xml.Serialization;
using static psdPH.Logic.PhotoshopDocumentExtension;

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
        public LayerComposition(string layername) { LayerName = layername; }
        public LayerComposition() { LayerName = string.Empty; }
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
                var textConfig = new ParameterConfig(this, nameof(this.Text), LayerName);
                ///TODO
                result.Add(Parameter.RichStringInput(textConfig));
                return result.ToArray();
            }
        }

        public PsJustification Justification = PsJustification.psLeft;

        override public void Apply(Document doc)
        {
            ArtLayer layer = doc.GetLayerByName(LayerName, LayerListing.Recursive);
            layer.TextItem.Contents = Text;
        }
    }

    [Serializable]
    [XmlRoot("Layer")]
    public class LayerLeaf : LayerComposition
    {
        public override string UIName => "Слой";

        public override Parameter[] Parameters => new Parameter[0];

        public override void Apply(Document doc) { }
    }
    [Serializable]
    [XmlRoot("Group")]
    public class GroupLeaf : LayerComposition
    {
        public override string UIName => "Группа";

        public override Parameter[] Parameters => new Parameter[0];
        public override string ObjName => LayerName;
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

        public bool SwitchStyle;

        public override Parameter[] Parameters => new Parameter[0];
        static Dictionary<PsJustification, HorizontalAlignment> JustificationMatchDict = new Dictionary<PsJustification, HorizontalAlignment>()
            {
                { PsJustification.psLeft,HorizontalAlignment.Left
    },
                { PsJustification.psRight,HorizontalAlignment.Right
},
                { PsJustification.psCenter,HorizontalAlignment.Center },
            };
public override void Apply(Document doc)
        {
            var textLayer = doc.GetLayerByName(TextLeafLayername);
            var areaLayer = doc.GetLayerByName(LayerName);

            Alignment alignment = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Center);

            doc.FitTextLayer(textLayer, areaLayer);

            alignment.H = JustificationMatchDict[TextLeaf.Justification];

            var initialAVector = doc.GetAlightmentVector(areaLayer, textLayer, alignment);
            textLayer.Translate(initialAVector);

        }
    }

}

