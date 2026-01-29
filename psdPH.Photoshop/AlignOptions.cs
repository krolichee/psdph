
using psdPH.Alignments;

namespace psdPH.Photoshop
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
