using Photoshop;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return new TextLeafEditorWindow(doc, config as TextLeafEditorConfig);
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
                result = BlobEditorWindow.CreateWithinDocument(doc,config);
            else
                result = BlobEditorWindow.OpenInDocument(doc, config);
            return result;
        }
    }
    public class PlaceholderLeafWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new PlaceholderLeafEditorWindow(doc, config, root);
        }
    }
    public class FlagEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new FlagEditorWindow(doc,config,root);
        }
    }
    public class PrototypeEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new PrototypeEditorWindow(doc, config, root);
        }
    }
    
        public class ImageEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            return new PrototypeEditorWindow(doc, config, root);
        }
    }


    /// <summary>
    /// -------------------------------------
    /// </summary>

    public abstract class CompositionEditorConfig
    {
        public Composition Composition;
        public PsLayerKind[] Kinds;
        public ICompositionEditorWindowFactory Factory;
    }
    public class TextLeafEditorConfig : CompositionEditorConfig
    {

        public TextLeafEditorConfig(TextLeaf textLeaf)
        {
            Kinds = new PsLayerKind[] { PsLayerKind.psTextLayer };
            Factory = new TextLeafEditorWindowFactory();
            Composition = textLeaf;
        }
    }
    public class BlobEditorConfig: CompositionEditorConfig
    {
        public BlobEditorConfig(Blob blob)
        {
            Kinds = new PsLayerKind[] { PsLayerKind.psSmartObjectLayer };
            Factory = new BlobEditorWindowFactory();
            Composition = blob;
        }
    }
    public class PlaceholderEditorConfig : CompositionEditorConfig
    {
        public PlaceholderEditorConfig(Blob pph)
        {
            Kinds = new PsLayerKind[] { PsLayerKind.psNormalLayer};
            Factory = new PlaceholderLeafWindowFactory();
            Composition = pph;
        }
    }
    public class ProtoEditorConfig : CompositionEditorConfig
    {
        public ProtoEditorConfig(PrototypeLeaf pph)
        {
            Kinds = new PsLayerKind[] { PsLayerKind.psSmartObjectLayer };
            Factory = new PrototypeEditorWindowFactory();
            Composition = pph;
        }
    }
    public class FlagEditorConfig: CompositionEditorConfig
    {
        public FlagEditorConfig(Blob pph)
        {
            Kinds = new PsLayerKind[] { PsLayerKind.psNormalLayer };
            Factory = new FlagEditorWindowFactory();
            Composition = pph;
        }
    }
    public class ImageEditorConfig : CompositionEditorConfig
    {
        public ImageEditorConfig(Blob pph)
        {
            Kinds = new PsLayerKind[] { PsLayerKind.psNormalLayer };
            Factory = new ImageEditorWindowFactory();
            Composition = pph;
        }
    }
    public class VisEditorConfig : CompositionEditorConfig
    {
        public VisEditorConfig(Blob pph)
        {
            Kinds = new PsLayerKind[] { PsLayerKind.psNormalLayer };
            Factory = new BlobEditorWindowFactory();
            Composition = pph;
        }
    }
}
