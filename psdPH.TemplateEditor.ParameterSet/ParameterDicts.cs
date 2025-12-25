using psdPH.Logic.Parameters;
using System;
using System.Collections.Generic;

namespace psdPH.TemplateEditor.Parameters
{
    public static class ParameterDicts
    {
        public delegate IBatchParameterCreator CreateParameter();
        public static Dictionary<Type, CreateParameter>
            CreatorDict = new Dictionary<Type, CreateParameter>
            (){
        { typeof(FlagParameter), () =>new ParameterCreator<FlagParameter>()},
        { typeof(StringParameter),() => new ParameterCreator<StringParameter>()},
        
        { typeof(StringChooseParameter), () =>new ParameterCreator<StringChooseParameter>()},
            };
    }
}
