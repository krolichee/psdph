using static psdPH.Logic.PhotoshopDocumentExtension;
using static psdPH.Photoshop.LayerWr;

namespace psdPH.Logic
{
    public static partial class PhotoshopLayerExtension
    {
        public struct AlignOptions
        {
            public Alignment Alignment;
            public ConsiderFx ConsiderFx;

            public AlignOptions(Alignment alignment, ConsiderFx considerFx)
            {
                Alignment = alignment;
                ConsiderFx = considerFx;
            }

            public static AlignOptions Default => new AlignOptions { ConsiderFx = ConsiderFx.NoFx, Alignment = Alignment.Default };
        }



    }
}
