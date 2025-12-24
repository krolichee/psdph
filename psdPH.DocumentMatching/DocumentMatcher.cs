using psdPH.Logic;
using psdPH.Photoshop;
using System.Collections.Generic;

namespace psdPH
{
    public static class DocumentMatcher
    {
        public static MatchingResult Match(this IDocumentMatchable matched, DocumentWr doc)
        {
            return new MatchingResult(matched, matched.IsMatching(doc));
        }
        public static MatchingResult? Match(this IEnumerable<IDocumentMatchable> matchings, DocumentWr doc)
        {
            foreach (var child in matchings)
            {
                MatchingResult mr = child.Match(doc);
                if (!mr.Match)
                {
                    return mr;
                }
            }
            return null;
        }

    }
    
    
}

