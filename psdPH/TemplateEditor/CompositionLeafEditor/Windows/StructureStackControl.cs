using psdPH.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    class StructureStackControl : TemplateStackControl<Composition>
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
            var grid = new Grid();
            grid.Children.Add(new Label()
            {
                Content = composition.UIName,
                Foreground = SystemColors.ActiveBorderBrush,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            grid.Children.Add(new Label()
            {
                Content = composition.ObjName,
                Foreground = SystemColors.ActiveCaptionTextBrush,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            Height = 28;
            Content = grid;
            CommandParameter = composition;
            Command = editCommand;
            setMenu(this, composition);
        }
    }
}
