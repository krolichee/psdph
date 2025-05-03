using Photoshop;
using psdPH.Utils;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public abstract class TemplateStackControl<T> : CEDStackControl<T>
    {
        protected PsdPhContext Context
        {
            get => new PsdPhContext(_doc, _root);
            set { _doc = value.doc; _root = value.root; }
        }
        protected Document _doc;
        protected Composition _root;

        protected TemplateStackControl(PsdPhContext context)
        {
            Context = context;
        }
    }
}
