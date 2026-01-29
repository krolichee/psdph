using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Photoshop
{
    public class LDFilter
    {
        Func<string, bool> layerNamePredicate;
        bool Kinded;
        PsLayerKind[] Kinds;
        bool? IsGroup;
        public LayerDescriptor[] Filter(LayerDescriptor[] lds)
        {
            bool kindFilter(LayerDescriptor ld) => 
                Kinded? 
                ld.Kind == null ? true : Kinds.Contains((PsLayerKind)ld.Kind)
                :true;
            bool layerNameFilter(LayerDescriptor ld) => (layerNamePredicate ?? ((_) => true))(ld.LayerName);
            bool isGroupFilter(LayerDescriptor ld) => IsGroup == null? true : ld.IsGroup == IsGroup;

            IEnumerable<LayerDescriptor> result = lds;
            result = result.Where(isGroupFilter);
            result = result.Where(kindFilter);
            result = result.Where(layerNameFilter);

            return result.ToArray();
        }
        public static LDFilter Group(Func<string, bool> layerNamePredicate = null) 
            => new LDFilter(layerNamePredicate, false, null, true);
        public static LDFilter Layer(PsLayerKind[] kinds, Func<string, bool> layerNamePredicate = null) 
            => new LDFilter(layerNamePredicate, true, kinds, false);
        public static LDFilter Layer(PsLayerKind kind, Func<string, bool> layerNamePredicate = null) 
            => new LDFilter(layerNamePredicate, true, new PsLayerKind[] {kind}, false);

        public static LDFilter Layer(Func<string, bool> layerNamePredicate = null) 
            => new LDFilter(layerNamePredicate, false, null, false);

        protected LDFilter(Func<string, bool> layerNamePredicate, bool kinded, PsLayerKind[] kinds, bool? isGroup)
        {
            this.layerNamePredicate = layerNamePredicate;
            Kinded = kinded;
            Kinds = kinds;
            IsGroup = isGroup;
        }
    }


}

