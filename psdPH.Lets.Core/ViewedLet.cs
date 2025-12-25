using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Lets.Core
{
    class ViewedLet
    {
        readonly Let let;
        readonly LetView letView;

        public ViewedLet(Let let, LetView letView)
        {
            this.let = let;
            this.letView = letView;
        }

        public LetView View => letView;
    }
}
