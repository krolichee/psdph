using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
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
    [XmlInclude(typeof(Blob))]

    [XmlInclude(typeof(FlagLeaf))]
    [XmlInclude(typeof(PrototypeLeaf))]
    [XmlInclude(typeof(PlaceholderLeaf))]
    
    [XmlInclude(typeof(ImageLeaf))]
    [XmlInclude(typeof(TextLeaf))]

    [XmlInclude(typeof(LayerLeaf))]
    [XmlInclude(typeof(GroupLeaf))]

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

        abstract public void Apply(Document doc);
        virtual public void addChild(Composition child) { }
        virtual public void removeChild(Composition child) { }
        public void Restore(Blob parent = null)
        {
            restoreParents(parent);
            RuleSet.restoreLinks(this);
        }
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
        public RuleSet RuleSet = new RuleSet();

        public Composition() { }
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

        public override void Apply(Document doc){ }
    }

    [Serializable]
    [XmlRoot("PrototypeLeaf")]
    public class PrototypeLeaf : Composition
    {
        Blob blob;
        [XmlIgnore]
        public Blob Blob { get { 
                if (blob == null) 
                    blob = Parent.getChildren<Blob>().First(b => b.LayerName == LayerName); return blob; } }
        public override string UIName => "Прототип";
        public string RelativeLayerName;
        public string LayerName;
        public Point GetRelativeLayerCompensationShift(Document doc)
        {
            return doc.GetRelativeLayerShift(RelativeLayerName,LayerName);
        }
        public override Parameter[] Parameters => new Parameter[0];
        public override string ObjName => Blob.LayerName;
        public override void Apply(Document doc)
        {
            doc.GetLayerByName(Blob.LayerName).Opacity = 0;
        }
        ~PrototypeLeaf()
        {
            //var placeholders = Parent.getChildren<PlaceholderLeaf>().Where(p=>p.PrototypeLayerName==LayerName);
            //foreach (var item in placeholders)
            //{
            //    Parent.removeChild(item);
            //}
        }

        public PrototypeLeaf(Blob blob, string rel_layer_name)
        {
            this.blob = blob;
            LayerName = blob.LayerName;
            RelativeLayerName = rel_layer_name;
        }
        public PrototypeLeaf() { }
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
        public PrototypeLeaf Prototype
        {
            get
            {
                return Parent.getChildren<PrototypeLeaf>().First(p => p.LayerName == PrototypeLayerName);
            }
            set
            {
                PrototypeLayerName = value.LayerName;
            }
        }
        public string PrototypeLayerName;
        string derivedLayerName;
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
            derivedLayerName = $"{PrototypeLayerName}_{LayerName}";
            ArtLayer ph_layer = doc.GetLayerByName(LayerName);
            ArtLayer new_layer = doc.CloneSmartLayer(PrototypeLayerName);
            var prototypeCShift = Prototype.GetRelativeLayerCompensationShift(doc);
            var cShift = doc.GetRelativeLayerShift(ph_layer, new_layer);
            new_layer.Translate(cShift.X,cShift.Y);
            new_layer.Translate(prototypeCShift.X, prototypeCShift.Y);
            //ph_layer.Delete();
            ph_layer.Opacity = 0;
            new_layer.Name = blob.LayerName = derivedLayerName;
            Parent.addChild(blob);
            Parent.removeChild(this);
        }

        public override void Apply(Document doc){ }
    }
}

