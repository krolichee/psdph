using psdPH.Logic.Parameters;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Creators.ParameterCreators;
using psdPHTest.Logic.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static psdPH.TemplateEditor.ParameterDicts;
using static psdPH.TemplateEditor.StructureDicts;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.CedStacks.ParameterCedStack
{
    public class ParameterCommand: CEDCommand
    {
        protected ParameterSet ParameterSet;
        public ParameterCommand(ParameterSet parameterSet)
        {
            ParameterSet = parameterSet;
        }
        protected override void CreateExecuteCommand(object parameter)
        {
            Type type = parameter as Type;
            CreateParameter creator_func;
            if (!ParameterDicts.CreatorDict.TryGetValue(type, out creator_func))
                throw new ArgumentException();
            IBatchParameterCreator creator = creator_func();
            if (creator.ShowDialog() != true)
                return;
            ParameterSet.Add(creator.GetResultBatch());
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            ParameterSet.AsCollection().Remove(parameter as Parameter);
        }
    }
}
