using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using Path = System.IO.Path;

namespace psdPH.CollectionEditor
{
    class ImageCollection
    {
        public readonly string Name;
        public readonly BitmapImage[] CollectionImages;
        public readonly Size Resolution;
        public ImageCollection(string directory)
        {
            Name = Path.GetFileNameWithoutExtension(directory);
            string[] images_paths = Directory.EnumerateFiles(
                directory)
                .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            CollectionImages = new BitmapImage[images_paths.Length];
            for (int i = 0; i < images_paths.Length; i++)
            {
                string p = images_paths[i];
                BitmapImage img = new BitmapImage(new Uri(p));
                if (i == 0)
                    Resolution = new Size(img.SourceRect.Width, img.SourceRect.Height);
                else
                    if (img.SourceRect.Width != Resolution.Width || img.SourceRect.Height != Resolution.Height)
                    throw new Exception("Разрешение изображений не совпадает");
            }
        }
    }
    /// <summary>
    /// Логика взаимодействия для CollectionEditor.xaml
    /// </summary>
    public partial class CollectionEditor : Window
    {
        ImageCollection currentCollection;

        OpenCollectionCommand occ;
        class OpenCollectionCommand : ICommand
        {
            private CollectionEditor collectionEditor;

            public OpenCollectionCommand(CollectionEditor collectionEditor)
            {
                this.collectionEditor = collectionEditor;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                string collectionName = parameter as string;
                collectionEditor.openCollection(collectionName);
            }
        }
        void openCollection(string collectionName)
        {
            string collection_dir = Path.Combine(PsdPhDirectories.CollectionsDirectory, collectionName);

            currentCollection = new ImageCollection(collection_dir);
            foreach (var bitmapImage in currentCollection.CollectionImages)
            {
                Image imageControl = new Image
                {
                    Source = bitmapImage,
                    Width = 100, // Установите желаемую ширину
                    Height = 100, // Установите желаемую высоту
                    Margin = new Thickness(5) // Отступы между изображениями
                };
                imagesStackPanel.Children.Add(imageControl);
            }
            collectionNameLabel.Content = collectionName + "\n" + $"{currentCollection.Resolution.ToString()}";
        }
        public CollectionEditor()
        {
            InitializeComponent();
            occ = new OpenCollectionCommand(this);
            string collections_path = PsdPhDirectories.CollectionsDirectory;
            string[] collections_paths = Directory.EnumerateDirectories(collections_path).ToArray();
            foreach (var item in collections_paths)
            {
                string collectionName = Path.GetFileNameWithoutExtension(item);
                collectionsMenuItem.Items.Add(new MenuItem() { Header = collectionName, Command = occ, CommandParameter = collectionName });
            }

        }
    }
}
