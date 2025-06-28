using psdPH.Logic;
using psdPH.Logic.Parameters;
using psdPH.Parameters;
using psdPH.Setups;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Utils.Setups;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.Creators.ParameterCreators
{
    public class ParameterCreator<T> : IBatchParameterCreator where T :Parameter,new()
    {
        protected T _result;
        protected SetupsInputWindow p_w;
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
            p_w = new SetupsInputWindow(GetSetups());
        }
        protected Setup getNameSetup(T par)
        {
            var nameConfig = new ReflectionConfig(par, nameof(par.Name), "Имя параметра");
            return new StringInputSetup(nameConfig);
        }
    }
}
