using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public abstract class SingleLeafCreator<T> : IBatchCompositionCreator where T : Composition, new()
    {
        protected T result;
        protected SetupsInputWindow p_w;
        public bool? ShowDialog()
        {
            return p_w.ShowDialog();
        }

        public Composition[] GetResultBatch()
        {
            return p_w.Applied ? new Composition[] { result } : new Composition[0];
        }

        protected SingleLeafCreator()
        {
            result = new T();
        }
    }
    public class TextLeafCreator : SingleLeafCreator<TextLeaf>
    {
        void Single(Document doc)
        {
            result.LayerName = "";
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой");
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psTextLayer }));
            List<Setup> parameters = new List<Setup>();
            var layerParameter = new ChooseSetup(ln_pconfig, layers_names);
            parameters.Add(layerParameter);
            p_w = new SetupsInputWindow(parameters.ToArray());
        }
        public TextLeafCreator(Document doc) : base()
        {
            Single(doc);
        }
    }
    
    [Obsolete]
    public class ImageLeafCreator : SingleLeafCreator<ImageLeaf>
    {
        public ImageLeafCreator(Document doc) : base()
        {
            result.LayerName = "";
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой");
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKind(PsLayerKind.psNormalLayer));
            p_w = new SetupsInputWindow(new[] { new ChooseSetup(ln_pconfig, layers_names) });
        }
    }
    public class LayerLeafCreator : SingleLeafCreator<LayerLeaf>
    {
        public LayerLeafCreator(Document doc) : base()
        {
            result.LayerName = "";
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой");
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer }));
            p_w = new SetupsInputWindow(new[] { new ChooseSetup(ln_pconfig, layers_names) });
        }
    }
    public class GroupLeafCreator : SingleLeafCreator<GroupLeaf>
    {
        public GroupLeafCreator(Document doc) : base()
        {
            result.LayerName = "";
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Группа");
            string[] layers_names = doc.GetLayerSetsNames(doc.GetLayerSets());
            p_w = new SetupsInputWindow(new[] { new ChooseSetup(ln_pconfig, layers_names) });
        }
    }
    public class AreaLeafCreator : SingleLeafCreator<AreaLeaf>
    {
        public AreaLeafCreator(Document doc) : base()
        {
            result.LayerName = "";
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer }));
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой поля");
            var ln_parameter = new ChooseSetup(ln_pconfig, layers_names);

            p_w = new SetupsInputWindow(new[] { ln_parameter});
        }
    }
    public class LayerBlobCreator : SingleLeafCreator<LayerBlob>
    {
        protected Setup getLayerSetup(Document doc)
        {
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSmartObjectLayer }));
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой");
            var ln_setup = new ChooseSetup(ln_pconfig, layers_names);
            return ln_setup;
        }
        public LayerBlobCreator(Document doc, Composition root) : base()
        {
            result.LayerName = "";
            
            p_w = new SetupsInputWindow(new[] { getLayerSetup( doc) });
        }
        protected LayerBlobCreator() { }
    }
    public class PrototypeCreator : LayerBlobCreator
    {
        protected Setup getRelativeLayerSetup(Document doc)
        {
            string[] rel_layers_names = PhotoshopDocumentExtension.GetLayersNames(
                doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer }));
            var rel_pconfig = new SetupConfig(result, nameof(PrototypeBlob.RelativeLayerName), "Опорный слой");
            var rel_setup = new ChooseSetup(rel_pconfig, rel_layers_names);
            return rel_setup;
        }
        public PrototypeCreator(Document doc, Composition root) : this()
        {
            p_w = new SetupsInputWindow(new[] { getLayerSetup(doc), getRelativeLayerSetup(doc) });
        }

        public PrototypeCreator():base()
        {
            result = new PrototypeBlob();
        }
    }

}

