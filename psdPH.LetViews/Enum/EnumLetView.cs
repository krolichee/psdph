using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.LetViews.Choose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.LetViews
{
    public class EnumLetView:LetView
    {
        public EnumLetView(Let let) : base(let)
        {
            control = new ChooseLetViewControl(let,new object[]{}) { DataContext = new EnumLetViewModel(let)};
        }
    }
}
