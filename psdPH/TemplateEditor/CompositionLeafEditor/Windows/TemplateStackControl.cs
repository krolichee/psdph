using Photoshop;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public abstract class TemplateStackControl<T>:CEDStackControl<T>
    {
        protected PsdPhContext Context { 
            get => new PsdPhContext(_doc, _root); 
            set { _doc = value.doc; _root = value.root; } }
        protected Document _doc;
        protected Composition _root;

        protected TemplateStackControl(PsdPhContext context)
        {
            Context = context;
        }
    }
}
