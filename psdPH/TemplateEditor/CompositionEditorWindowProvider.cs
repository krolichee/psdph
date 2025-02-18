using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH
{
    public interface ICompositionEditor
    {
        Composition getResultComposition();
        bool? ShowDialog();
    }
    abstract public partial class CompositionEditorWindowProvider : ICompositionEditor
    {
        abstract public Composition getResultComposition();

        public bool? ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}
