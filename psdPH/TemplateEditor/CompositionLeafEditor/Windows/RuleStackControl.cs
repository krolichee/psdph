using psdPH.RuleEditor;
using psdPH.Utils;
using System.Windows;
using System.Windows.Input;
using Rule = psdPH.Logic.Rule;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public class RuleStackControl : TemplateStackControl<Rule>
    {
        public override ICommand DeleteCommand() =>
            new RuleCommand(Context).DeleteCommand;

        public override ICommand EditCommand() =>
            new RuleCommand(Context).EditCommand;
        public RuleStackControl(Rule rule, PsdPhContext context) : base(context)
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            ICommand editCommand = EditCommand();
            ICommand deleteCommand = DeleteCommand();
            var rtb = new RuleTextBlock((Rule)rule);
            //Height = 28;
            Content = rtb;
            CommandParameter = rule;
            Command = editCommand;
            setMenu(this, rule);
        }
    }
}
