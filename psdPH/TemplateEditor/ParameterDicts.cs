using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Creators.ParameterCreators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.TemplateEditor
{
    public static class ParameterDicts
    {
        public delegate IBatchParameterCreator CreateParameter();
        public static Dictionary<Type, CreateParameter>
            CreatorDict = new Dictionary<Type, CreateParameter>
            (){
        { typeof(StringParameter),() => new ParameterCreator<StringParameter>()},
        { typeof(FlagParameter), () =>new ParameterCreator<FlagParameter>()}
            };
    }
}
