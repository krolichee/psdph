using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;

namespace psdPH
{
    public class CompositeRule
    {

    }
    public class CopyingRule:CompositeRule { }
    public class RuleSet
    {

    }
    public abstract class Composition
    {
        protected static string _xmlTag = "";
        protected static string _uiTag = "";
        public string XmlName { get { return _xmlTag; } }
        public string UIName { get { return _uiTag; } }
        abstract public string ObjName { get; }
        private Composition _parent = null;
        public Composition getParent()
        {
            return _parent;
        }
        virtual public void apply(XmlDocument xmlDoc, XmlElement xmlEl) { }
        virtual public void addChild(Composition child) { }
        virtual public void removeChild(Composition child) { }
        virtual public List<Composition> getChildren() { return null; }
        public RuleSet getRules() { return null; }

        public Composition() { }
    }
    public class FlagLeaf : Composition
    {
        private bool _toggle;
        private string _name;
        public override string ObjName => _name;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {

            XmlElement flag = xmlDoc.CreateElement(XmlName);
            flag.SetAttribute("name", _name);
            flag.SetAttribute("is", _toggle.ToString());
            root_elem.AppendChild(flag);
        }
        public FlagLeaf()
        {
            _xmlTag = "flag";
            _uiTag = "Флаг";
        }
    }
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
            _xmlTag = "tph";
            _uiTag = "Изобр.";
        }
    }
    class VisLeaf : Composition
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
            _xmlTag = "vis";
            _uiTag = "Видим.";

        }
    }
    public class TextLeaf : Composition
    {
        private string _text;
        private string _layer_name;

        public override string ObjName => _layer_name;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            
            XmlElement tph = xmlDoc.CreateElement(_xmlTag);
            tph.SetAttribute("ln", _layer_name);
            XmlElement text = xmlDoc.CreateElement("text");
            text.InnerText = _text;
            root_elem.AppendChild(tph);
        }
        public TextLeaf(string text, string layer_name)
        {
            _xmlTag = "tph";
            _uiTag = "Текст";
            _text = text;
            _layer_name = layer_name;
        }
    }
    public enum BlobMode
    {
        Layer,
        Path
    }
    public class Blob : Composition
    {
        private string _name = "";
        private string _psd_path;
        private string _layer_name;
        private BlobMode _mode;
        List<Composition> children = new List<Composition>();
        RuleSet ruleset = null;

        public override string ObjName => _name;
        public string Name { get { return _name; } set { _name = value; } }
        public string LayerName => _layer_name;
        public string Path { get { return _psd_path; } set { _psd_path = value; } }
        public BlobMode Mode => _mode;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            XmlElement blobEl = xmlDoc.CreateElement(XmlName);
            blobEl.SetAttribute("path", _name);
            root_elem.AppendChild(blobEl);
            foreach (var child in children)
            {
                child.apply(xmlDoc, blobEl);
            }

        }
        override public void addChild(Composition child) => children.Add(child);
        override public void removeChild(Composition child) { children.Remove(child); }
        override public List<Composition> getChildren() { return children; }
        public Blob(string indiv, BlobMode mode)
        {
            //----static----
            _xmlTag = "blob";
            _uiTag = "Подфайл";
            //--------------
            _mode = mode;
            if (_mode == BlobMode.Layer) {
                _name = System.IO.Path.GetFileNameWithoutExtension(indiv);
                _layer_name = indiv;
                _psd_path = null;
            }
            if (_mode == BlobMode.Path)
            {
                _name = System.IO.Path.GetFileNameWithoutExtension(indiv);
                _layer_name = null;
                _psd_path = indiv;
            }
        }
    }
    public class PlaceholderLeaf : Composition
    {
        
        private string _layer_name;
        public string LayerName => _layer_name;
        public override string ObjName => throw new NotImplementedException();
        public PlaceholderLeaf(string layername)
        {
            _layer_name = layername;
        }
    }
}
