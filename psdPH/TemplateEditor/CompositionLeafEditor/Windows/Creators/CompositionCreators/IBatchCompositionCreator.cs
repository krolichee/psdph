namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    public interface IBatchCompositionCreator
    {
        Composition[] GetResultBatch();
        bool? ShowDialog();
    }
}
