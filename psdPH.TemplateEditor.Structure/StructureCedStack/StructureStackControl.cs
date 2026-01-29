using psdPH.Context;
using psdPH.Localization;
using psdPH.TemplateEditor.Core;
using System.Windows;
using System.Windows.Input;

namespace psdPH.TemplateEditor.Structure
{
    partial class StructureStackControl : TemplateStackControl<Composition>
    {
        public override ICommand DeleteCommand() =>
            new StructureCommand(Context).DeleteCommand;
        public override ICommand EditCommand() =>
            new StructureCommand(Context).EditCommand;
        public StructureStackControl(Composition composition, PsdPhContext context) : base(context)
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            _doc = context.doc;
            _root = context.root;
            ICommand editCommand = EditCommand();
            ICommand deleteCommand = DeleteCommand();
            
            Height = 28;
            Content = TypeAndNameGrid.Get(LocalizationService.Localize(composition.GetType()), composition.Name);
            CommandParameter = composition;
            Command = editCommand;
            setContextMenu(this, composition);
        }
    }
}
