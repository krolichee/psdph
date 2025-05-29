using Photoshop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
    [Serializable]
    public class Blob : LayerComposition,CoreComposition
    {
        [XmlIgnore]
        public override Parameter[] Setups
        {
            get
            {
                var result = new List<Parameter>();
                if (!IsPrototyped())
                    foreach (var item in Children)
                        result.AddRange(item.Setups);
                return result.ToArray();
            }
        }
        public override string UIName => "Подфайл";
        public BlobMode Mode;
        
        public string Path;
        public bool IsPrototyped()
        {
            if (Parent == null)
                return false;
            if (!Parent.getChildren<Blob>().Contains(this))
                return false;
            var prototypes = Parent.getChildren<PrototypeLeaf>();
            bool result = prototypes.Any(p => p.LayerName == LayerName);
            return result;
        }
        ~Blob()
        {
            //if (Parent == null)
            //    return;
            //var prototypes = Parent.getChildren<PrototypeLeaf>().Where(p => p.LayerName==LayerName);
            //foreach (var item in prototypes)
            //{
            //    Parent.removeChild(item);
            //}
        }
        public override string ObjName => Mode == BlobMode.Layer ? LayerName : System.IO.Path.GetFileNameWithoutExtension(Path);

        protected Composition[] CoreChildren => Children.Where(item => (item is CoreComposition)).ToArray();
        protected Composition[] NonCoreChildren => Children.Where(item => !(item is CoreComposition)).ToArray();
        override public void Apply(Document doc)
        {
            if (IsPrototyped())
                return;
            Document cur_doc=doc;

            if (Mode == BlobMode.Layer)
                cur_doc = doc.OpenSmartLayer(LayerName);

            CoreApply();
            RuleSet.CoreApply();

            NonCoreApply(cur_doc);
            RuleSet.NonCoreApply(cur_doc);

            if (Mode == BlobMode.Layer)
                cur_doc.Close(PsSaveOptions.psSaveChanges);
        }

        public void CoreApply()
        {
            if (IsPrototyped())
                return;
            foreach (CoreComposition item in CoreChildren)
                item.CoreApply();
        }
        public void NonCoreApply(Document doc)
        {
            foreach (var item in Children)
                item.Apply(doc);
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
        override public T[] getChildren<T>()
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
            XmlSerializer serializer = new XmlSerializer(typeof(Composition));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, this);
            StringReader sr = new StringReader(sb.ToString());
            Blob result = serializer.Deserialize(sr) as Blob;
            result.Restore(Parent as Blob);
            return result;
        }

        public Blob() : base()
        {
        }
    }
}
