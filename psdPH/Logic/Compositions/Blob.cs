using Photoshop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace psdPH.Logic.Compositions
{
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
        public override string ObjName => Mode==BlobMode.Layer?LayerName: System.IO.Path.GetFileNameWithoutExtension(Path);
        override public void Apply(Document doc)
        {
            if (IsPrototyped())
                return;
            if (this.LayerName == "Прототип дня")
                ;
            Document cur_doc;
            if (Mode == BlobMode.Layer)
                cur_doc = doc.OpenSmartLayer(LayerName);
            else
                cur_doc = doc;
            foreach (var item in Children)
                item.Apply(cur_doc);
            foreach (var rule in RuleSet.Rules)
            {
                rule.Apply(cur_doc);
            }
            if (Mode == BlobMode.Layer)
                cur_doc.Close(PsSaveOptions.psSaveChanges);
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
        public Blob()
        {
        }
    }
}
