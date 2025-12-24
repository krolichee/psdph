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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    
    public abstract class MultiCompositionCreator<T> : IBatchCompositionCreator where T : Composition, new()
    {
        protected virtual LDFilter ldFilter => LDFilter.Layer(CommonLayers);
        protected virtual string label => "Слой";
        protected DocumentWr _doc;
        protected static PsLayerKind[] CommonLayers = new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer, PsLayerKind.psSmartObjectLayer, PsLayerKind.psTextLayer };
        public object[] Inputs
        {
            set
            {
                foreach (var input in value)
                {
                    var composition = processInput(input);
                    result.Add(composition);
                }
            }
        }
        protected abstract T processInput(object input);
        protected List<T> result=new List<T>();
        protected SetupsInputWindow p_w;
        public Composition[] GetResultBatch()
        {
            return p_w.Applied ? result.ToArray() : new Composition[0];
        }
        public bool? ShowDialog()
        {
            return p_w.ShowDialog();
        }
        protected MultiCompositionCreator()
        {
            result = new List<T>();
        }

        protected virtual Setup multiLayerSetup(DocumentWr doc)
        {
            var lds = LayerSetup.GetFilteredLDs(doc, ldFilter);
            var ln_pconfig = new ReflectionConfig(this, nameof(Inputs), label);
            return new MultiChooseSetup(ln_pconfig, lds); 
        }
        protected virtual Setup[] GetSetups(DocumentWr doc, Composition root)
        {
            return new Setup[] { multiLayerSetup(doc) };
        }
        public MultiCompositionCreator(DocumentWr doc, Composition root)
        {
            p_w = new SetupsInputWindow(GetSetups(doc, root));
            _doc = doc;
        }
    }
    public class MultiTextLeafCreator: MultiCompositionCreator<TextLeaf>
    {
        protected override LDFilter ldFilter => LDFilter.Layer(PsLayerKind.psTextLayer);
        protected override TextLeaf processInput(object input)
        {
            var ld = input as LayerDescriptor;
            var layerWr = ld.GetLayerWr(_doc);
            layerWr.FixLayerName();
            return new TextLeaf() { LayerDescriptor = ld };
        }
        public MultiTextLeafCreator(DocumentWr doc, Composition root) : base(doc,root) {  }
    }
    public class MultiLayerLeafCreator : MultiCompositionCreator<LayerLeaf>
    {
        public MultiLayerLeafCreator(DocumentWr doc, Composition root) : base(doc, root) { }

        protected override LayerLeaf processInput(object input)=> 
            new LayerLeaf() { LayerDescriptor = input as LayerDescriptor };
    }
    public class MultiGroupLeafCreator : MultiCompositionCreator<GroupLeaf>
    {
        protected override LDFilter ldFilter => LDFilter.Group();
        public MultiGroupLeafCreator(DocumentWr doc, Composition root) : base(doc, root) { }

        protected override GroupLeaf processInput(object input)=> 
            new GroupLeaf() { LayerName = input as string };
    }
    public class MultiAreaLeafCreator : MultiCompositionCreator<AreaLeaf>
    {
        public MultiAreaLeafCreator(DocumentWr doc, Composition root) : base(doc, root) { }

        protected override AreaLeaf processInput(object input) =>
            new AreaLeaf() { LayerName = input as string };
    }
    public class MultiPlaceholderLeafCreator : MultiCompositionCreator<PlaceholderLeaf>
    {
        public PrototypeBlob PrototypeBlob;
        protected override LDFilter ldFilter => LDFilter.Layer(new[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer });
        protected override Setup[] GetSetups(DocumentWr doc, Composition root)
        {
            var result = new List<Setup>();
            var prototypeConfig = new ReflectionConfig(this, nameof(PrototypeBlob), "Прототип");
            var prototypes = root.GetChildren<PrototypeBlob>();
            var prototypeSetup = new ChooseSetup(prototypeConfig, prototypes);
            result.Add(prototypeSetup);
            result.AddRange(base.GetSetups(doc, root));
            return result.ToArray();
        }

        protected override PlaceholderLeaf processInput(object input) => 
            new PlaceholderLeaf() { LayerName = input as string, PrototypeBlob = PrototypeBlob };

        public MultiPlaceholderLeafCreator(DocumentWr doc, Composition root) : base(doc, root) { }
    }
}
