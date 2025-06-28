using psdPH.Photoshop;
using psdPH.Utils;
using System;
using psdPH.Setups;
using psdPH.Nodes.Core;

namespace psdPH.Logic.Compositions
{
    [UIName("Главный документ")]
    public class RootBlob : Composition
    {
        public RootBlob()
        {
            DtoConvertersRegistry.Register<RootBlob>(new NullDtoConverter());
            SetupsRegistry.Register<RootBlob>(new EmptySetupsSource());
        }
        public NodeSet NodeSet = new NodeSet();

        public override string ObjName => "Главный документ";

        public override void Apply(DocumentWr doc)
        {
            throw new NotImplementedException();
        }

        public override bool IsMatching(DocumentWr doc) => true;
        
        public override MatchingResult IsMatchingRouted(DocumentWr doc)
        {
            MatchingResult result = new MatchingResult(this, IsMatching(doc));
            if (!result)
                return result;
            matchChildren(result, doc);
            return result;
        }
    }
    
}
    
