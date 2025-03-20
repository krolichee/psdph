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

    public partial class BlobEditorWindow : Window, ICompositionEditor
    {
        Composition _root;
        EditCommand _addStructureItemCommand;
        EditCommand _deleteStructureCommand;
        Document _doc;
        MenuItem CreateMenuItem(Type type)
        {
            Type configType = CompositionConfigDictionary.GetConfigType(type);
            return new MenuItem()
            {
                Header = TypeLocalization.GetLocalizedDescription(type),
                Command = _addStructureItemCommand.Command,
                CommandParameter = Activator.CreateInstance(configType)
            };
        }
        void InitializeElements()
        {
            List<MenuItem> items = new List<MenuItem>();
            foreach (var comp_type in CompositionConfigDictionary.StoC.Keys)
                items.Add(CreateMenuItem(comp_type));
            DropdownMenu.ItemsSource = items;
        }
        static string ChooseLayer(Document doc, CompositionEditorConfig config)
        {
            string[] layer_names = PsDocWr.GetLayersNames(doc.GetLayersByKinds(config.Kinds));
            StringChoiceWindow lc_w = new StringChoiceWindow(layer_names, "Выбор слоя поддокумента");
            return lc_w.getResultString();

        }
        public static BlobEditorWindow CreateWithinDocument(Document doc, CompositionEditorConfig config)
        {
            string[] layer_names = PsDocWr.GetLayersNames(doc.GetLayersByKinds(config.Kinds));
            var lc_w = new StringChoiceWindow(layer_names, "Выбор слоя поддокумента");
            if(lc_w.ShowDialog()!=true)
                return null;
            string ln = lc_w.getResultString();
            CompositionEditorConfig new_config = new BlobEditorCfg() { Composition = Blob.LayerBlob(ln) };
            return OpenInDocument(doc, new_config);
        }
        public static BlobEditorWindow OpenInDocument(Document doc, CompositionEditorConfig config)
        {
            Blob blob = config.Composition as Blob;
            if (blob.Mode != BlobMode.Layer)
                throw new ArgumentException();
            Document new_doc;

            new_doc = doc.OpenSmartLayer(blob.LayerName);

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
            _addStructureItemCommand = EditCommand.StructureCommand(doc, _root, this);
            _deleteStructureCommand = EditCommand.DeleteStructureCommand(doc,_root,this);
            _doc = doc;
            //_root.RuleSet.Rules.Add(new TextAnchorRule(_root) { Condition = new MaxRowCountCondition(_root) { RowCount = 10, TextLeafLayerName = "MudryBatyaVtuber" }, TextLeafLayerName = "MudryBatyaVtuber" });
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
                var grid = new Grid();
                grid.Children.Add(new Label() { Content = child.UIName, Foreground = SystemColors.ActiveBorderBrush, HorizontalAlignment = HorizontalAlignment.Left });
                grid.Children.Add(new Label() { Content = child.ObjName, Foreground = SystemColors.ActiveCaptionTextBrush, HorizontalAlignment = HorizontalAlignment.Center });
                grid.Children.Add(new Button() { Content = "X", Foreground = new SolidColorBrush(Color.FromRgb(124, 0, 0)), HorizontalAlignment = HorizontalAlignment.Right, Command = _deleteStructureCommand.Command, CommandParameter = child, Width = 20 });
                grid.Width = structuresStackPanel.RenderSize.Width;
                var button = new Button();
                button.Height = 28;
                button.Content = grid;
                button.Command = _addStructureItemCommand.Command;
                Type type = CompositionConfigDictionary.GetConfigType(child.GetType());
                CompositionEditorConfig config = Activator.CreateInstance(type) as CompositionEditorConfig;
                config.Composition = child;
                button.CommandParameter = config;
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
