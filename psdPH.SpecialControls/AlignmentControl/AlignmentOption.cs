using psdPH.Alignments;
using psdPH.Localization;
using System.ComponentModel;

namespace psdPH.SpecialControls
{
    public class AlignmentOption:INotifyPropertyChanged
    {
        private bool _isSelected;
        public AlignmentOption(string vertical, string horizontal)
        {
            Alignment = Alignment.Create(vertical, horizontal);
            DisplayName = LocalizationService.Localize(Alignment);
        }
        public int Row
        {
            get
            {
                switch (Vertical) {
                    case VAilgnment.Center:
                        return 2;
                    case VAilgnment.Top:
                        return Horizontal == HAilgnment.None ? 0 : 1;
                    case VAilgnment.Bottom:
                        return Horizontal == HAilgnment.None ? 4 : 3;
                    case VAilgnment.None:
                        return 2;
                    default:
                        return 0;
                }
            }
        }

        public int Column
        {
            get
            {
                switch (Horizontal)
                {
                    case HAilgnment.Center:
                        return 2;
                    case HAilgnment.Left:
                        return Vertical == VAilgnment.None ? 0 : 1;
                    case HAilgnment.Right:
                        return Vertical == VAilgnment.None ? 4 : 3;
                    case HAilgnment.None:
                        return 2;
                    default:
                        return 0;
                }
            }
        }
        protected HAilgnment Horizontal=>Alignment.H;
        protected VAilgnment Vertical=>Alignment.V;
        public Alignment Alignment { get; }
        public string DisplayName { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
