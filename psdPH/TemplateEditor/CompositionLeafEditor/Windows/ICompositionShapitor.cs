namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public interface ICompositionShapitor
    {
        Composition GetResultComposition();
        bool? ShowDialog();
    }

    public enum EditorMode
    {
        Edit,
        Create
    }
}
