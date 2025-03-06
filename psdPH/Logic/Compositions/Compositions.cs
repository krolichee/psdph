using Photoshop;
using psdPH.Logic;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    [XmlInclude(typeof(TextLeaf))]
    [XmlInclude(typeof(Blob))]
    [XmlInclude(typeof(Prototype))]

    [XmlInclude(typeof(PlaceholderLeaf))]
    [XmlInclude(typeof(LayerLeaf))]
    [XmlInclude(typeof(Rule))]
    public abstract class Composition : IParameterable
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

        public abstract Parameter[] Parameters { get; }

        abstract public void apply(Document doc);
        virtual public void addChild(Composition child) { }
        virtual public void removeChild(Composition child) { }
        public void Restore(Blob parent = null)
        {
            restoreParents(parent);
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
        public override string UIName => "Флаг";
        public bool Toggle;
        public string Name;
        public override string ObjName => Name;

        public override Parameter[] Parameters
        {
            get
            {
                var result = new List<Parameter>();
                var toggleConfig = new ParameterConfig(this, nameof(this.Toggle), Name);
                result.Add(Parameter.Check(toggleConfig));
                return result.ToArray();
            }
        }

        public FlagLeaf(string name)
        {
            Name = name;
        }
        public FlagLeaf()
        {
        }

        public override void apply(Document doc){ }
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

        public override void apply(Document doc)
        {
            throw new NotImplementedException();
        }
    }
    [Serializable]
    [XmlRoot("Text")]
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



        override public void apply(Document doc)
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
    public enum BlobMode
    {
        Layer,
        Path
    }

    [Serializable]
    [XmlRoot("Blob")]
    public class Blob : LayerComposition
    {
        public override Parameter[] Parameters
        {
            get
            {
                var result = new List<Parameter>();
                if (!IsPrototyped())
                    foreach (var item in Children)
                        result.AddRange(item.Parameters);
                return result.ToArray();
            }
        }
        public override string UIName => "Подфайл";
        public BlobMode Mode;
        [XmlArray("Children")]
        public Composition[] Children = new Composition[0];
        public string Name;
        public string Path;
        public bool IsPrototyped()
        {
            if (Parent == null)
                return false;
            var prototypes = Parent.getChildren<Prototype>();
            bool result = prototypes.Any(p => p.Blob == this);
            return result;
        }
        public override string ObjName => Name;
        override public void apply(Document doc)
        {
            Document cur_doc;
            if (Mode == BlobMode.Layer)
                cur_doc = doc.OpenSmartLayer(LayerName);
            else
                cur_doc = doc;
            foreach (var item in Children)
                item.apply(cur_doc);
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
        public Blob Clone()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Composition));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, this);
            StringReader sr = new StringReader(sb.ToString());
            Blob result = serializer.Deserialize(sr) as Blob;
            result.Parent = Parent;
            result.Restore();
            return result;
        }
        public Blob()
        {
        }
    }
    [Serializable]
    [XmlRoot("Prototype")]
    public class Prototype : Composition
    {
        Blob blob;
        [XmlIgnore]
        public Blob Blob { get { if (blob == null) blob = Parent.getChildren<Blob>().First(b => b.LayerName == LayerName); return blob; } }
        public override string UIName => "Прототип";
        public string RelativeLayerName;
        public string LayerName;
        public Point GetRelativeLayerShift(Document doc)
        {
            ArtLayer relativeLayer = doc.GetLayerByName(RelativeLayerName);
            ArtLayer prototypeLayer = doc.GetLayerByName(Blob.LayerName);
            return new Point(
                prototypeLayer.Bounds[0] - relativeLayer.Bounds[0],
                prototypeLayer.Bounds[1] - relativeLayer.Bounds[1]
                );
        }
        public override Parameter[] Parameters => new Parameter[0];
        public override string ObjName => Blob.LayerName;
        public override void apply(Document doc)
        {
            doc.GetLayerByName(Blob.LayerName).Opacity = 0;
        }

        

        public Prototype(Blob blob, string rel_layer_name)
        {
            this.blob = blob;
            LayerName = blob.LayerName;
            RelativeLayerName = rel_layer_name;
        }
        public Prototype() { }
    }
    [Serializable]
    [XmlRoot("Placeholder")]
    public class PlaceholderLeaf : Composition
    {
        public override string UIName => "Плейс.";
        public string LayerName;
        [XmlIgnore]
        public Blob ActualBlob;
        [XmlIgnore]
        public Prototype Prototype
        {
            get
            {
                return Parent.getChildren<Prototype>().First(p => p.LayerName == PrototypeLayerName);
            }
            set
            {
                PrototypeLayerName = value.LayerName;
            }
        }
        public string PrototypeLayerName;
        DerivedLayerLeaf derived;
        public override string ObjName => LayerName;

        public override Parameter[] Parameters => new Parameter[0];

        public PlaceholderLeaf(string layername, string prototypeLayername)
        {
            LayerName = layername;
            PrototypeLayerName = prototypeLayername;
        }
        public PlaceholderLeaf() { }
        public override void restoreParents(Composition parent = null)
        {
            base.restoreParents(parent);
        }
        internal void ReplaceWithFiller(Document doc, Blob blob)
        {
            derived = new DerivedLayerLeaf($"{PrototypeLayerName}_{LayerName}");
            ArtLayer ph_layer = doc.GetLayerByName(LayerName);
            ArtLayer new_layer = doc.GetLayerByName(PrototypeLayerName).Duplicate();
            ph_layer.Delete();
            new_layer.Name = blob.LayerName = derived.LayerName;
            Parent.addChild(blob);
            Parent.removeChild(this);
        }

        public override void apply(Document doc){ }
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

        public override Parameter[] Parameters => new Parameter[0];

        public LayerLeaf(string layername)
        {
            LayerName = layername;
        }
        public LayerLeaf() { }

        public override void apply(Document doc){}
    }
}

