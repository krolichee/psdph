using psdPH.Lets;
using System;
using System.ComponentModel;
using psdPH.Utils;
using psdPH.Alignments;

namespace psdPH.LetViews
{
    public class AlignmentLetViewModel : CaptionViewModel
    {
        protected readonly Func<Alignment> valueGetter;
        protected readonly Action<Alignment> valueSetter;

        public AlignmentLetViewModel(Let let) : base(let.Name)
        {
            valueGetter = () => let.Value as Alignment;
            valueSetter = (v) => let.Value = v;
        }


        public Alignment Value 
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