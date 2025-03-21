using Photoshop;
using psdPH.TemplateEditor;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System;
using System.Windows.Input;
using static psdPH.TemplateEditor.StructureDicts;

namespace psdPH
{
    public partial class BlobEditorWindow
    {
        public class EditCommand
        {
            private Composition _root_composition;
            private Document _doc;
            private BlobEditorWindow _editor;

            public ICommand Command { get; set; }
            public static EditCommand StructureCommand(Document doc, Composition root, BlobEditorWindow editor)
            {
                var result = new EditCommand(doc, root, editor, null);
                result.Command = new RelayCommand(result.EditStructureExecuteCommand, result.CanExecuteCommand);
                return result;

            }
            public static EditCommand RuleCommand(Document doc, Composition root, BlobEditorWindow editor)
            {
                var result = new EditCommand(doc, root, editor, null);
                result.Command = new RelayCommand(result.EditRuleExecuteCommand, result.CanExecuteCommand);
                return result;
            }
            public static EditCommand DeleteStructureCommand(Document doc, Composition root, BlobEditorWindow editor)
            {
                var result = new EditCommand(doc, root, editor, null);
                result.Command = new RelayCommand(result.DeleteStructureExecuteCommand, result.CanExecuteCommand);
                return result;
            }
            protected EditCommand(Document doc, Composition root, BlobEditorWindow editor, ICommand command)
            {
                _root_composition = root;
                _doc = doc;
                _editor = editor;
            }
            private void CreateStructureExecuteCommand(object parameter)
            {
                Composition root = parameter as Composition;
                CreateComposition ce_w_func;
                StructureDicts.CreatorDict.TryGetValue(root.GetType(), out ce_w_func);
                ICompositionShapitor ce_w = ce_w_func(_doc, root);
                if (ce_w == null)
                    return;
                if (ce_w.ShowDialog() != true)
                    return;
                Composition result = ce_w.GetResultComposition();
                if (result == null)
                    return;
                _root_composition.addChild(result);
                _editor.refreshSctuctureStack();
            }
            private void EditStructureExecuteCommand(object parameter)
            {
                
            }
            private void EditRuleExecuteCommand(object parameter)
            {
                throw new NotImplementedException();
            }
            private void DeleteStructureExecuteCommand(object parameter)
            {
                _root_composition.removeChild(parameter as Composition);
                _editor.refreshSctuctureStack();
            }
            private bool CanExecuteCommand(object parameter)
            {
                return true; // Здесь можно добавить логику для определения, может ли команда быть выполнена
            }

        }
    }
}
