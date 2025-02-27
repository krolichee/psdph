using Photoshop;
using psdPH.Logic;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace psdPH
{
    [XmlInclude(typeof(FlagLeaf))]
    [XmlInclude(typeof(ImageLeaf))]
    [XmlInclude(typeof(VisLeaf))]
    [XmlInclude(typeof(TextLeaf))]
    [XmlInclude(typeof(Blob))]
    [XmlInclude(typeof(PrototypeLeaf))]
    [XmlInclude(typeof(PlaceholderLeaf))]
    [XmlInclude(typeof(LayerLeaf))]
    [XmlInclude(typeof(Rule))]
    public abstract class Composition
    {
        public string XmlName
        {
            get
            {
                Type type = this.GetType();
                XmlRootAttribute rootAttribute = (XmlRootAttribute)Attribute.GetCustomAttribute(type, typeof(XmlRootAttribute));
                return rootAttribute.ElementName;
            }
        }
        public virtual string UIName { get { return ""; } }
        abstract public string ObjName { get; }
        public override string ToString()
        {
            return ObjName;
        }
        [XmlIgnore]
        virtual public Composition Parent { get; set; }
        virtual public void apply(Document doc) { }
        virtual public void addChild(Composition child) { }
        virtual public void removeChild(Composition child) { }
        public void restore()
        {
            restoreParents();
            restoreRuleLinks();
        }
        virtual public void restoreParents(Composition parent = null)
        {
            if (parent != null)
                Parent = parent;
            if (getChildren() != null)
                foreach (var item in getChildren())
                    item.restoreParents(this);
        }
        public void restoreRuleLinks()
        {
            RuleSet.restoreLinks(this);
        }
        virtual public Composition[] getChildren() { return null; }
        virtual public T[] getChildren<T>() { return null; }
        public RuleSet RuleSet = new RuleSet();

        public Composition() { }
    }
    public abstract class LayerComposition : Composition
    {
        public string LayerName;
        public override string ObjName => LayerName;
    }
    [Serializable]
    [XmlRoot("Flag")]
    public partial class FlagLeaf : Composition
    {
        public override string UIName => EnumLocalization.GetLocalizedDescription(this.GetType());
        public bool Toggle;
        public string Name;
        public override string ObjName => Name;

        public FlagLeaf(string name)
        {
            Name = name;
        }
        public FlagLeaf()
        {
        }
    }
    [Serializable]
    [XmlRoot("Image")]
    public class ImageLeaf : LayerComposition
    {
        public override string UIName => "Изобр.";

        override public void apply(Document doc)
        {

        }
        public ImageLeaf()
        {
        }
    }
    [Serializable]
    [XmlRoot("Visibility")]
    public class VisLeaf : LayerComposition
    {
        public override string UIName => "Видим.";
        public bool Toggle;
        public VisLeaf()
        {

        }

    }
    [Serializable]
    [XmlRoot("Text")]
    public class TextLeaf : LayerComposition
    {
        public override string UIName => EnumLocalization.GetLocalizedDescription(this.GetType());
        public string Text;

        override public void apply(Document doc)
        {
            ArtLayer layer =  doc.GetLayerByName(LayerName, LayerListing.Recursive);
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
    public enum BlobMode
    {
        Layer,
        Path
    }

    [Serializable]
    [XmlRoot("Blob")]
    public class Blob : LayerComposition
    {
        public override string UIName => "Подфайл";
        public BlobMode Mode;
        [XmlArray("Children")]
        public Composition[] Children = new Composition[0];
        public string Name;

        public string Path;

        public override string ObjName => Name;
        override public void apply(Document doc)
        {
            Document cur_doc;
            if (Mode == BlobMode.Layer)
                cur_doc = doc.OpenSmartLayer(LayerName);
            foreach (var item in Children)
            {
                item.apply(doc);
            }
        }
        override public void addChild(Composition child)
        {
            var children = Children.ToHashSet();
            children.Add(child);
            Children = children.ToArray();
        }
        override public void removeChild(Composition child)
        {
            var children = Children.ToHashSet();
            children.Remove(child);
            Children = children.ToArray();
        }
        override public Composition[] getChildren()
        {
            return Children;
        }
        override public T[] getChildren<T>()
        {
            return Children.Where(l => l is T).Cast<T>().ToArray();
        }
        public static Blob PathBlob(string path)
        {
            return new Blob(
                BlobMode.Path,
                System.IO.Path.GetFileNameWithoutExtension(path),
                null,
                path
                );
        }
        public static Blob LayerBlob(string layername)
        {
            return new Blob(
                BlobMode.Layer,
                layername,
                layername,
                null
                );
        }
        Blob(BlobMode mode, string name, string layername, string path)
        {
            Mode = mode;
            Name = name;
            LayerName = layername;
            Path = path;
        }
        public Blob()
        {
        }
    }
    [Serializable]
    [XmlRoot("Prototype")]
    public class PrototypeLeaf : Composition
    {
        public override string UIName => "Прототип";
        [XmlIgnore]
        public Blob Blob;
        public string LayerName;
        public string RelativeLayerName;
        public override string ObjName => LayerName;
        public PrototypeLeaf(string layername, string rel_layer_name)
        {
            LayerName = layername;
            RelativeLayerName = rel_layer_name;
        }
        public PrototypeLeaf()
        {
        }
    }
    [Serializable]
    [XmlRoot("Placeholder")]
    public class PlaceholderLeaf : Composition
    {

        public override string UIName => "Плейс.";
        public string LayerName;
        [XmlIgnore]
        public PrototypeLeaf PrototypeLeaf {
            get
            {
                return Parent.getChildren<PrototypeLeaf>().Where(p => p.LayerName == PrototypeLayerName).ToArray()[0];
            }
            set
            {
                PrototypeLayerName = value.LayerName;
            }
        }
        public string PrototypeLayerName;
        DerivedLayerLeaf derived;
        public override string ObjName => LayerName;

        public PlaceholderLeaf(string layername, string prototypeLayername)
        {
            LayerName = layername;
            PrototypeLayerName = prototypeLayername;
        }
        public PlaceholderLeaf() { }
        public override void restoreParents(Composition parent = null)
        {
            base.restoreParents(parent);
            if (derived == null)
                parent.addChild(derived = new DerivedLayerLeaf($"{PrototypeLayerName}_{LayerName}"));
        }
    }
    public class DerivedLayerLeaf : LayerLeaf
    {
        public override string UIName => "Произв.";
        public DerivedLayerLeaf(string layername)
        {
            LayerName = layername;
        }
    }

    [Serializable]
    [XmlRoot("Layer")]
    public class LayerLeaf : LayerComposition
    {
        public override string UIName => "Слой";

        public LayerLeaf(string layername)
        {
            LayerName = layername;
        }
        public LayerLeaf() { }
    }
}

