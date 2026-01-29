using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.TemplateEditor.Core
{
    public interface IBatchCreator<T>
    {
        T[] GetResultBatch();
        bool? ShowDialog();
    }
}
