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
        Composition _root;
        EditCommand _addStructureItemCommand;
        EditCommand _deleteStructureCommand;
        EditCommand _create_deleteStructureCommand;
        Document _doc;
        MenuItem CreateMenuItem(Type type)
        {
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = _addStructureItemCommand.Command,
                CommandParameter = _root
            };
        }
        void InitializeElements()
        {
            List<MenuItem> items = new List<MenuItem>();
            foreach (var comp_type in StructureDicts.CreatorDict.Keys)
                items.Add(CreateMenuItem(comp_type));
            CreateDropdownMenu.ItemsSource = items;
        }
        public static BlobEditorWindow CreateWithinDocument(Document doc,Blob root)
        {

            string[] layer_names = PsDocWr.GetLayersNames(doc.GetLayersByKind(PsLayerKind.psSmartObjectLayer));
            var lc_w = new StringChoiceWindow(layer_names, "Выбор слоя поддокумента");
            if(lc_w.ShowDialog()!=true)
                return null;
            string ln = lc_w.getResultString();
            var blob = Blob.LayerBlob(ln);
            return OpenInDocument(doc, blob);
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
            _root = root;
            _addStructureItemCommand = EditCommand.StructureCommand(doc, _root, this);
            _deleteStructureCommand = EditCommand.DeleteStructureCommand(doc,_root,this);
            _doc = doc;
            InitializeComponent();
            Closing += (object sender, CancelEventArgs e) => DialogResult = true;
            InitializeElements();
            refreshSctuctureStack();
            refreshRuleStack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        public Composition GetResultComposition()
        {
            return _root;
        }
        void removeRule(Rule rule)
        {
            _root.RuleSet.Rules.Remove(rule);
            refreshRuleStack();
        }
        void refreshRuleStack()
        {
            rulesStackPanel.Children.Clear();
            ConditionRule[] rules = _root.RuleSet.Rules.Cast<ConditionRule>().ToArray();
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
            _root.Restore();
            structuresStackPanel.Children.Clear();
            foreach (Composition child in _root.getChildren())
            {
                var button = new CompositionStackElement(child,_addStructureItemCommand.Command);
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
            var rc_window = new RuleControlWindow(_root);
            if (rc_window.ShowDialog() != true)
                return;
            _root.RuleSet.Rules.Add(rc_window.GetResultRule());
            refreshRuleStack();
        }
    }
}
