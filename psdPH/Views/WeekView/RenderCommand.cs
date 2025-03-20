using Photoshop;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.TemplateEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.BlobEditorWindow;
using System.Windows.Input;
using psdPH.Logic.Compositions;

namespace psdPH.Views.WeekView
{
    class RenderCommand
    {
        private Composition _root_composition;
        private Document _doc;
        private BlobEditorWindow _editor;

        public ICommand Command { get; set; }
        public RenderCommand(Document doc)
        {
            (_doc.ActiveLayer as ArtLayer).Duplicate();
            Command = new RelayCommand(RenderExecuteCommand, CanExecuteCommand);
        }
        private void RenderExecuteCommand(object parameter)
        {
            var blob = parameter as Blob;
        }
        private bool CanExecuteCommand(object parameter)
        {
            return true; // Здесь можно добавить логику для определения, может ли команда быть выполнена
        }
    }
}
