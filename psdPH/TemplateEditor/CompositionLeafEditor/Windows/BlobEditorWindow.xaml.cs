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
using PsDocWr = psdPH.Logic.PhotoshopDocumentExtension;
using System.ComponentModel;
using psdPH.RuleEditor;
using psdPH.Logic.Rules;
using psdPH.Logic.Compositions;
using System.Runtime.Remoting.Messaging;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для TemplateEditor.xaml
    /// </summary>

    public partial class BlobEditorWindow : Window, ICompositionShapitor
    {
        Composition _composition;
        CEDCommand _addStructureCommand;
        CEDCommand _editStructureCommand;
        CEDCommand _deleteStructureCommand;
        Document _doc;
        MenuItem CreateAddMenuItem(Type type)
        {
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = _addStructureCommand.Command,
                CommandParameter = _composition
            };
        }
        void InitializeAddDropDownMenu()
        {
            List<MenuItem> items = new List<MenuItem>();
            foreach (var comp_type in StructureDicts.CreatorDict.Keys)
                items.Add(CreateAddMenuItem(comp_type));
            CreateDropdownMenu.ItemsSource = items;
        }
        public static BlobEditorWindow OpenInDocument(Document doc, Blob blob)
        {
            if (blob.Mode != BlobMode.Layer)
                throw new ArgumentException();
            Document new_doc;
            new_doc = doc.OpenSmartLayer(blob.LayerName);
            return new BlobEditorWindow(new_doc, blob);
        }
        public static BlobEditorWindow OpenFromDisk(Blob blob)
        {
            if (blob.Mode != BlobMode.Path)
                throw new ArgumentException();
            PsApp psApp = PsWr.GetPhotoshopApplication();
            Document doc = psApp.Open(blob.Path);
            return new BlobEditorWindow(doc, blob);
        }
        BlobEditorWindow(Document doc,Blob root)
        {
            _composition = root;

            _addStructureCommand = CEDCommand.CreateCommand(doc, _composition);
            _editStructureCommand = CEDCommand.EditCommand(doc,_composition);
            _deleteStructureCommand = CEDCommand.DeleteCommand(_composition);
            _doc = doc;
            InitializeComponent();
            Closing += (object sender, CancelEventArgs e) => DialogResult = true;
            InitializeAddDropDownMenu();
            _composition.ChildrenUpdatedEvent+=refreshSctuctureStack;
            refreshSctuctureStack();
            refreshRuleStack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        public Composition GetResultComposition()
        {
            return _composition;
        }
        void removeRule(Rule rule)
        {
            _composition.RuleSet.Rules.Remove(rule);
            refreshRuleStack();
        }
        void refreshRuleStack()
        {
            rulesStackPanel.Children.Clear();
            ConditionRule[] rules = _composition.RuleSet.Rules.Cast<ConditionRule>().ToArray();
            foreach (var rule in rules)
            {
                var rtb = new RuleTextBlock(rule);

                rtb.ContextMenu = new ContextMenu();
                rtb.ContextMenu.Items.Add(new MenuItem() { 
                    Header = "Удалить",
                    Command = new RelayCommand((object o)=>removeRule(o as Rule),(_)=>true),
                    CommandParameter = rule,
                Margin = new Thickness(0,0,0,10)}
                    
                    );
                rulesStackPanel.Children.Add(rtb);
            }

        }
        void refreshSctuctureStack()
        {
            _composition.Restore();
            structuresStackPanel.Children.Clear();
            foreach (Composition child in _composition.getChildren())
            {
                var button = new CompositionStackElement(child,_editStructureCommand.Command,_deleteStructureCommand.Command);
                button.Width = structuresStackPanel.RenderSize.Width;
                structuresStackPanel.Children.Add(button);
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (_doc.Application.ActiveDocument == _doc)
                _doc.Close(PsSaveOptions.psDoNotSaveChanges);
            else
                throw new Exception();
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            refreshSctuctureStack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var rc_window = new RuleControlWindow(_composition);
            if (rc_window.ShowDialog() != true)
                return;
            _composition.RuleSet.Rules.Add(rc_window.GetResultRule());
            refreshRuleStack();
        }
    }
}
