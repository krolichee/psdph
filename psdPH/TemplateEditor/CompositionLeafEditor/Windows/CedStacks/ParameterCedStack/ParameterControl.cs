using psdPH.Logic.Parameters;
using psdPH.RuleEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.CedStacks.ParameterCedStack
{
    public class ParameterControl:CEDStackControl<Parameter>
    {
        ParameterCommand ParameterCommand;
        public override ICommand DeleteCommand() =>
            ParameterCommand.DeleteCommand;

        public override ICommand EditCommand() =>
            ParameterCommand.EditCommand;
        public ParameterControl(Parameter parameter, ParameterSet parameterSet)
        {
            ParameterCommand = new ParameterCommand(parameterSet);
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            ICommand editCommand = EditCommand();
            ICommand deleteCommand = DeleteCommand();
            
            Height = 28;
            Content = new Label() { Content = parameter.Name};
            CommandParameter = parameter;
            Command = editCommand;
            setContextMenu(this, parameter);
        }
    }
}
