using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.RuleEditor;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils;
using psdPH.Utils.CedStack;
using psdPH.Views.WeekView;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PsApp = Photoshop.Application;
using PsWr = psdPH.PhotoshopWrapper;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для TemplateEditor.xaml
    /// </summary>

    public partial class BlobEditorWindow : Window, IBatchCompositionCreator
    {

        Composition _composition;
        Document _doc;
        public static BlobEditorWindow OpenInDocument(Document doc, Blob blob)
        {
            if (blob.Mode != BlobMode.Layer)
                throw new ArgumentException();
           
            Document new_doc;
            new_doc = doc.OpenSmartLayer(blob.LayerName);
            var result = new BlobEditorWindow(new_doc, blob);
            result.templateMenu.Visibility = Visibility.Hidden;
            return result;
        }
        
        public static BlobEditorWindow OpenFromDisk()
        {
            Blob blob = PsdPhProject.Instance().openOrCreateMainBlob();
            Document doc = PhotoshopWrapper.OpenDocument(PsdPhDirectories.ProjectPsd(PsdPhProject.Instance().ProjectName));
            var editor = new BlobEditorWindow(doc, blob);
            editor.Show();
            return editor;
        }
        BlobEditorWindow(Document doc, Blob root)
        {
            _composition = root;
            _doc = doc;
            InitializeComponent();
            
            structureTab.Content = CEDStackUI.CreateCEDStack(
                new StructureStackHandler(new PsdPhContext(doc, root)));
            ruleTab.Content = CEDStackUI.CreateCEDStack(
                new StructureRuleStackHandler(_composition.RuleSet));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        public Composition GetResultComposition()
        {
            return _composition;
        }
        private void Window_Closed(object sender, EventArgs e)
        {                
            if ((_composition as Blob).Mode == BlobMode.Path)
                save();
            _doc.Close(PsSaveOptions.psSaveChanges);
        }
        void save()
        {
            PsdPhProject.Instance().saveBlob(GetResultComposition() as Blob);
            
        }
        private void Window_Activated(object sender, EventArgs e)
        {

        }
        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _composition = Blob.PathBlob("template.psd");
            Close();
        }

        public Composition[] GetResultBatch()
        {
            return  new Composition[] { _composition };
    }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            save();
        }
    }
}
