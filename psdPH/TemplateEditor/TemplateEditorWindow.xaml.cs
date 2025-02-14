using Photoshop;
using System;
using System.Collections.Generic;
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
using static psdPH.TemplateEditorWindow;

namespace psdPH
{
    //enum LayerType
    //{
    //    Group,
    //    Adjusting,
    //    Smart
    //}
    /// <summary>
    /// Логика взаимодействия для TemplateEditor.xaml
    /// </summary>
    public partial class TemplateEditorWindow : Window
    {
        public class EditCompositionWindow : Window
        {
            

        }
        Composition _root;
        AddStructureItemCommand addStructureItemCommand;
        public TemplateEditorWindow(PhotoshopWrapper psd,Composition root=null)
        {
            InitializeComponent();
            if (root == null)
            {
                root = new Compositor(psd.GetDocPath());
                //CompositionLeafEditorWindow cmp_wind = new CompositionLeafEditorWindow(,new CompositionLeafEditorConfig());
                //cmp_wind.ShowDialog();
                //root = cmp_wind.getResult();
            }
            _root = root;

            addStructureItemCommand = new AddStructureItemCommand(psd,_root);
            addTphItem.Command = addStructureItemCommand.MyCommand;
            addTphItem.CommandParameter = "hello";


            
            
        }
        public class PSDFile
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }
        public class AddStructureItemCommand
        {
            private Composition _root_composition;
            private PhotoshopWrapper _psd;

            public ICommand MyCommand { get; set; }

            public AddStructureItemCommand(PhotoshopWrapper psd, Composition composition )
            {
                _root_composition = composition;
                _psd = psd;

                MyCommand = new RelayCommand(ExecuteCommand, CanExecuteCommand);
            }

            private void ExecuteCommand(object parameter)
            {
                List<string> layer_names = new List<string>();
                foreach (var layer in _psd.GetLayerNames())
                    if (layer.kind == PsLayerKind.psTextLayer)
                        layer_names.Add(layer.name);
                var cle_w = new CompositionLeafEditorWindow(layer_names.ToArray(),new CompositionLeafEditorConfig());
                cle_w.ShowDialog();
                _root_composition.addChild(cle_w.getResult());
                new EditCompositionWindow();
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
