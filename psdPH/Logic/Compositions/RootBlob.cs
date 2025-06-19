using Photoshop;
using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public override string ObjName => "Главный документ";

        public override void Apply(Document doc)
        {
            throw new NotImplementedException();
        }

        public override bool IsMatching(Document doc) => true;
        
        public override MatchingResult IsMatchingRouted(Document doc)
        {
            MatchingResult result = new MatchingResult(this, IsMatching(doc));
            if (!result)
                return result;
            matchChildren(result, doc);
            return result;
        }
    }
    
}
    
