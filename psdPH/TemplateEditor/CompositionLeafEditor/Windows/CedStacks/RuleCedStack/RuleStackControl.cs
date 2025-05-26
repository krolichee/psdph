using Photoshop;
using psdPH.Logic;
using psdPH.RuleEditor;
using psdPH.Utils;
using System.Windows;
using System.Windows.Input;
using Rule = psdPH.Logic.Rule;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public class RuleStackControl : CEDStackControl<Rule>
    {
        RuleSet RuleSet;
        public override ICommand DeleteCommand() =>
            new StructureRuleCommand(RuleSet).DeleteCommand;

        public override ICommand EditCommand() =>
            new StructureRuleCommand(RuleSet).EditCommand;
        public RuleStackControl(Rule rule, RuleSet ruleSet,Document doc)
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
            setContextMenu(this, rule);
        }
    }
}
