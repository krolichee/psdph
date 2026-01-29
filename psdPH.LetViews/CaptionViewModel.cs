using psdPH.Localization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace psdPH.LetViews
{
    public class CaptionViewModel : INotifyPropertyChanged
    {
        private string _caption;

        public event PropertyChangedEventHandler PropertyChanged;

        public CaptionViewModel(string caption)
        {
            _caption = caption;
        }

        public string Caption
        {
            get => _caption;
            set
            {
                if (_caption != value)
                {
                    _caption = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
