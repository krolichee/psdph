using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

public class CanvasDragBehavior : Behavior<UIElement>
{
    public static readonly DependencyProperty CanvasDraggedProperty =
        DependencyProperty.Register(
            nameof(CanvasDragged),
            typeof(ICommand),
            typeof(CanvasDragBehavior));

    public ICommand CanvasDragged
    {
        get => (ICommand)GetValue(CanvasDraggedProperty);
        set => SetValue(CanvasDraggedProperty, value);
    }

    private Point _startPoint;
    private bool _isDragging;
    private Canvas _parentCanvas;

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
        AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
        AssociatedObject.MouseMove += OnMouseMove;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
        AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
        AssociatedObject.MouseMove -= OnMouseMove;
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _isDragging = true;
        _startPoint = e.GetPosition(AssociatedObject);
        AssociatedObject.CaptureMouse();
    }

    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        AssociatedObject.ReleaseMouseCapture();
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging) return;

        if (_parentCanvas == null)
        {
            _parentCanvas = VisualTreeHelper.GetParent(AssociatedObject) as Canvas;
            if (_parentCanvas == null) return;
        }

        var newPosition = e.GetPosition(_parentCanvas);
        Canvas.SetLeft(AssociatedObject, newPosition.X - _startPoint.X);
        Canvas.SetTop(AssociatedObject, newPosition.Y - _startPoint.Y);

        // Вызов команды (если она задана)
        if (CanvasDragged?.CanExecute(null) == true)
        {
            CanvasDragged.Execute(newPosition);
        }
    }
}