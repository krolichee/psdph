using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleBrushBinding
{
    public partial class MyControl : UserControl
    {
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(
                "MyBrush",
                typeof(Brush),
                typeof(MyControl),
                new PropertyMetadata(Brushes.LightGray));

        public Brush MyBrush
        {
            get => (Brush)GetValue(MyBrushProperty);
            set => SetValue(MyBrushProperty, value);
        }

        public MyControl()
        {
            InitializeComponent();
        }
    }
}