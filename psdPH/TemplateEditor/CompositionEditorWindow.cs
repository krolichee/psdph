using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.TemplateEditor
{
    public interface ICompositionGenerator
    {
        Composition getResultComposition();
    }
    public abstract class CompositionEditorWindow : Window, ICompositionGenerator
    {
        abstract public Composition getResultComposition();
    }
}
