using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Views.WeekView.Logic;
using System.Windows.Input;

namespace psdPH.Views.WeekView
{
    class RenderCommand
    {
        public ICommand Command { get; set; }
        public RenderCommand()
        {
            Command = new RelayCommand(RenderExecuteCommand, CanExecuteCommand);
        }
        private void RenderExecuteCommand(object parameter)
        {
            WeekRenderer.RenderWeek((WeekData)parameter);
        }
        private bool CanExecuteCommand(object parameter)
        {
            return true;
        }
    }
}
