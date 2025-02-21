using psdPH.Logic;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Serialization;

namespace psdPH
{
    public abstract class Composition
    {
        protected static string _uiTag = "";
        public string XmlName
        {
            get
            {
                Type type = this.GetType();
                XmlRootAttribute rootAttribute = (XmlRootAttribute)Attribute.GetCustomAttribute(type, typeof(XmlRootAttribute));
                return rootAttribute.ElementName;
            }
        }
        public string UIName { get { return _uiTag; } }
        abstract public string ObjName { get; }
        [XmlIgnore]
        virtual public Composition Parent { get; set; }
        virtual public void apply(XmlDocument xmlDoc, XmlElement xmlEl) { }
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
        virtual public Composition[] getChildren(Type type) { return null; }
        public RuleSet getRules() { return null; }

        public Composition() { }
    }
    [Serializable]
    [XmlRoot("Flag")]
    public class FlagLeaf : Composition
    {
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
            _uiTag = "Флаг";
        }
    }
    [Serializable]
    [XmlRoot("Image")]
    public class ImageLeaf : Composition
    {
        private string _path;
        private string _layer_name;


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
            _uiTag = "Изобр.";
        }
    }
    [Serializable]
    [XmlRoot("Visibility")]
    public class VisLeaf : Composition
    {
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
            _uiTag = "Видим.";

        }

    }
    [Serializable]
    [XmlRoot("Text")]
    public class TextLeaf : Composition
    {
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
            _uiTag = "Текст";
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
        override public Composition[] getChildren(Type type)
        {

            return Children.Where(l => l.GetType() == type).ToArray();
        }
        public Blob(string indiv, BlobMode mode) : this()
        {
            //--------------
            Mode = mode;
            if (Mode == BlobMode.Layer)
            {
                Name = indiv;
                LayerName = indiv;
                Path = null;
            }
            if (Mode == BlobMode.Path)
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(indiv);
                LayerName = null;
                Path = indiv;
            }
        }
        public Blob()
        {
            _uiTag = "Подфайл";
        }
    }
    [Serializable]
    [XmlRoot("Placeholder")]
    public class PlaceholderLeaf : Composition
    {
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
            _uiTag = "Прототип";
        }
    }
}

