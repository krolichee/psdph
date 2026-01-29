using psdPH.Alignments;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace psdPH.SpecialControls
{
    public partial class AlignmentControl : UserControl
    {
        public static readonly DependencyProperty SelectedAlignmentProperty =
            DependencyProperty.Register(
                "SelectedAlignment",
                typeof(Alignment),
                typeof(AlignmentControl),
                new FrameworkPropertyMetadata(
                    Alignment.Create("center", "center"),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty DimensionProperty =
            DependencyProperty.Register(
                "Dimension",
                typeof(int),
                typeof(AlignmentControl),
                new PropertyMetadata(100, OnDimensionChanged));

        public Alignment SelectedAlignment
        {
            get => (Alignment)GetValue(SelectedAlignmentProperty);
            set => SetValue(SelectedAlignmentProperty, value);
        }

        public int Dimension
        {
            get => (int)GetValue(DimensionProperty);
            set => SetValue(DimensionProperty, value);
        }

        public AlignmentViewModel ViewModel { get; }

        public AlignmentControl()
        {
            InitializeComponent();
            ViewModel = new AlignmentViewModel();
            DataContext = ViewModel;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AlignmentViewModel.SelectedAlignment))
            {
                // Обновляем DependencyProperty при изменении в ViewModel
                SetValue(SelectedAlignmentProperty, ViewModel.SelectedAlignment);
            }
        }

        private static void OnDimensionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AlignmentControl control && e.NewValue is int size)
            {
                control.Height = size;
                control.Width = size;
            }
        }

        // Конструкторы для обратной совместимости
        public AlignmentControl(Alignment alignment) : this()
        {
            if (alignment != null)
                SelectedAlignment = alignment;
        }

        public AlignmentControl(int size) : this()
        {
            Dimension = size;
        }
    }
}
