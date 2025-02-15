using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.TemplateEditor
{
    public interface ICompositionEditorWindowFactory
    {
        ICompositionGenerator CreateCompositionEditorWindow(PhotoshopWrapper psd, CompositionEditorConfig config);
    };
    public class TextLeafEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionGenerator CreateCompositionEditorWindow(PhotoshopWrapper psd, CompositionEditorConfig config)
        {
            return new TextLeafEditorWindow(psd, config as TextLeafEditorConfig);
        }
    }
    public class BlobEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionGenerator CreateCompositionEditorWindow(PhotoshopWrapper psd, CompositionEditorConfig config)
        {
            return new BlobEditorWindow(psd, config);
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
        public BlobEditorConfig(Blob textLeaf)
        {
            Kinds = new PsLayerKind[] { PsLayerKind.psSmartObjectLayer };
            Factory = new BlobEditorWindowFactory();
            Composition = textLeaf;
        }
    }
}
