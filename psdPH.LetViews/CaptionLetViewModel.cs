using psdPH.Lets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.LetViews
{
    class CaptionLetViewModel : CaptionViewModel
    {
        protected readonly Func<object> valueGetter;
        protected readonly Action<object> valueSetter;
        public CaptionLetViewModel(Let let) : base(let.Name)
        {
            valueGetter = () => let.Value;
            valueSetter = (v) => let.Value = v;
        }
        public object Value
        {
            get => valueGetter();
            set
            {
                if (valueGetter() != value)
                {
                    valueSetter(value);
                    OnPropertyChanged();
                }
            }
        }
    }
}
