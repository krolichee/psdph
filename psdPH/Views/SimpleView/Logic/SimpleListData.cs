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
    public class SimpleListData:ViewListData
    {

        public ObservableCollection<SimpleData> Variants = new ObservableCollection<SimpleData>();
        public SimpleListData(Blob blob)
        {
            RootBlob = blob;
        }
        public override void New()=>
            Variants.Add(new SimpleData(this));

        internal void Restore(Blob root)
        {
            RootBlob = root;
            foreach (var item in Variants)
            {
                item.Restore(this);
            }
        }
        public void Remove(SimpleData item) => Variants.Remove(item);
        public SimpleListData() { }
    }
}
