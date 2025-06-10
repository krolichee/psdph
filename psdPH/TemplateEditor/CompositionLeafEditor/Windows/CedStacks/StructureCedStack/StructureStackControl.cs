using psdPH.Utils;
using System.Windows;
using System.Windows.Input;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
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
            Content = TypeAndNameGrid.Get(composition.UIName, composition.ObjName);
            CommandParameter = composition;
            Command = editCommand;
            setContextMenu(this, composition);
        }
    }
}
