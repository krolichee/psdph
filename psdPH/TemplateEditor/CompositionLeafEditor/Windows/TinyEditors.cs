using Photoshop;
using psdPH.Logic;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    class TinyEditors
    {
        public abstract class TinyEditor<T> : ICompositionEditor where T : Composition, new()
        {
            protected T result;
            protected ParametersWindow p_w;
            public Composition GetResultComposition()
            {
                if (p_w.Applied)
                    return result as Composition;
                else
                    return null;
            }
            public bool? ShowDialog()
            {
                return p_w.ShowDialog();
            }
            public TinyEditor(CompositionEditorConfig config)
            {
                if (config.Composition == null)
                    result = new T();
                else
                    result = config.Composition as T;
            }
        }
        public class FlagEditor : TinyEditor<FlagLeaf>
        {
            public FlagEditor(CompositionEditorConfig config):base(config)
            {
                result.Name = "";
                var par_config = new ParameterConfig(result, nameof(result.Name), "Имя флага");
                p_w = new ParametersWindow(new[] { Parameter.StringInput(par_config) });
            }
            
        }
        public class PlaceholderLeafEditor : TinyEditor<PlaceholderLeaf>
        {
            public PlaceholderLeafEditor(Document doc, CompositionEditorConfig config,Composition root) :base(config){
                var prototype_pconfig = new ParameterConfig(result, nameof(result.PrototypeLayerName), "Прототип");
                var rel_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Опорный слой");
                var prototype_parameter = Parameter.Choose(prototype_pconfig,root.getChildren<Prototype>().Select(p=>p.LayerName).ToArray());
                var rel_parameter = Parameter.Choose(rel_pconfig, doc.GetLayersNames( doc.GetLayersByKinds(config.Kinds)));
                p_w = new ParametersWindow(new[] { prototype_parameter, rel_parameter });
            }
        }
    }
}
