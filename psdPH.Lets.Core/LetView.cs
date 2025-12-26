using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Lets.Core
{
    public abstract class LetView
    {
        protected Control control;
        readonly Let let;
        public LetView(Let let)
        {
            this.let = let;
        }
        public Control Control => control;
        public Let Let => let;

    }
}
