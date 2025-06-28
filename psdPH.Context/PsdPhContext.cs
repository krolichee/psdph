using psdPH.Photoshop;

namespace psdPH.Context
{
    public struct PsdPhContext
    {
        public DocumentWr doc;
        public Composition root;
        public PsdPhContext(DocumentWr doc, Composition root)
        {
            this.doc = doc;
            this.root = root;
        }
    }
}
