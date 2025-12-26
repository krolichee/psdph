using psdPH.Lets;
using System;
using System.ComponentModel;

namespace psdPH.LetViews
{
    internal class AlignmentLetViewModel : CaptionViewModel
    {
        protected readonly Func<object> valueGetter;
        protected readonly Action<object> valueSetter;

        public AlignmentLetViewModel(Let let) : base(let.Name)
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