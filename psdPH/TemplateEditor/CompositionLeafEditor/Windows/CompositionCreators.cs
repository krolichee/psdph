using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
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
        public abstract class LeafCreator<T> : ICompositionShapitor where T : Composition, new()
        {
            protected T result;
            protected ParametersInputWindow p_w;
            public Composition GetResultComposition()
            {
                return p_w.Applied ? result: null;
            }
            public bool? ShowDialog()
            {
                return p_w.ShowDialog();
            }
            protected LeafCreator()
            {
            result = new T();
            }
    }
        public class FlagLeafCreator : LeafCreator<FlagLeaf>
        {
        public FlagLeafCreator() : base()
            {
                result.Name = "";
                var par_config = new ParameterConfig(result, nameof(result.Name), "Имя флага");
                p_w = new ParametersInputWindow(new[] { Parameter.StringInput(par_config) });
            }
            
        }
        public class PlaceholderLeafCreator : LeafCreator<PlaceholderLeaf>
        {
            public PlaceholderLeafCreator(Document doc,Composition root) : base()
        {
                var prototype_pconfig = new ParameterConfig(result, nameof(result.PrototypeLayerName), "Прототип");
                var rel_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Слой вставки");
                var prototype_parameter = Parameter.Choose(prototype_pconfig,root.getChildren<PrototypeLeaf>().Select(p=>p.LayerName).ToArray());
                var rel_parameter = Parameter.Choose(rel_pconfig, doc.GetLayersNames());
                p_w = new ParametersInputWindow(new[] { prototype_parameter, rel_parameter });
            }
        }
        public class TextLeafCreator : LeafCreator<TextLeaf>
        {
            public TextLeafCreator(Document doc) : base()
        {
                result.LayerName = "";
                var ln_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Слой");
                string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] {PsLayerKind.psTextLayer}));
                p_w = new ParametersInputWindow(new[] { Parameter.Choose(ln_pconfig, layers_names) });
            }
        }
        public class PrototypeCreator : LeafCreator<PrototypeLeaf>
        {
            public PrototypeCreator(Document doc,Composition root) : base()
        {
                string[] blobs_names = root.getChildren<Blob>().Select(b => b.LayerName).ToArray();
                var bn_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Поддокумент");
                var bn_parameter = Parameter.Choose(bn_pconfig,blobs_names);

                string[] rel_layers_names = PhotoshopDocumentExtension.GetLayersNames(
                    doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer}));
                var rel_pconfig = new ParameterConfig(result, nameof(result.RelativeLayerName), "Опорный слой");
                var rel_parameter = Parameter.Choose(rel_pconfig, rel_layers_names);
                p_w = new ParametersInputWindow(new[] { bn_parameter, rel_parameter });
            }
        }
        public class ImageLeafCreator : LeafCreator<ImageLeaf>
        {
            public ImageLeafCreator(Document doc) : base()
        {
                result.LayerName = "";
                var ln_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Слой");
                string[] layers_names = doc.GetLayersNames(doc.GetLayersByKind(PsLayerKind.psNormalLayer));
                p_w = new ParametersInputWindow(new[] { Parameter.Choose(ln_pconfig, layers_names) });
            }
        }
        public class LayerLeafCreator : LeafCreator<LayerLeaf>
        {
            public LayerLeafCreator(Document doc) : base()
        {
                result.LayerName = "";
                var ln_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Слой");
                string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer }));
                p_w = new ParametersInputWindow(new[] { Parameter.Choose(ln_pconfig, layers_names) });
            }
        }
        public class GroupLeafCreator : LeafCreator<GroupLeaf>
        {
            public GroupLeafCreator(Document doc) : base()
        {
                result.LayerName = "";
                var ln_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Группа");
                string[] layers_names = doc.GetLayerSetsNames(doc.GetLayerSets());
                p_w = new ParametersInputWindow(new[] { Parameter.Choose(ln_pconfig, layers_names) });
            }
        }
        public class TextAreaLeafEditor : LeafCreator<TextAreaLeaf>
        {
            public TextAreaLeafEditor(Document doc,Blob root) : base()
        {
                result.LayerName = "";
                string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer }));
                var ln_pconfig = new ParameterConfig(result, nameof(result.LayerName), "Слой");
                var ln_parameter = Parameter.Choose(ln_pconfig, layers_names);

                var textleaf_config = new ParameterConfig(result, nameof(result.TextLeafLayername), "Текст");
                var textleaf_parameter = Parameter.Choose(ln_pconfig, root.getChildren<TextAreaLeaf>().Select(p => p.LayerName).ToArray());
                
                p_w = new ParametersInputWindow(new[] { ln_parameter, textleaf_parameter });
            }
        }
    }

