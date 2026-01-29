namespace psdPH.Photoshop
{

    public abstract partial class LayerWr
    {
        public class LayerStyle
        {
            LayerWr source;
            public bool Toggle
            {
                set
                {
                    if (value)
                        source.OnStyle();
                    else
                        source.OffStyle();
                }
            }
            public LayerStyle(LayerWr source)
            {
                this.source = source;
            }
            public void Paste(LayerWr dest)
            {
                if (source.HasStyle())
                {
                    source.CopyStyle();
                    dest.PasteStyle();
                }
            }
        }

    }
}
