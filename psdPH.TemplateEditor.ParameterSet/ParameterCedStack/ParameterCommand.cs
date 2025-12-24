using psdPH.CED;
using psdPH.Parameters;
using System;

namespace psdPH.TemplateEditor
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
