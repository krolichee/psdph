using Photoshop;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public partial class StructureStackHandler
    {
        public struct PsdPhContext
        {
            public Document doc;
            public Composition root;
            public PsdPhContext(Document doc, Composition root)
            {
                this.doc = doc;
                this.root = root;
            }
        }
    }
}
