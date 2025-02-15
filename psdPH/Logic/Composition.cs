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
        protected static string _xmlName = "";
        protected static string _uiName = "";
        public string XmlName { get { return _xmlName; } }
        public string UIName { get { return _uiName; } }
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
            _xmlName = "flag";
            _uiName = "Флаг";
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
            _xmlName = "tph";
            _uiName = "Изобр.";
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
            _xmlName = "vis";
            _uiName = "Видим.";

        }
    }
    public class TextLeaf : Composition
    {
        private string _text;
        private string _layer_name;

        public override string ObjName => _layer_name;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            
            XmlElement tph = xmlDoc.CreateElement(_xmlName);
            tph.SetAttribute("ln", _layer_name);
            XmlElement text = xmlDoc.CreateElement("text");
            text.InnerText = _text;
            root_elem.AppendChild(tph);
        }
        public TextLeaf(string text, string layer_name)
        {
            _xmlName = "tph";
            _uiName = "Текст";
            _text = text;
            _layer_name = layer_name;
        }
    }
    public class Blob : Composition
    {
        private string _layer_name = "";
        private string _psd_path;
        List<Composition> children = new List<Composition>();
        RuleSet ruleset = null;

        public override string ObjName => _layer_name;
        public string LayerName => _layer_name;
        public string Path => _psd_path;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            XmlElement blob = xmlDoc.CreateElement(XmlName);
            blob.SetAttribute("path", _layer_name);
            root_elem.AppendChild(blob);
            foreach (var child in children)
            {
                child.apply(xmlDoc, blob);
            }

        }
        override public void addChild(Composition child) => children.Add(child);
        override public void removeChild(Composition child) { children.Remove(child); }
        override public List<Composition> getChildren() { return children; }
        public Blob(string layer_name, string psd_path)
        {
            _xmlName = "blob";
            _uiName = "Подфайл";
            _layer_name = layer_name;
            _psd_path = psd_path;
        }
    }
}
