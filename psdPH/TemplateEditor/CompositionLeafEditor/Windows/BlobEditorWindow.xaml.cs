using Photoshop;
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

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для TemplateEditor.xaml
    /// </summary>

    public partial class BlobEditorWindow : Window, ICompositionGenerator
    {
        Composition _root;
        AddStructureItemCommand _addStructureItemCommand;

        ///<summary>
        ///Конструктор для открытия смарт-объекта в данном документе. Выход из документа без сохранения
        ///</summary>
        public BlobEditorWindow(PhotoshopDocumentWrapper doc, CompositionEditorConfig config)
        {   
            string psd_path = InitializeParameters(doc, ref config);
            InitializeComponent();
            if (psd_path == "")
            {
                this.Close();
                return;
            }
            doc.OpenSmartLayer(psd_path);
            _root = config.Composition;
            _addStructureItemCommand = new AddStructureItemCommand(psd, _root, this);

            addTphItem.Command = _addStructureItemCommand.MyCommand;
            addSub.Command = _addStructureItemCommand.MyCommand;
            addTphItem.CommandParameter = new TextLeafEditorConfig(null);
            addSub.CommandParameter = new BlobEditorConfig(null);
        }

        ///<summary>
        ///Конструктор для открытия документа. Выход из документа без сохранения
        ///</summary>
        public BlobEditorWindow(CompositionEditorConfig config)

        {
            Blob blob = config.Composition as Blob;
            if (blob.Mode != BlobMode.Path)
                throw new ArgumentException();
            PhotoshopWrapper psd = new PhotoshopWrapper();
            psd.OpenDocument(blob.Path);
        }
        ///<summary>
        ///Конструктор для выбора смарт-объекта для создания блоба в данном документе. Выход из документа без сохранения
        ///</summary>
        public BlobEditorWindow(PhotoshopDocumentWrapper doc)
        {

        }


        protected string InitializeParameters(PhotoshopDocumentWrapper doc, ref CompositionEditorConfig config)
        {
            //1. doc = object, config = object : в открытом документе открыть смарт-объект
            //2. doc = object, config = 0 : в открытом документе выбрать слой, затем открыть смарт объект
            //3. doc = 0, config = object : открыть документ по пути, потом 1
            if (doc == null)
            {
                if (config?.Composition == null)
                    throw new ArgumentNullException();
                if (config)
                PhotoshopWrapper
            }

            if (config?.Composition == null) //2
            {
                string[] layer_names = PhotoshopDocumentWrapper.GetLayersNames(doc.GetLayersByKinds(config.Kinds));
                var lc_w = new LayerChoiceWindow(layer_names);
                lc_w.ShowDialog();
                string ln = lc_w.getResultString();
                if (ln == "")
                    return "";
                int id = doc.GetLayerByName(ln).id;
                config = new BlobEditorConfig(new Blob(ln, BlobMode.Layer));
            }
            else //1
            {
                int id = doc.GetLayerByName(((Blob)config.Composition).Name).id;
            }
            return "psd_path";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        public Composition getResultComposition()
        {
            return _root;
        }

        void refreshCompositionStack()
        {
            stackPanel.Children.Clear();
            foreach (var child in _root.getChildren())
            {
                var button = new Button();
                button.Height = 28;
                var grid = new Grid();
                button.Content = grid;
                grid.Children.Add(new Label() { Content = child.UIName, Foreground = SystemColors.ActiveBorderBrush, HorizontalAlignment = HorizontalAlignment.Left });
                grid.Children.Add(new Label() { Content = child.ObjName, Foreground = SystemColors.ActiveCaptionTextBrush, HorizontalAlignment = HorizontalAlignment.Center });
                stackPanel.Children.Add(button);
                grid.Width = stackPanel.Width;
            }
        }

        public class AddStructureItemCommand
        {
            private Composition _root_composition;
            private PhotoshopDocumentWrapper _doc;
            private BlobEditorWindow _tew;

            public ICommand MyCommand { get; set; }

            public AddStructureItemCommand(PhotoshopDocumentWrapper doc, Composition composition, BlobEditorWindow tew)
            {
                _root_composition = composition;
                _doc = doc;
                _tew = tew;
                MyCommand = new RelayCommand(ExecuteCommand, CanExecuteCommand);
            }

            private void ExecuteCommand(object parameter)
            {
                var config = parameter as CompositionEditorConfig;
                ICompositionGenerator cle_w = config.Factory.CreateCompositionEditorWindow(_doc, config);
                cle_w.ShowDialog();
                _root_composition.addChild(cle_w.getResultComposition());
                _tew.refreshCompositionStack();
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
    }
}
