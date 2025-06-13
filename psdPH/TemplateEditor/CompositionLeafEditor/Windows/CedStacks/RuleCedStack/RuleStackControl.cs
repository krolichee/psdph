using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Ruleset.Rules;
using psdPH.RuleEditor;
using psdPH.Utils;
using System.Windows;
using System.Windows.Input;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public class RuleStackControl : CEDStackControl<Rule>
    {
        readonly RuleCommand RuleCommand;
        public override ICommand DeleteCommand() =>
            RuleCommand.DeleteCommand;

        public override ICommand EditCommand() =>
            RuleCommand.EditCommand;
        public RuleStackControl(Rule rule, RuleCommand ruleCommand)
        {
            RuleCommand = ruleCommand;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            var rtb = new RuleTextBlock((Rule)rule);
            //Height = 28;
            Content = rtb;
            CommandParameter = rule;
            Command = EditCommand();
            setContextMenu(this, rule);
        }
    }
}
