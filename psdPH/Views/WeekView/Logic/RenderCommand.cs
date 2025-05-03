using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Views.WeekView.Logic;
using System.Windows.Input;

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
            _doc = doc;
            Command = new RelayCommand(RenderExecuteCommand, CanExecuteCommand);
        }
        private void RenderExecuteCommand(object parameter)
        {
            WeekRenderer.renderWeek((WeekData)parameter, _doc);
        }
        private bool CanExecuteCommand(object parameter)
        {
            return true;
        }
    }
}
