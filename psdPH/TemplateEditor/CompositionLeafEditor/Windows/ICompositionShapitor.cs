using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public interface ICompositionShapitor
    {
        Composition GetResultComposition();
        bool? ShowDialog();
    }
    
    public enum EditorMode
    {
        Edit,
        Create
    }
}
