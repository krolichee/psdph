using psdPH.Logic;
using psdPH.Logic.Parameters;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.Creators.ParameterCreators
{
    public class ParameterCreator<T> : IBatchParameterCreator where T :Parameter,new()
    {
        protected T _result;
        protected ParametersInputWindow p_w;
        public Parameter[] GetResultBatch()
        {
            return p_w.Applied ? new T[] { _result } : new Parameter[0];
        }
        public bool? ShowDialog()
        {
            return p_w.ShowDialog();
        }
        public virtual Setup[] GetSetups() => new Setup[] { getNameSetup(_result) };
        public ParameterCreator()
        {
            
            _result = new T();
            _result.Name = "";
            p_w = new ParametersInputWindow(GetSetups());
        }
        protected Setup getNameSetup(T par)
        {
            var nameConfig = new SetupConfig(par, nameof(par.Name), "Имя параметра");
            return Setup.StringInput(nameConfig);
        }
    }
}
