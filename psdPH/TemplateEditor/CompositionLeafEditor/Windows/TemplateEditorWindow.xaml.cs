using Photoshop;
using psdPH.TemplateEditor;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    
    
    public class TemplateEditorWindow : CompositionEditorWindow
    {
        
        public class EditCompositionWindow : Window
        {
            

        }
        Composition _root;
        AddStructureItemCommand _addStructureItemCommand;
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

            _addStructureItemCommand = new AddStructureItemCommand(psd,_root);
            addTphItem.Command = _addStructureItemCommand.MyCommand;
            addTphItem.CommandParameter = "hello";
        }
        public class PSDFile
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        public Composition getResultComposition()
        {
            return _root;
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
                
                var cle_w = new TextLeafEditorWindow(_psd.GetLayerNames(), new CompositionLeafEditorConfig( new PsLayerKind[]{PsLayerKind.psTextLayer}));
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
