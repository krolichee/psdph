using Photoshop;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [UIName("Поддокумент")]
    public class Blob : LayerComposition,CoreComposition
    {
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                if (!IsPrototyped())
                    foreach (var item in Children)
                        result.AddRange(item.Setups);
                return result.ToArray();
            }
        }
        public BlobMode Mode;
        
        public string Path;
        public bool IsPrototyped()
        {
            if (Parent == null)
                return false;
            if (!Parent.GetChildren<Blob>().Contains(this))
                return false;
            var prototypes = Parent.GetChildren<PrototypeLeaf>();
            bool result = prototypes.Any(p => p.LayerName == LayerName);
            return result;
        }
        public override string ObjName => Mode == BlobMode.Layer ? LayerName : System.IO.Path.GetFileNameWithoutExtension(Path);

        protected Composition[] CoreChildren => Children.Where(item => (item is CoreComposition)).ToArray();
        protected Composition[] NonCoreChildren => Children.Where(item => !(item is CoreComposition)).ToArray();

        

        Document DocOfThis(Document doc)
        {
            if (Mode == BlobMode.Layer)
                return doc.OpenSmartLayer(LayerName);
            else
                return doc;
        }
        override public void Apply(Document doc)
        {
            if (IsPrototyped())
                return;
            Document cur_doc = DocOfThis(doc);

            CoreApply();
            NonCoreApply(cur_doc);

            if (Mode == BlobMode.Layer)
                cur_doc.Close(PsSaveOptions.psSaveChanges);
        }

        public void CoreApply()
        {
            if (IsPrototyped())
                return;
            foreach (CoreComposition item in CoreChildren)
                item.CoreApply();
            RuleSet.CoreApply();
        }
        public void NonCoreApply(Document doc)
        {
            foreach (var item in Children)
                item.Apply(doc);
            RuleSet.NonCoreApply(doc);
        }
        override public void AddChild(Composition child)
        {
            child.Parent = this;
            var children = Children.ToHashSet();
            children.Add(child);
            Children = children.ToArray();
            invokeChildrenEvent();
        }
        override public void RemoveChild(Composition child)
        {
            var children = Children.ToHashSet();
            children.Remove(child);
            Children = children.ToArray();
            invokeChildrenEvent();
        }
        override public Composition[] GetChildren()
        {
            return Children;
        }
        override public T[] GetChildren<T>()
        {
            return Children.Where(l => l is T).Cast<T>().ToArray();
        }
        public static Blob PathBlob(string path)
        {
            return new Blob(
                BlobMode.Path,
                null,
                path
                );
        }
        public static Blob LayerBlob(string layername)
        {
            return new Blob(
                BlobMode.Layer,
                layername,
                null
                );
        }
        Blob(BlobMode mode, string layername, string path)
        {
            Mode = mode;
            LayerName = layername;
            Path = path;
        }
        public Blob Clone()
        {
            Blob result = CloneConverter.Clone(this) as Blob;
            result.Restore(Parent as Blob);
            return result;
        }
        public override bool IsMatching(Document doc)
        {
            if (Mode == BlobMode.Layer)
                return LayerDescriptor.Layer(LayerName).DoesDocHas(doc);
            else
                return true;

        }
        public override MatchingResult IsMatchingRouted(Document doc)
        {
            MatchingResult result = new MatchingResult(this,IsMatching(doc));
            
            if (!result)
                return result;
            var cur_doc = DocOfThis(doc);
            foreach (var child in Children)
            {
                var r = child.IsMatchingRouted(cur_doc);
                result.Match &= r;
                
                if (!result)
                {
                    result.MismatchRoute.AddRange(r.MismatchRoute);
                    break;
                }
            }
            if (Mode == BlobMode.Layer)
                cur_doc.Close(PsSaveOptions.psSaveChanges);
            return result;
        }

        public Blob() : base()
        {

        }
    }
}
