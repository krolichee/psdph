using psdPH.Logic.Compositions;
using psdPH.Views.SimpleView.Logic;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Views
{
     public abstract class ViewListData
    {
        [XmlIgnore]
        public Blob RootBlob;
        public abstract void New();
    }
}
