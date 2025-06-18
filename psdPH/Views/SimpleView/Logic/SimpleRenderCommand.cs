using psdPH.Views.WeekView.Logic;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace psdPH.Views.SimpleView.Logic
{
    class SimpleRenderCommand
    {
        public ICommand Command { get; set; }
        public SimpleRenderCommand()
        {
            Command = new RelayCommand(RenderExecuteCommand, CanExecuteCommand);
        }
        private void RenderExecuteCommand(object parameter)
        {
            SimpleRenderer.Render((SimpleData)parameter);
        }
        private bool CanExecuteCommand(object parameter)
        {
            return true;
        }
    }
}
