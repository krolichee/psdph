using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace psdPH.TemplateEditor { 
public partial class SectionControl : ContentControl
    {
    public SectionControl()
    {
        InitializeComponent();
        this.DataContext = this;
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(SectionControl));

    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(SectionControl));

    public static readonly new DependencyProperty BorderThicknessProperty =
        DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(SectionControl),
            new PropertyMetadata(new Thickness(1, 1, 1, 1)));

    public static readonly new DependencyProperty BorderBrushProperty =
        DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(SectionControl),
            new PropertyMetadata(Brushes.Black));

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public ImageSource IconSource
    {
        get { return (ImageSource)GetValue(IconSourceProperty); }
        set { SetValue(IconSourceProperty, value); }
    }

    public new Thickness BorderThickness
    {
        get { return (Thickness)GetValue(BorderThicknessProperty); }
        set { SetValue(BorderThicknessProperty, value); }
    }

    public new Brush BorderBrush
    {
        get { return (Brush)GetValue(BorderBrushProperty); }
        set { SetValue(BorderBrushProperty, value); }
    }

    public new object Content
    {
        get { return GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    public static new readonly DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(object), typeof(SectionControl));
}}