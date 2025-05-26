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
using System.Windows;
using System.Windows.Controls;
using PsApp = Photoshop.Application;
using PsWr = psdPH.PhotoshopWrapper;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для TemplateEditor.xaml
    /// </summary>

    public partial class BlobEditorWindow : Window, ICompositionShapitor
    {

        Composition _composition;
        Document _doc;
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
            Document doc = PhotoshopWrapper.OpenDocument(psApp,Directories.ProjectPsd(PsdPhProject.Instance().ProjectName));
            return new BlobEditorWindow(doc, blob);
        }
        BlobEditorWindow(Document doc, Blob root)
        {
            _composition = root;
            _doc = doc;
            InitializeComponent();
            Closing += (object sender, CancelEventArgs e) => DialogResult = true;
            structureTab.Content = CEDStackUI.CreateCEDStack(
                new StructureStackHandler(new PsdPhContext(doc, root)));
            ruleTab.Content = CEDStackUI.CreateCEDStack(
                new StructureRuleStackHandler(new PsdPhContext(doc, root)));
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
            if (_doc.Application.ActiveDocument == _doc)
                _doc.Close(PsSaveOptions.psDoNotSaveChanges);
            else
                throw new Exception();
        }
        private void Window_Activated(object sender, EventArgs e)
        {

        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _composition = Blob.PathBlob("template.psd");
            Close();
        }
    }
}
