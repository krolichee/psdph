
using Photoshop;
using psdPH.Compositions;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Photoshop;
using psdPH.Setups;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace psdPH.TemplateEditor.Structure
{

    public abstract class SingleLeafCreator: IBatchCompositionCreator
    {
        protected static PsLayerKind[] CommonLayers = new PsLayerKind[] {
            PsLayerKind.psSolidFillLayer,
            PsLayerKind.psNormalLayer,
            PsLayerKind.psSmartObjectLayer,
            PsLayerKind.psTextLayer
        };
        protected Composition result;
        protected SetupsInputWindow p_w;
        public bool? ShowDialog()
        {
            return p_w.ShowDialog();
        }
        public Composition[] GetResultBatch()
        {
            return p_w.Applied ? new Composition[] { result } : new Composition[0];
        }
    }
    public class LayerLeafCreator : SingleLeafCreator
    {
        protected virtual LDFilter ldFilter => LDFilter.Layer(CommonLayers);
        protected virtual string label => "Слой";
        
        protected virtual void resultInit() => result = new LayerLeaf();
        protected ReflectionConfig resultLDConfig => new ReflectionConfig(result, nameof(LayerComposition.LayerDescriptor));
        protected virtual Setup[] getSetups(DocumentWr docWr) => new[] { LayerSetup.getLayerChooseSetup(docWr, ldFilter, resultLDConfig) };
        public LayerLeafCreator(DocumentWr docWr) : base()
        {
            resultInit();
            p_w = new SetupsInputWindow(getSetups(docWr));
        }
    }
    public class TextLeafCreator : LayerLeafCreator
    {
        protected override LDFilter ldFilter => LDFilter.Layer(PsLayerKind.psTextLayer);
        protected override string label => "Слой текстового поля";
        protected override void resultInit() => result = new TextLeaf();
        public TextLeafCreator(DocumentWr doc) : base(doc) { }
    }

    [Obsolete]
    public class ImageLeafCreator : LayerLeafCreator
    {
        protected override LDFilter ldFilter => LDFilter.Layer(PsLayerKind.psSmartObjectLayer);
        protected override string label => "Слой изображения";
        protected override void resultInit() => result = new ImageLeaf();
        public ImageLeafCreator(DocumentWr doc) : base(doc) { }
    }

    public class GroupLeafCreator : LayerLeafCreator
    {
        protected override LDFilter ldFilter => LDFilter.Group();
        protected override string label => "Группа";
        protected override void resultInit() => result = new GroupLeaf();
        public GroupLeafCreator(DocumentWr docWr) : base(docWr){}
    }
    public class AreaLeafCreator : LayerLeafCreator
    {
        protected override string label => "Слой зоны";
        public AreaLeafCreator(DocumentWr doc) : base(doc) { }
    }
    public class LayerBlobCreator : LayerLeafCreator
    {
        protected override LDFilter ldFilter => LDFilter.Layer(PsLayerKind.psSmartObjectLayer);
        protected override string label => "Слой поддокумента";
        protected override void resultInit() => result = new LayerBlob();
        public LayerBlobCreator(DocumentWr docWr) : base(docWr) { }
    }
    public class PrototypeBlobCreator : LayerLeafCreator
    {
        protected override LDFilter ldFilter => LDFilter.Layer(PsLayerKind.psSmartObjectLayer);
        protected override string label => "Слой прототипа";
        const string relativeLayerCaption = "Опорный слой";
        protected override void resultInit() => result = new PrototypeBlob();
        protected override Setup[] getSetups(DocumentWr docWr)
        {
            var relativeFilter = LDFilter.Layer(CommonLayers);
            var relativeConfig = new ReflectionConfig(result, nameof(PrototypeBlob.RelativeLayerDescriptor));
            return new[] {
                LayerSetup.getLayerChooseSetup(docWr, ldFilter, resultLDConfig),
                LayerSetup.getLayerChooseSetup(docWr,relativeFilter,relativeConfig) };
        }
        public PrototypeBlobCreator(DocumentWr doc):base(doc) {  }
    }

}

