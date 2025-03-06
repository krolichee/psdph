using Photoshop;
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
            return new TextLeafEditor(doc, config as TextLeafEditorConfig);
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
                return BlobEditorWindow.OpenInDocument(doc, new BlobEditorConfig() { Composition = (config.Composition as Prototype).Blob});
        }
    }

    public class ImageEditorWindowFactory : ICompositionEditorWindowFactory
    {
        public ICompositionEditor CreateCompositionEditorWindow(Document doc, CompositionEditorConfig config, Composition root)
        {
            throw new NotImplementedException();
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

    public class TextLeafEditorConfig : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psTextLayer };
        public override ICompositionEditorWindowFactory Factory => new TextLeafEditorWindowFactory();
    }

    public class BlobEditorConfig : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psSmartObjectLayer };
        public override ICompositionEditorWindowFactory Factory => new BlobEditorWindowFactory();
    }
    public class PlaceholderEditorConfig : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psNormalLayer };
        public override ICompositionEditorWindowFactory Factory => new PlaceholderLeafWindowFactory();
    }

    public class PrototypeEditorConfig : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psNormalLayer };
        public override ICompositionEditorWindowFactory Factory => new PrototypeEditorWindowFactory();
    }

    public class FlagEditorConfig : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psNormalLayer };
        public override ICompositionEditorWindowFactory Factory => new FlagEditorWindowFactory();
    }

    public class ImageEditorConfig : CompositionEditorConfig
    {
        public override PsLayerKind[] Kinds => new PsLayerKind[] { PsLayerKind.psNormalLayer };
        public override ICompositionEditorWindowFactory Factory => new ImageEditorWindowFactory();
    }
}
