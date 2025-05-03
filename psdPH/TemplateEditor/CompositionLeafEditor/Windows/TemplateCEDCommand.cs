using Photoshop;
using psdPH.Utils;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public class TemplateCEDCommand : CEDCommand
    {
        protected PsdPhContext Context
        {
            get => new PsdPhContext(_doc, _root);
            set { _doc = value.doc; _root = value.root; }
        }
        protected Document _doc;
        protected Composition _root;
        protected TemplateCEDCommand(PsdPhContext context)
        {
            Context = context;
        }
    }
}
