﻿using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
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
    public class PlaceholderLeafCreator : SingleLeafCreator<PlaceholderLeaf>
    {
        public PlaceholderLeafCreator(Document doc, Composition root) : base()
        {
            var prototype_pconfig = new SetupConfig(result, nameof(result.PrototypeLayerName), "Прототип");
            var rel_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой вставки");
            var prototypeNames = root.GetChildren<PrototypeLeaf>().Select(p => p.LayerName).ToArray();
            var prototype_parameter = Setup.Choose(prototype_pconfig, prototypeNames);
            var rel_parameter = Setup.Choose(rel_pconfig, doc.GetLayersNames());
            p_w = new SetupsInputWindow(new[] { prototype_parameter, rel_parameter });
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
            var layerParameter = Setup.Choose(ln_pconfig, layers_names);
            parameters.Add(layerParameter);
            p_w = new SetupsInputWindow(parameters.ToArray());
        }
        public TextLeafCreator(Document doc) : base()
        {
            Single(doc);
        }
    }
    public class PrototypeCreator : SingleLeafCreator<PrototypeLeaf>
    {
        public PrototypeCreator(Document doc, Composition root) : base()
        {
            string[] blobs_names = root.GetChildren<Blob>().Select(b => b.LayerName).ToArray();
            var bn_pconfig = new SetupConfig(result, nameof(result.LayerName), "Поддокумент");
            var bn_parameter = Setup.Choose(bn_pconfig, blobs_names);

            string[] rel_layers_names = PhotoshopDocumentExtension.GetLayersNames(
                doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer }));
            var rel_pconfig = new SetupConfig(result, nameof(result.RelativeLayerName), "Опорный слой");
            var rel_parameter = Setup.Choose(rel_pconfig, rel_layers_names);
            p_w = new SetupsInputWindow(new[] { bn_parameter, rel_parameter });
        }
    }
    public class ImageLeafCreator : SingleLeafCreator<ImageLeaf>
    {
        public ImageLeafCreator(Document doc) : base()
        {
            result.LayerName = "";
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой");
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKind(PsLayerKind.psNormalLayer));
            p_w = new SetupsInputWindow(new[] { Setup.Choose(ln_pconfig, layers_names) });
        }
    }
    public class LayerLeafCreator : SingleLeafCreator<LayerLeaf>
    {
        public LayerLeafCreator(Document doc) : base()
        {
            result.LayerName = "";
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой");
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer }));
            p_w = new SetupsInputWindow(new[] { Setup.Choose(ln_pconfig, layers_names) });
        }
    }
    public class GroupLeafCreator : SingleLeafCreator<GroupLeaf>
    {
        public GroupLeafCreator(Document doc) : base()
        {
            result.LayerName = "";
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Группа");
            string[] layers_names = doc.GetLayerSetsNames(doc.GetLayerSets());
            p_w = new SetupsInputWindow(new[] { Setup.Choose(ln_pconfig, layers_names) });
        }
    }
    public class AreaLeafCreator : SingleLeafCreator<AreaLeaf>
    {
        public AreaLeafCreator(Document doc) : base()
        {
            result.LayerName = "";
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer }));
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой поля");
            var ln_parameter = Setup.Choose(ln_pconfig, layers_names);

            p_w = new SetupsInputWindow(new[] { ln_parameter});
        }
    }
    public class BlobCreator : SingleLeafCreator<Blob>
    {
        public BlobCreator(Document doc, Blob root) : base()
        {
            result.LayerName = "";
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(new PsLayerKind[] { PsLayerKind.psSmartObjectLayer }));
            var ln_pconfig = new SetupConfig(result, nameof(result.LayerName), "Слой");
            var ln_parameter = Setup.Choose(ln_pconfig, layers_names);
            p_w = new SetupsInputWindow(new[] { ln_parameter });
        }
    }

}

