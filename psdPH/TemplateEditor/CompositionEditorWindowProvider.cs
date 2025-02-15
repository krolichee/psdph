using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH
{
    public interface ICompositionGenerator
    {
        Composition getResultComposition();
        bool? ShowDialog();
    }
    abstract public partial class CompositionEditorWindowProvider : ICompositionGenerator
    {
        abstract public Composition getResultComposition();

        public bool? ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}
