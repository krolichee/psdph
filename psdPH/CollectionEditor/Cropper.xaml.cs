using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class CropperWindow : Window
    {
        private Point _lastMousePosition;
        private bool _isDragging;
        private double _initialAngle;
        private bool _isRotating;

        private Size _size;
        private Size Size
        {
            get { return _size; }
            set { _size = value; 
            CutoutBorder.Width = _size.Width;
                CutoutBorder.Height = _size.Height;
            }
        }

        public BitmapImage Result { get; }
        public CropperWindow(Size size)
        {
            InitializeComponent();
            Size = size;


            // Получаем координаты CutoutBorder относительно MainRectangle
            Result = new BitmapImage();

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            renderCrop();
        }
        private void renderCrop()
        {
            Point cutoutPosition = CutoutBorder.TranslatePoint(new Point(0, 0), MainRectangle);

            // Создаем геометрию для основного прямоугольника
            RectangleGeometry mainGeometry = new RectangleGeometry(
                new Rect(0, 0, MainRectangle.Width, MainRectangle.Height)
            );

            // Создаем геометрию для вырезаемой области
            RectangleGeometry cutoutGeometry = new RectangleGeometry(
                new Rect(cutoutPosition, new Size(CutoutBorder.Width, CutoutBorder.Height))
            );

            // Вычитаем одну геометрию из другой
            CombinedGeometry combinedGeometry = new CombinedGeometry(
                GeometryCombineMode.Exclude, // Режим вычитания
                mainGeometry,
                cutoutGeometry
            );

            // Применяем обрезку к основному элементу
            MainRectangle.Clip = combinedGeometry;
            
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderCrop();
            //UpdateRenderTransformOrigin();
        }
        private void ImageControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Масштабирование изображения колесиком мыши
            double scaleFactor = e.Delta > 0 ? 1.1 : 0.9;
            ScaleTransform.ScaleX *= scaleFactor;
            ScaleTransform.ScaleY *= scaleFactor;
            
        }
        private void UpdateRenderTransformOrigin()
        {
            // Получаем центр Border в координатах относительно ImageControl
            Point borderCenter = new Point(CutoutBorder.ActualWidth / 2, CutoutBorder.ActualHeight / 2);
            Point borderCenterRelativeToImage = CutoutBorder.TranslatePoint(borderCenter, image);

            // Преобразуем абсолютные координаты в относительные (от 0 до 1)
            double originX = borderCenterRelativeToImage.X / image.ActualWidth;
            double originY = borderCenterRelativeToImage.Y / image.ActualHeight;

            // Устанавливаем RenderTransformOrigin
            image.RenderTransformOrigin = new Point(originX, originY);
        }

        private void ImageControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _isDragging = true;
                _lastMousePosition = e.GetPosition(grid);
                image.CaptureMouse();
            }
        }

        private void ImageControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _isDragging = false;
                image.ReleaseMouseCapture();
            }
            //UpdateRenderTransformOrigin();
        }

        private void ImageControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                _isRotating = true;
                _initialAngle = GetAngle(image, e.GetPosition(this));
                image.CaptureMouse();
            }

        }

        private void ImageControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                _isRotating = false;
                image.ReleaseMouseCapture();
            }
           //UpdateRenderTransformOrigin();
        }

        private void ImageControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                // Перемещение изображения
                Point currentPosition = e.GetPosition(grid);
                TranslateTransform.X += currentPosition.X - _lastMousePosition.X;
                TranslateTransform.Y += currentPosition.Y - _lastMousePosition.Y;
                _lastMousePosition = currentPosition;
            }
            else if (_isRotating)
            {
                // Поворот изображения
                double currentAngle = GetAngle(image, e.GetPosition(this));
                RotateTransform.Angle += currentAngle - _initialAngle;
                _initialAngle = currentAngle;
            }
        }

        private double GetAngle(FrameworkElement element, Point mousePosition)
        {
            Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
            Point relative = new Point(mousePosition.X - center.X, mousePosition.Y - center.Y);
            return Math.Atan2(relative.Y, relative.X) * 180 / Math.PI;
        }

        private void I(object sender, MouseButtonEventArgs e)
        {

        }

        private void image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //UpdateRenderTransformOrigin();
        }
        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            // Получаем визуальное представление изображения (с учетом трансформаций)
            var visual = image; // Замени на свой элемент с изображением
            var bounds = VisualTreeHelper.GetDescendantBounds(visual);
            var renderTarget = new RenderTargetBitmap(
                (int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(visual);

            // Получаем позицию и размер CutoutBorder относительно MainRectangle
            var cutoutPosition = CutoutBorder.TranslatePoint(new Point(0, 0), grid);
            var cutoutSize = new Size(CutoutBorder.ActualWidth, CutoutBorder.ActualHeight);

            // Создаем CroppedBitmap для вырезания области
            var croppedBitmap = new CroppedBitmap(
                renderTarget,
                new Int32Rect(
                    (int)cutoutPosition.X, (int)cutoutPosition.Y,
                    (int)cutoutSize.Width, (int)cutoutSize.Height)
            );

            // Сохраняем обрезанное изображение в PNG
            SaveBitmapToPng(croppedBitmap, "output.png");
        }

        private void SaveBitmapToPng(BitmapSource bitmap, string filePath)
        {
            // Создаем encoder для PNG
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            // Сохраняем на диск
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }

            MessageBox.Show($"Изображение сохранено: {filePath}");
        }

        private void SaveImage_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
