using Photoshop;
using psdPH.Utils;
using psdPH.Utils.CedStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    abstract public class TemplateStackHandler:CEDStackHandler
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
