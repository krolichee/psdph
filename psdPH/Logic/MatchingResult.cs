using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic
{
    public class MatchingResult
    {
        public List<Composition> MismatchRoute= new List<Composition>();
        public bool Match=true;
        string GetRouteString(List<Composition> compositions)
        {
            var route = compositions.GetRange(0, compositions.Count - 1).Select(c => c.ObjName).ToArray();
            var last = compositions.Last();
            return string.Join("/", route) + $"/[{last.UIName}]{last.ObjName}";
        }
        public override string ToString()
        {
            return GetRouteString(MismatchRoute);
        }
        public MatchingResult(Composition cmp, bool match)
        {
            Match = match;
            MismatchRoute.Add(cmp);
        }
        public static implicit operator bool(MatchingResult mr)=> mr.Match;
        
    }
}
