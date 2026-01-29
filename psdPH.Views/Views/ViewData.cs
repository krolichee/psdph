using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Views.SimpleView.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Views
{
    public abstract class ViewData
    {
        [XmlIgnore]
        public abstract RootBlob RootBlob { get; }
    }
}
