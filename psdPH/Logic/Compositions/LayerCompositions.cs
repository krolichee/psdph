using Photoshop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
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

}

