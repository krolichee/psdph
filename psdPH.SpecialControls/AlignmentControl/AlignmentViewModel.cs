using psdPH.Alignments;
using psdPH.Localization;
using psdPH.Photoshop;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace psdPH.SpecialControls
{
    public class AlignmentViewModel : INotifyPropertyChanged
    {
        private Alignment _selectedAlignment;

        public Alignment SelectedAlignment
        {
            get => _selectedAlignment;
            set
            {
                AlignmentOptions.ForEach((ao) => ao.IsSelected = ao.Alignment == value);
                _selectedAlignment = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AlignmentDescription));
            }
        }

        public string AlignmentDescription =>
            LocalizationService.Localize(SelectedAlignment);

        public ICommand SelectAlignmentCommand { get; }

        public List<AlignmentOption> AlignmentOptions { get; }

        static List<AlignmentOption> GenerateAlignmentOptions()
        {
            return new List<AlignmentOption>
        {
            new AlignmentOption("up", "left"),
            new AlignmentOption("up", "center"),
            new AlignmentOption("up", "right"),
            new AlignmentOption("center", "left"),
            new AlignmentOption("center", "center"),
            new AlignmentOption("center", "right"),
            new AlignmentOption("down", "left"),
            new AlignmentOption("down", "center"),
            new AlignmentOption("down", "right"),
            new AlignmentOption("up", "none"),
            new AlignmentOption("down", "none"),
            new AlignmentOption("none", "left"),
            new AlignmentOption("none", "right")
        };
}
        public AlignmentViewModel()
        {
            AlignmentOptions = GenerateAlignmentOptions();
            SelectedAlignment = Alignment.Create("center", "center");
            SelectAlignmentCommand = new RelayCommand((o)=> 
            SelectAlignment(o as Alignment));

        }

        private void SelectAlignment(Alignment alignment)
        {
            SelectedAlignment = alignment;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
