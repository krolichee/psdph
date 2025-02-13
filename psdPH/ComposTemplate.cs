using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;

namespace psdPH
{
    class CompositeRule
    {

    }
    class CopyingRule:CompositeRule { }
    class RuleSet
    {

    }
    class Composition
    {
        static public readonly string Xml_name;
        private Composition _parent = null;
        public Composition getParent()
        {
            return _parent;
        }
        virtual public void apply(XmlDocument xmlDoc, XmlElement xmlEl) { }
        virtual public void addChild(Composition child) { }
        virtual public void removeChild(Composition child) { }
        public Composition[] getChildren() { return null; }
        public RuleSet getRules() { return null; }

        public Composition() { }
    }
    class FlagLeaf : Composition
    {
        private bool _toggle;
        private string _name;
        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            XmlElement flag = xmlDoc.CreateElement("flag");
            flag.SetAttribute("name", _name);
            flag.SetAttribute("is", _toggle.ToString());
            root_elem.AppendChild(flag);
        }
    }
    class ImageLeaf : Composition
    {
        private string _path;
        private string _layer_name;
        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            XmlElement img = xmlDoc.CreateElement("flag");
            img.SetAttribute("ln", _layer_name);
            img.SetAttribute("path", _path.ToString());
            root_elem.AppendChild(img);
        }
    }
    class VisLeaf : Composition
    {
        private bool _toggle;
        private string _layer_name;

        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            XmlElement img = xmlDoc.CreateElement("flag");
            img.SetAttribute("ln", _layer_name);
            img.SetAttribute("is", _toggle.ToString());
            root_elem.AppendChild(img);
        }
    }
    class TextLeaf : Composition
    {
        private string _text;
        private string _layer_name;
        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            XmlElement tph = xmlDoc.CreateElement("tph");
            tph.SetAttribute("ln", _layer_name);
            XmlElement text = xmlDoc.CreateElement("text");
            text.InnerText = _text;
            root_elem.AppendChild(tph);
        }
    }
    class Compositor : Composition
    {
        const string _xmlName = "blob";
        private string _psd_path = "";
        List<Composition> children = null;
        RuleSet ruleset = null;
        override public void apply(XmlDocument xmlDoc, XmlElement root_elem)
        {
            XmlElement blob = xmlDoc.CreateElement(_xmlName);
            blob.SetAttribute("path", _psd_path);
            root_elem.AppendChild(blob);
            foreach (var child in children)
            {
                child.apply(xmlDoc, blob);
            }

        }
        override public void addChild(Composition child) => children.Append(child);
        override public void removeChild(Composition child) { children.Remove(child); }
    }
}
