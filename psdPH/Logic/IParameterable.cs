using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic
{
    public interface IParameterable
    {
        Parameter[] Parameters { get; }
    }
}
