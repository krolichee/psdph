using Photoshop;
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
        ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config);
    };
    public class TextLeafEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config)
        {
            return new TextLeafEditorWindow(doc, config as TextLeafEditorConfig);
        }
    }
    public class BlobEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config)
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
