using psdPH.CED;
using psdPH.Context;
using psdPH.Photoshop;

namespace psdPH.TemplateEditor
{
    abstract public class TemplateStackHandler : CEDPanelHandler
    {
        
        protected DocumentWr _doc;
        protected Composition _root;
        protected PsdPhContext Context
        {
            get => new PsdPhContext(_doc, _root);
            set { _doc = value.doc; _root = value.root; }
        }
        public TemplateStackHandler(PsdPhContext context)
        {
            Context = context;
        }
    }
}
