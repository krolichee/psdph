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

namespace psdPH
{
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
        virtual public void apply(XmlDocument xmlDoc, XmlElement root_elem) { }
        virtual public void addChild(Composition child) { }
        virtual public void removeChild(Composition child) { }
        virtual public void restoreParents(Composition parent = null)
        {
            if (parent != null)
                Parent = parent;
            if (getChildren() != null)
                foreach (var item in getChildren())
                    item.restoreParents(this);
        }
        virtual public Composition[] getChildren() { return null; }
        virtual public T[] getChildren<T>() { return null; }
        public RuleSet getRules() { return null; }

        public Composition() { }
    }
    [Serializable]
    [XmlRoot("Flag")]
    public class FlagLeaf : Composition
    {
        public override string UIName => "Флаг";
        public bool Toggle;
        public string Name;
        public override string ObjName => Name;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {

            XmlElement flag = xmlDoc.CreateElement(XmlName);
            flag.SetAttribute("name", Name);
            flag.SetAttribute("is", Toggle.ToString());
            root_elem.AppendChild(flag);
        }
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
    public class ImageLeaf : Composition
    {
        private string _path;
        private string _layer_name;
        public override string UIName => "Изобр.";

        public override string ObjName => _layer_name;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {

            XmlElement img = xmlDoc.CreateElement(XmlName);
            img.SetAttribute("ln", _layer_name);
            img.SetAttribute("path", _path.ToString());
            root_elem.AppendChild(img);
        }
        public ImageLeaf()
        {
        }
    }
    [Serializable]
    [XmlRoot("Visibility")]
    public class VisLeaf : Composition
    {
        public override string UIName => "Видим.";
        private bool _toggle;
        private string _layer_name;
        public override string ObjName => _layer_name;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            XmlElement img = xmlDoc.CreateElement(XmlName);
            img.SetAttribute("ln", _layer_name);
            img.SetAttribute("is", _toggle.ToString());
            root_elem.AppendChild(img);
        }
        public VisLeaf()
        {

        }

    }
    [Serializable]
    [XmlRoot("Text")]
    public class TextLeaf : Composition
    {
        public override string UIName => "Текст";
        public string Text;
        public string LayerName;
        public override string ObjName => LayerName;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {

            XmlElement tph = xmlDoc.CreateElement(XmlName);
            tph.SetAttribute("ln", LayerName);
            XmlElement text = xmlDoc.CreateElement("text");
            text.InnerText = Text;
            root_elem.AppendChild(tph);
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
    public class Blob : Composition
    {
        public override string UIName => "Подфайл";
        public BlobMode Mode;
        [XmlArray("Children")]
        public Composition[] Children = new Composition[0];
        public string Name;
        public string LayerName;
        public string Path;

        public override string ObjName => Name;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            XmlElement blobEl = xmlDoc.CreateElement(XmlName);
            blobEl.SetAttribute("path", Name);
            root_elem.AppendChild(blobEl);
            foreach (var child in Children)
            {
                child.apply(xmlDoc, blobEl);
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

            return Children.Where(l => l.GetType() == typeof(T)).Cast<T>().ToArray();
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
        Blob(BlobMode mode,string name,string layername,string path)
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
    [XmlRoot("Placeholder")]
    public class PlaceholderLeaf : Composition
    {
        public override string UIName => "Плейс.";
        public string LayerName;
        public string PrototypeLayerName;
        public override string ObjName => LayerName;
        public PlaceholderLeaf(string layername)
        {
            LayerName = layername;
        }
        public PlaceholderLeaf() { }
    }
    [Serializable]
    [XmlRoot("Prototype")]
    public class PrototypeLeaf : Composition
    {
        public override string UIName => "Прототип";
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
}

