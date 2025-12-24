using psdPH.Photoshop;
using psdPH.Setups;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.TemplateEditor
{
    public class LayerSetup
    {
        public static LayerDescriptor[] GetFilteredLDs(DocumentWr docWr, LDFilter ldFilter)
        {
           return ldFilter.Filter(LayerDescriptor.GetLayerDescriptors(docWr));
        }
        public static Setup getLayerChooseSetup(DocumentWr docWr, LDFilter ldFilter, ReflectionConfig config)
        {
            var lds = GetFilteredLDs(docWr,ldFilter);
            return new ChooseSetup(config, lds);
        }
    }
}
