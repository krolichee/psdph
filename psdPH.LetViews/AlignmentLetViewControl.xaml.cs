using psdPH.Lets.Core;
using psdPH.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace psdPH.LetViews
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AlignmentLetViewControl : UserControl
    {
        public AlignmentLetViewControl()
        {
            InitializeComponent();
        }
        public AlignmentLetViewControl(string caption = null):this()
        {
            DataContext = new AlignmentLetViewModel(caption);
        }
    }
    public class AlignmentLetViewModel : INotifyPropertyChanged
    {
        private string _caption;

        public event PropertyChangedEventHandler PropertyChanged;

        public AlignmentLetViewModel(string caption = null)
        {
            if (caption == null)
                caption = AlignmentLetViewStrings.DefaultCaption.Localize();
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
    enum AlignmentLetViewStrings
    {
        DefaultCaption
    }
    public class AlignmentLetView : LetView
    {
        readonly Control control;
        public AlignmentLetView()
        {
            control = new AlignmentLetViewControl();
        }
        public Control Control => control;
    }
}
