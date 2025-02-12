using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;


namespace psdPH
{
    public class DrawingGroupToImage
    {
        public static BitmapSource ExportDrawingGroupToImage(DrawingGroup drawingGroup, int width, int height)
        {
            // Создание Visual для рендеринга
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Рисуем DrawingGroup на DrawingContext
                drawingContext.DrawDrawing(drawingGroup);
            }

            // Настройка размеров битмапа
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(visual);

            return renderTargetBitmap; // Возвращаем созданное изображение
        }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Cropper cropper = new Cropper();
        public MainWindow()
        {
            InitializeComponent();
            CreateSubtractingMask();
            
           
            
            //border_semitrans.Opacity = 0.5;

        }
        private void CreateSubtractingMask()
        {

            // Создайте DrawingBrush с белым цветом и прозрачным прямоугольником
            DrawingBrush drawingBrush = new DrawingBrush();
            DrawingGroup drawingGroup = new DrawingGroup();
            drawingBrush.Drawing = drawingGroup;
            //(drawingBrush.Drawing as DrawingGroup).Children.Add(new GeometryDrawing(Brushes.Black, null, new RectangleGeometry(new Rect(0, 0, border_semitrans.Width, border_semitrans.Height))));
            //(drawingBrush.Drawing as DrawingGroup).Children.Add(new GeometryDrawing(Brushes.Red, null, rectangleGeometry));
            // Установите OpacityMask для Border
            border_semitrans.OpacityMask = drawingBrush;
            var m_t = rect_mask.Margin.Top;
            var m_l = rect_mask.Margin.Left;
            var m_b = rect_mask.Margin.Bottom;
            var m_r = rect_mask.Margin.Right;
            var w = border_semitrans.Width;
            var h = border_semitrans.Height;
            using (DrawingContext dc = drawingGroup.Open())
            {
                dc.DrawRectangle(Brushes.Red, null, new Rect(0, 0, w, m_t));
                dc.DrawRectangle(Brushes.Red, null, new Rect(0, 0, m_l, h));
                dc.DrawRectangle(Brushes.Red, null, new Rect(0, m_b, w, h-m_b));
                dc.DrawRectangle(Brushes.Red, null, new Rect(m_r, 0, w - m_r, h));
            }

            SaveDrawingBrushAsImage(drawingBrush, "C:\\Users\\Puziko\\Desktop\\mask.bmp");
            BitmapSource image = DrawingGroupToImage.ExportDrawingGroupToImage(drawingBrush.Drawing as DrawingGroup , 1000, 500);
            using (var fileStream = new FileStream("C:\\Users\\Puziko\\Desktop\\mask1.bmp", FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
            //sobolcoin_image.Clip = border_semitrans
        }
        private void SaveDrawingBrushAsImage(DrawingBrush brush, string filePath)
        {
            // Создаем отображаемый элемент с использованием DrawingBrush
            var drawingVisual = new DrawingVisual();
            using (var dc = drawingVisual.RenderOpen())
            {
                dc.DrawRectangle(brush, null, new Rect(0, 0, 1000, 500));
            }


            // Создаем RenderTargetBitmap
            RenderTargetBitmap bmp = new RenderTargetBitmap(1000, 500, 96d, 96d, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            // Сохраняем изображение в файл
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();   // Выбор формата PNG
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(fileStream);
            }

            MessageBox.Show($"Изображение сохранено: {filePath}");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cropper.ShowDialog();
        }
    }
}
