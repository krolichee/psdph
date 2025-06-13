using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Views.SimpleView.Logic
{
    public class SimpleListData:ISerializable
    {
        [XmlIgnore]
        public Blob RootBlob;
        public ObservableCollection<SimpleData> Variants=new ObservableCollection<SimpleData>();

        public SimpleListData(Blob blob)
        {
            RootBlob = blob;
        }
        public void New()=>
            Variants.Add(new SimpleData(this));
        public void Remove(SimpleData item) =>
            Variants.Remove(item);

        internal void Restore(Blob root)
        {
            RootBlob = root;
            foreach (var item in Variants)
            {
                item.Restore(this);
            }
        }
        public SimpleListData() { }
    }
}
