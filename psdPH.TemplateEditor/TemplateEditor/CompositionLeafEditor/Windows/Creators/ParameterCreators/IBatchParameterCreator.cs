using psdPH.Logic.Parameters;
using psdPH.Parameters;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.Creators.ParameterCreators
{
    public interface IBatchParameterCreator
    {
        Parameter[] GetResultBatch();
        bool? ShowDialog();
    }
}