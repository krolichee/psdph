using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace psdPH.TemplateEditor
{
    public partial class SectionControl : UserControl
    {
        public SectionControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SectionTitleProperty =
            DependencyProperty.Register("SectionTitle", typeof(string), typeof(SectionControl));

        public static readonly DependencyProperty SectionIconSourceProperty =
            DependencyProperty.Register("SectionIconSource", typeof(ImageSource), typeof(SectionControl));

        public static readonly DependencyProperty SectionBorderThicknessProperty =
            DependencyProperty.Register("SectionBorderThickness", typeof(Thickness), typeof(SectionControl),
                new PropertyMetadata(new Thickness(1, 1, 1, 1)));

        public static readonly DependencyProperty SectionBorderBrushProperty =
            DependencyProperty.Register("SectionBorderBrush", typeof(Brush), typeof(SectionControl),
                new PropertyMetadata(Brushes.Black));

        public string SectionTitle
        {
            get => (string)GetValue(SectionTitleProperty);
            set => SetValue(SectionTitleProperty, value);
        }

        public ImageSource SectionIconSource
        {
            get => (ImageSource)GetValue(SectionIconSourceProperty);
            set => SetValue(SectionIconSourceProperty, value);
        }

        public Thickness SectionBorderThickness
        {
            get => (Thickness)GetValue(SectionBorderThicknessProperty);
            set => SetValue(SectionBorderThicknessProperty, value);
        }

        public Brush SectionBorderBrush
        {
            get => (Brush)GetValue(SectionBorderBrushProperty);
            set => SetValue(SectionBorderBrushProperty, value);
        }
    }
}