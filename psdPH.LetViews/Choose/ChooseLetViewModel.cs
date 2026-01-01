using psdPH.Lets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.LetViews.Choose
{
    class ChooseLetViewModel : CaptionLetViewModel
    {
        public object Options { get; }
        public ChooseLetViewModel(Let let, object[] options) : base(let)
        {
            Options = options;
        }
    }
}
