using System.Windows.Input;

namespace psdPH
{
    public abstract class CEDCommand
    {

        public ICommand CreateCommand => new RelayCommand(CreateExecuteCommand, (_) => true);
        public ICommand EditCommand => new RelayCommand(EditExecuteCommand, IsEditableCommand);
        public ICommand DeleteCommand => new RelayCommand(DeleteExecuteCommand, (_) => true);
        protected virtual bool IsEditableCommand(object parameter) { return true; }
        protected virtual void CreateExecuteCommand(object parameter) { }
        protected virtual void EditExecuteCommand(object parameter) { }
        protected virtual void DeleteExecuteCommand(object parameter) { }

    }
}

