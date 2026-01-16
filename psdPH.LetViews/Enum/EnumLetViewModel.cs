using psdPH.Lets;
using psdPH.LetViews.Choose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace psdPH.LetViews
{
    class EnumLetViewModel : ChooseLetViewModel
    {
        static object[] getEnumOptions(Let let)
        {
            var enumValues = Enum.GetValues(let.Type).Cast<Enum>();
            var options = enumValues.ToArray();
            return options;
        }
        public EnumLetViewModel(Let let) : base(let, getEnumOptions(let))
        {
        }
    }
}
