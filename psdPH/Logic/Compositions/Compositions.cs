﻿using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace psdPH
{
    [Serializable]
    [XmlInclude(typeof(Blob))]

    [XmlInclude(typeof(FlagLeaf))]
    [XmlInclude(typeof(PrototypeLeaf))]
    [XmlInclude(typeof(PlaceholderLeaf))]

    [XmlInclude(typeof(LayerComposition))]


    [XmlInclude(typeof(Rule))]
    public abstract class Composition : ISetupable
    {
        public delegate void RulesetUpdated();
        public delegate void ChildrenUpdated();
        public event ChildrenUpdated ChildrenUpdatedEvent;
        public event RulesetUpdated RulesetUpdatedEvent;
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
        protected T[] Siblings<T>() where T:Composition
        {
            if (Parent == null)
                return new Composition[0] as T[];
            return Parent.getChildren<T>().ToArray();
        }
        public abstract Parameter[] Setups { get; }

        abstract public void Apply(Document doc);
        protected void invokeChildrenEvent()
        {
            ChildrenUpdatedEvent?.Invoke();
        }
        virtual public void addChild(Composition child) { }
        virtual public void removeChild(Composition child) { }
        public void Restore(Composition parent = null)
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
                    item.Restore(this);
        }
        virtual public Composition[] getChildren() { return null; }
        virtual public T[] getChildren<T>() { return null; }
        public RuleSet RuleSet = new RuleSet();

        public Composition() { ChildrenUpdatedEvent += () => Restore(); RuleSet.Updated += () => RulesetUpdatedEvent?.Invoke(); }
    }

    [Serializable]
    [XmlRoot("Flag")]
    public partial class FlagLeaf : Composition
    {
        public override string UIName => "Флаг";
        public bool Toggle;
        public string Name;
        public override string ObjName => Name;

        public override Parameter[] Setups
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
        public FlagLeaf() : base() { }

        public override void Apply(Document doc) { }
    }

    [Serializable]
    [XmlRoot("PrototypeLeaf")]
    public class PrototypeLeaf : Composition
    {
        Blob blob;
        [XmlIgnore]
        public Blob Blob
        {
            get
            {
                if (blob == null)
                    blob = Parent.getChildren<Blob>().First(b => b.LayerName == LayerName); return blob;
            }
            set { LayerName = value.LayerName; }
        }
        public override string UIName => "Прототип";
        public string RelativeLayerName;
        public string LayerName;
        public Vector GetRelativeLayerAlightmentVector(Document doc)
        {
            return doc.GetAlightmentVector(RelativeLayerName, LayerName);
        }
        public override Parameter[] Setups => new Parameter[0];
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
    
    
}

