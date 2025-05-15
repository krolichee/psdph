using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Views.SimpleView.Logic
{
    [Serializable]
    public class SimpleListData: ViewListData<Blob>
    {
        public SimpleListData() { }
        public override void New()
        {
            Variants.Add(RootBlob.Clone());
        }

    }
}
