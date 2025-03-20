using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.TemplateEditor.CompositionLeafEditor.Windows.TinyEditors;
using PsApp = Photoshop.Application;

namespace psdPH.TemplateEditor
{
    public interface ICompositionEditorWindowFactory
    {
        ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root);
    };
    public class TextLeafEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new TextLeafEditor(doc, config as TextLeafEditorCfg);
        }
    }
    public class BlobEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            ICompositionEditor result;
            if (doc == null)
                result = BlobEditorWindow.OpenFromDisk(config);
            else if (config.Composition == null)
                result = BlobEditorWindow.CreateWithinDocument(doc, config);
            else
                result = BlobEditorWindow.OpenInDocument(doc, config);
            return result;
        }
    }
    public class PlaceholderLeafWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new PlaceholderLeafEditor(doc, config, root);
        }
    }
    public class FlagEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new FlagEditor(config);
        }
    }
    public class PrototypeEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            if (config.Composition == null)
                return new PrototypeEditor(doc, config, root);
            else
                return BlobEditorWindow.OpenInDocument(doc, new BlobEditorCfg() { Composition = (config.Composition as PrototypeLeaf).Blob});
        }
    }

    public class ImageLeafEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new ImageLeafEditor(doc, config);
        }
    }public class LayerLeafEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new LayerLeafEditor(doc, config);
        }
    }
    public class GroupLeafEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new GroupLeafEditor(doc, config);
        }
    }
    public class TextAreaEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new TextAreaLeafEditor(doc, config,root as Blob);
        }
    }


    /// <summary>
    /// -------------------------------------
    /// </summary>

    public abstract class CompositionEditorConfig
    {
        public Composition Composition;
        public virtual PsLayerKind[] Kinds { get; }
        public virtual ICompositionEditorWindowFactory Factory { get; }
    }

    public class TextLeafEditorCfg : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psTextLayer };
        public override ICompositionEditorWindowFactory Factory => new TextLeafEditorWindowFactory();
    }

    public class BlobEditorCfg : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psSmartObjectLayer };
        public override ICompositionEditorWindowFactory Factory => new BlobEditorWindowFactory();
    }
    public class PlaceholderEditorCfg : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psNormalLayer };
        public override ICompositionEditorWindowFactory Factory => new PlaceholderLeafWindowFactory();
    }

    public class PrototypeLeafEditorCfg : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psNormalLayer };
        public override ICompositionEditorWindowFactory Factory => new PrototypeEditorWindowFactory();
    }

    public class FlagLeafEditorCfg : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psNormalLayer };
        public override ICompositionEditorWindowFactory Factory => new FlagEditorWindowFactory();
    }

    public class ImageLeafEditorCfg : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psNormalLayer };
        public override ICompositionEditorWindowFactory Factory => new ImageLeafEditorWindowFactory();
    }
    public class LayerLeafEditorCfg : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psNormalLayer, PsLayerKind.psSolidFillLayer, PsLayerKind.psSmartObjectLayer};
        public override ICompositionEditorWindowFactory Factory => new LayerLeafEditorWindowFactory();
    }
    public class GroupLeafEditorCfg : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { };
        public override ICompositionEditorWindowFactory Factory => new GroupLeafEditorWindowFactory();
    }
    public class TextAreaLeafEditorCfg : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { };
        public override ICompositionEditorWindowFactory Factory => new TextAreaEditorWindowFactory();
    }
    


}
