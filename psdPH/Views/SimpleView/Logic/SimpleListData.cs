using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Views.SimpleView.Logic
{
    public class SimpleListData
    {
        public Blob RootBlob;
        public ObservableCollection<Blob> Variants;
        public void New()
        {
            Variants.Add(RootBlob.Clone());
        }
        public void Remove(Blob item)
        {
            Variants.Remove(item);
        }

        internal void Restore()
        {
            RootBlob.Restore();
            foreach (var item in Variants)
            {
                item.Restore();
            }
        }
    }
}
