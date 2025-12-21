using psdPH.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic
{
    public struct MatchingResult
    {
        readonly bool match;
        readonly List<IDocumentMatchable> mismatchRoute;

        public IEnumerable<IDocumentMatchable> MismatchRoute => mismatchRoute;
        public bool Match => match;
        string GetRouteString()
        {
            var route = mismatchRoute.GetRange(0, mismatchRoute.Count - 1).Select(c => c.ToString()).ToArray();
            var last = mismatchRoute.Last();
            return string.Join("/", route) + $"/[{LocalizationService.Localize(last.GetType())}]{last.ToString()}";
        }
        public override string ToString()
        {
            return GetRouteString();
        }
        public MatchingResult(IDocumentMatchable matching, bool match)
        {
            this.match = match;
            mismatchRoute = new List<IDocumentMatchable>
            {
                matching
            };
        }
        private MatchingResult(IEnumerable<IDocumentMatchable> matchings, bool match)
        {
            this.match = match;
            mismatchRoute = matchings.ToList();
        }
        private MatchingResult(bool match)
        {
            mismatchRoute = new List<IDocumentMatchable>();
            this.match = match;
        }
        public static implicit operator bool(MatchingResult mr) => mr.Match;
        public static MatchingResult operator +(MatchingResult @this,MatchingResult other)
        {
            var match = @this.match & other.match;
            var result = new MatchingResult(@this.mismatchRoute.Concat(other.mismatchRoute),match);
            return result;
        }
    }
}
