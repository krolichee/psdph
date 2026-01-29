using Photoshop;
using psdPH.CED;
using psdPH.Context;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Nodes.Editor;
using psdPH.Photoshop;
using psdPH.Project;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.CedStacks.ParameterCedStack;
using psdPH.Utils;
using psdPH.Utils.CedStack;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PsApp = Photoshop.Application;

namespace psdPH.TemplateEditor
{
    /// <summary>
    /// Логика взаимодействия для TemplateEditor.xaml
    /// </summary>

    public partial class TemplateEditorWindow : Window, IBatchCompositionCreator
    {
        Composition _composition;
        DocumentWr _doc;
        public static TemplateEditorWindow OpenInDocument(DocumentWr doc, LayerBlob blob)
        {
            DocumentWr new_doc;
            new_doc = doc.OpenSmartLayer(blob.LayerDescriptor);
            var editor = new TemplateEditorWindow(new_doc, blob);
            editor.templateMenu.Visibility = Visibility.Hidden;
            return editor;
        }
        
        public static TemplateEditorWindow OpenFromDisk()
        {
            RootBlob blob = PsdPhProject.Instance().openOrCreateMainBlob();
            DocumentWr doc = PhotoshopWrapper.OpenDocument(PsdPhDirectories.ProjectPsd(PsdPhProject.Instance().ProjectName));
            var editor = new TemplateEditorWindow(doc, blob);
            return editor;
        }
        
        TemplateEditorWindow(DocumentWr doc, Composition root)
        {
            _composition = root;
            _doc = doc;
            InitializeComponent();
            new CEDStackUI();

            var structureCed = CEDStackUI.CreateCEDStack(
                new StructureStackHandler(new PsdPhContext(doc, root)))
            ;
            structureCed.VerticalAlignment = VerticalAlignment.Stretch;
            structureSection.Content = structureCed;
            paramSection.Content = CEDStackUI.CreateCEDStack(
                new ParameterHandler(_composition.ParameterSet));
            nodesSection.Content = new NodesEditor(root);
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
            if ((_composition is RootBlob))
                save();
            _doc.Close(PsSaveOptions.psSaveChanges);
        }
        void save()
        {
            PsdPhProject.Instance().saveBlob(GetResultComposition() as RootBlob);
            
        }
        private void Window_Activated(object sender, EventArgs e)
        {

        }
        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _composition = new RootBlob();
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
