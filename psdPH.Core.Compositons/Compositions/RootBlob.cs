using psdPH.Photoshop;
using psdPH.Utils;
using System;
using psdPH.Setups;
using psdPH.Nodes.Core;

namespace psdPH.Logic.Compositions
{
    public class RootBlob : Composition
    {
        public RootBlob():base() { }
        
        //TODO имена из локализации
        public override string Name => "Главный документ";

        public override void Apply(DocumentWr doc)
        {
            throw new NotImplementedException();
        }

        public override bool IsMatching(DocumentWr doc) => true;
        
        public override MatchingResult IsMatchingRouted(DocumentWr doc)
        {
            MatchingResult result = this.Match(doc);
            if (!result)
                return result;
            var childrenMatch = DocumentMatcher.Match(Hierarchy.Children, doc);
            if (childrenMatch == null)
                return result;
            else
                return childrenMatch.Value;
        }
    }
    
}
    
