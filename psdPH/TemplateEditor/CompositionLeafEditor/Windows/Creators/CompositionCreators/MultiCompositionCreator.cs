﻿using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
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
        protected Document _doc;
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
        protected abstract PsLayerKind[] Kinds { get; }
        protected virtual Setup multiLayerParameter(Document doc)
        {
            string[] layers_names = doc.GetLayersNames(doc.GetLayersByKinds(Kinds));
            var ln_pconfig = new SetupConfig(this, nameof(Inputs), "Слой");
            return Setup.MultiChoose(ln_pconfig, layers_names); 
        }
        protected virtual Setup[] GetParameters(Document doc, Composition root)
        {
            return new Setup[] { multiLayerParameter(doc) };
        }
        public MultiCompositionCreator(Document doc, Composition root)
        {
            p_w = new SetupsInputWindow(GetParameters(doc, root));
            _doc = doc;
        }
    }
    public class MultiTextLeafCreator: MultiCompositionCreator<TextLeaf>
    {
        
        protected override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psTextLayer };
        protected override TextLeaf processInput(object input)
        {
            var layername = input as string;
            var layerWr =  _doc.GetLayerWrByName(layername);
            layerWr.FixLayerName();
            return new TextLeaf() { LayerName = layername };
        }
            
        public MultiTextLeafCreator(Document doc, Composition root) : base(doc,root) {  }
    }
    public class MultiLayerLeafCreator : MultiCompositionCreator<LayerLeaf>
    {
        
        protected override PsLayerKind[] Kinds => CommonLayers;

        public MultiLayerLeafCreator(Document doc, Composition root) : base(doc, root) { }

        protected override LayerLeaf processInput(object input)=> 
            new LayerLeaf() { LayerName = input as string };
    }
    public class MultiGroupLeafCreator : MultiCompositionCreator<GroupLeaf>
    {
        protected override Setup multiLayerParameter(Document doc)
        {
            string[] layers_names = doc.GetLayerSetsNames(doc.GetLayerSets());
            var ln_pconfig = new SetupConfig(this, nameof(Inputs), "Группа");
            return Setup.MultiChoose(ln_pconfig, layers_names);
        }
        public MultiGroupLeafCreator(Document doc, Composition root) : base(doc, root) { }

        protected override PsLayerKind[] Kinds => throw new NotImplementedException();

        protected override GroupLeaf processInput(object input)=> 
            new GroupLeaf() { LayerName = input as string };
    }
    public class MultiAreaLeafCreator : MultiCompositionCreator<AreaLeaf>
    {
        protected override PsLayerKind[] Kinds => CommonLayers;

        public MultiAreaLeafCreator(Document doc, Composition root) : base(doc, root) { }

        protected override AreaLeaf processInput(object input) =>
            new AreaLeaf() { LayerName = input as string };
    }
    public class MultiPlaceholderLeafCreator : MultiCompositionCreator<PlaceholderLeaf>
    {
        public string PrototypeLayerName;
        protected override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psSolidFillLayer, PsLayerKind.psNormalLayer };
        protected override Setup[] GetParameters(Document doc, Composition root)
        {
            var prototype_pconfig = new SetupConfig(this, nameof(PrototypeLayerName), "Прототип");
            var prototypeNames = root.GetChildren<PrototypeLeaf>().Select(p => p.LayerName).ToArray();
            var prototype_parameter = Setup.Choose(prototype_pconfig, prototypeNames); 
            return new[] { prototype_parameter, multiLayerParameter(doc) };
        }

        protected override PlaceholderLeaf processInput(object input) => 
            new PlaceholderLeaf() { LayerName = input as string, PrototypeLayerName = PrototypeLayerName };

        public MultiPlaceholderLeafCreator(Document doc, Composition root) : base(doc, root) { }
    }
}
