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
                item.Command = _addStructureItemCommand.Command;
            }
            addFlagItem.CommandParameter = new FlagEditorConfig();
            addTphItem.CommandParameter = new TextLeafEditorConfig();
            addImageItem.CommandParameter = new ImageEditorConfig();
            addBlob.CommandParameter = new BlobEditorConfig();
            addPhItem.CommandParameter = new PlaceholderEditorConfig();
            addProtoItem.CommandParameter = new PrototypeEditorConfig();
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
            lc_w.ShowDialog();
            string ln = lc_w.getResultString();
            if (ln == "")
                return null;
            CompositionEditorConfig new_config = new BlobEditorConfig() { Composition = Blob.LayerBlob(ln) };
            return OpenInDocument(doc, new_config);
        }
        public static BlobEditorWindow OpenInDocument(Document doc, CompositionEditorConfig config)
        {
            Blob blob = config.Composition as Blob;
            if (blob.Mode != BlobMode.Layer)
                throw new ArgumentException();
            Document new_doc;
            while (true)
                try
                {
                    new_doc = doc.OpenSmartLayer(blob.LayerName);
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
            _addStructureItemCommand = EditCommand.StructureCommand(doc, _root, this);
            _deleteStructureCommand = EditCommand.DeleteStructureCommand(doc,_root,this);
            _doc = doc;
            //_root.RuleSet.Rules.Add(new TextAnchorRule(_root) { Condition = new MaxRowCountCondition(_root) { RowCount = 10, TextLeafLayerName = "MudryBatyaVtuber" }, TextLeafLayerName = "MudryBatyaVtuber" });
            InitializeComponent();
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
        void refreshRuleStack()
        {
            rulesStackPanel.Children.Clear();
            ConditionRule[] rules = _root.RuleSet.Rules.Cast<ConditionRule>().ToArray();
            foreach (var rule in rules)
            {
                rulesStackPanel.Children.Add(new RuleTextBlock(rule));
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
                grid.Children.Add(new Button() { Content = "X", Foreground = new SolidColorBrush(Color.FromRgb(124, 0, 0)), HorizontalAlignment = HorizontalAlignment.Right, Command = _deleteStructureCommand.Command, CommandParameter = child });
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
            rc_window.ShowDialog();
            _root.RuleSet.Rules.Add(rc_window.GetResultRule());
            refreshRuleStack();
        }
    }
}
