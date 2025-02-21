using Photoshop;
using psdPH;
using psdPH.Logic;
using psdPH.TemplateEditor;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PsApp = Photoshop.Application;
using PsWr = psdPH.PhotoshopWrapper;
using PsDocWr = psdPH.Logic.PhotoshopDocumentWrapper;
using System.ComponentModel;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для TemplateEditor.xaml
    /// </summary>

    public partial class BlobEditorWindow : Window, ICompositionEditor
    {
        Composition _root;
        AddStructureItemCommand _addStructureItemCommand;
        Document _doc;
        void InitializeElements()
        {
            /*
             * <MenuItem x:Name="addFlagItem" Header="Флаг"/>
                <MenuItem x:Name="addTphItem" Header="Текстовое поле" />
                <MenuItem x:Name="addImageItem" Header="Поле изображения" />
                <MenuItem x:Name="addVisItem" Header="Видимость слоя" />
                <MenuItem x:Name="addSub" Header="Поддокумент" />
                <MenuItem x:Name="addPhItem" Header="Плейсхолдер" />
                <MenuItem x:Name="addProtoItem" Header="Прототип" />
             */
            foreach (MenuItem item in DropdownMenu.Items)
            {
                item.Command = _addStructureItemCommand.MyCommand;
            }
            addFlagItem.CommandParameter = new FlagEditorConfig(null);
            addTphItem.CommandParameter = new TextLeafEditorConfig(null);
            addImageItem.CommandParameter = new ImageEditorConfig(null);
            addVisItem.CommandParameter = new VisEditorConfig(null);
            addBlob.CommandParameter = new BlobEditorConfig(null);
            addPhItem.CommandParameter = new PlaceholderEditorConfig(null);
            addProtoItem.CommandParameter = new ProtoEditorConfig(null);
        }
        static string ChooseLayer(Document doc, CompositionEditorConfig config)
        {
            PsDocWr docWr = new PsDocWr(doc);

            string[] layer_names = PsDocWr.GetLayersNames(docWr.GetLayersByKinds(config.Kinds, LayerListing.Recursive));
            StringChoiceWindow lc_w = new StringChoiceWindow(layer_names, "Выбор слоя поддокумента");
            return lc_w.getResultString();

        }
        public static BlobEditorWindow CreateWithinDocument(Document doc, CompositionEditorConfig config)
        {
            string[] layer_names = PsDocWr.GetLayersNames(new PsDocWr(doc).GetLayersByKinds(config.Kinds, LayerListing.Recursive));
            var lc_w = new StringChoiceWindow(layer_names, "Выбор слоя поддокумента");
            lc_w.ShowDialog();
            string ln = lc_w.getResultString();
            if (ln == "")
                return null;
            CompositionEditorConfig new_config = new BlobEditorConfig(new Blob(ln, BlobMode.Layer));
            return OpenInDocument(doc, new_config);
        }
        public static BlobEditorWindow OpenInDocument(Document doc, CompositionEditorConfig config)
        {
            Blob blob = config.Composition as Blob;
            if (blob.Mode != BlobMode.Layer)
                throw new ArgumentException();
            PsDocWr docWr = new PsDocWr(doc);
            Document new_doc;
            while (true)
                try
                {
                    new_doc = docWr.OpenSmartLayer(blob.LayerName, LayerListing.Recursive);
                    break;
                }
                catch
                {
                    string ln;
                    MessageBox.Show("Нет такого слоя. Возможно, документ был изменён. Исправьте имя слоя");
                    ln = ChooseLayer(doc, config);
                    blob.LayerName = ln;
                }

            return new BlobEditorWindow(new_doc, config);
        }
        public static BlobEditorWindow OpenFromDisk(CompositionEditorConfig config)
        {
            Blob blob = config.Composition as Blob;
            if (blob.Mode != BlobMode.Path)
                throw new ArgumentException();
            PsApp psApp = PsWr.GetPhotoshopApplication();
            Document doc = psApp.Open(blob.Path);
            return new BlobEditorWindow(doc, config);
        }
        BlobEditorWindow(Document doc, CompositionEditorConfig config)
        {
            _root = config.Composition;
            _addStructureItemCommand = new AddStructureItemCommand(doc, _root, this);
            _doc = doc;
            InitializeComponent();
            InitializeElements();
            RefreshCompositionStack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        public Composition getResultComposition()
        {
            return _root;
        }

        void RefreshCompositionStack()
        {
            stackPanel.Children.Clear();
            foreach (Composition child in _root.getChildren())
            {
                var grid = new Grid();
                grid.Children.Add(new Label() { Content = child.UIName, Foreground = SystemColors.ActiveBorderBrush, HorizontalAlignment = HorizontalAlignment.Left });
                grid.Children.Add(new Label() { Content = child.ObjName, Foreground = SystemColors.ActiveCaptionTextBrush, HorizontalAlignment = HorizontalAlignment.Center });
                grid.Width = stackPanel.Width;
                var button = new Button();
                button.Height = 28;
                button.Content = grid;
                button.Command = _addStructureItemCommand.MyCommand;
                Type type = CompositionConfigDictionary.GetConfigType(child.GetType());
                button.CommandParameter = Activator.CreateInstance(type, new object[] { child });
                stackPanel.Children.Add(button);
            }
        }

        public class AddStructureItemCommand
        {
            private Composition _root_composition;
            private Document _doc;
            private BlobEditorWindow _editor;

            public ICommand MyCommand { get; set; }

            public AddStructureItemCommand(Document doc, Composition composition, BlobEditorWindow tew)
            {
                _root_composition = composition;
                _doc = doc;
                _editor = tew;
                MyCommand = new RelayCommand(ExecuteCommand, CanExecuteCommand);
            }

            private void ExecuteCommand(object parameter)
            {
                var config = parameter as CompositionEditorConfig;
                ICompositionEditor ce_w = config.Factory.CreateCompositionEditorWindow(_doc, config, _root_composition);
                if (ce_w == null)
                    return;
                ce_w.ShowDialog();
                Composition result = ce_w.getResultComposition();
                if (result != null)
                    _root_composition.addChild(result);
                _editor.RefreshCompositionStack();
            }

            private bool CanExecuteCommand(object parameter)
            {
                return true; // Здесь можно добавить логику для определения, может ли команда быть выполнена
            }
        }
        public class RelayCommand : ICommand
        {

            private readonly Action<object> _execute;
            private readonly Func<object, bool> _canExecute;

            public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_doc.Application.ActiveDocument == _doc)
                _doc.Close(PsSaveOptions.psDoNotSaveChanges);
        }
    }
}
