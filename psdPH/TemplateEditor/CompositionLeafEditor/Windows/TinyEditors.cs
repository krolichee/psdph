using Photoshop;
using psdPH.Logic;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                return p_w.Applied ? result: null;
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
                var rel_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Слой вставки");
                var prototype_parameter = Parameter.Choose(prototype_pconfig,root.getChildren<Prototype>().Select(p=>p.LayerName).ToArray());
                var rel_parameter = Parameter.Choose(rel_pconfig, doc.GetLayersNames());
                p_w = new ParametersWindow(new[] { prototype_parameter, rel_parameter });
            }
        }
        public class TextLeafEditor : TinyEditor<TextLeaf>
        {
            public TextLeafEditor(Document doc, CompositionEditorConfig config) : base(config)
            {
                result.LayerName = "";
                var ln_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Слой");
                string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(config.Kinds));
                p_w = new ParametersWindow(new[] { Parameter.Choose(ln_pconfig, layers_names) });
            }
        }
        public class PrototypeEditor : TinyEditor<Prototype>
        {
            public PrototypeEditor(Document doc, CompositionEditorConfig config,Composition root) : base(config)
            {
                string[] blobs_names = root.getChildren<Blob>().Select(b => b.LayerName).ToArray();
                var bn_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Поддокумент");
                var bn_parameter = Parameter.Choose(bn_pconfig,blobs_names);

                string[] rel_layers_names = PhotoshopDocumentExtension.GetLayersNames(
                    doc.GetLayersByKinds(config.Kinds));
                var rel_pconfig = new ParameterConfig(result, nameof(result.RelativeLayerName), "Опорный слой");
                var rel_parameter = Parameter.Choose(bn_pconfig, rel_layers_names);
                p_w = new ParametersWindow(new[] { bn_parameter, rel_parameter });
            }
        }
    }
}
