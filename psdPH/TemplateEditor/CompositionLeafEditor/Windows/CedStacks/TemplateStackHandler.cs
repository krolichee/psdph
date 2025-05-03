using Photoshop;
using psdPH.Utils;
using psdPH.Utils.CedStack;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    abstract public class TemplateStackHandler : CEDStackHandler
    {
        protected PsdPhContext Context
        {
            get => new PsdPhContext(_doc, _root);
            set { _doc = value.doc; _root = value.root; }
        }
        protected Document _doc;
        protected Composition _root;
        public TemplateStackHandler(PsdPhContext context)
        {
            Context = context;
        }
    }
}
