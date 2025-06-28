using Photoshop;
using psdPH.CED;
using psdPH.Context;
using psdPH.Photoshop;
using psdPH.Utils;

namespace psdPH.TemplateEditor
{
    public abstract class TemplateStackControl<T> : CEDElementControl<T>
    {
        protected PsdPhContext Context
        {
            get => new PsdPhContext(_doc, _root);
            set { _doc = value.doc; _root = value.root; }
        }
        protected DocumentWr _doc;
        protected Composition _root;

        protected TemplateStackControl(PsdPhContext context)
        {
            Context = context;
        }
    }
}
