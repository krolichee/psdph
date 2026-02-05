using psdPH.Lets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.LetViews
{
    class CaptionLetViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected readonly Func<object> valueGetter;
        protected readonly Action<object> valueSetter;
        public CaptionLetViewModel(Let let)
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
