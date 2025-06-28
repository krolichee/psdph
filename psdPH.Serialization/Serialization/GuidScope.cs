using psdPH.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Logic.Serialization
{
    public class GuidScope
    {
        static GuidScope _current;
        public static GuidScope Current => _current;
        static bool loading = false;
        public event Action GuidsLoaded;
        public void Add(Guided guided)
        {
            if (loading)
                scope.Add(guided);
        }
        public void EndOfLoad()
        {
            GuidsLoaded?.Invoke();
            scope.Clear();
            GuidsLoaded = null;
            loading = false;
        }
        public static void StartLoad()
        {
            loading = true;
            _current = new GuidScope();
        }
        public Guided GetByGuid(Guid guid)
        {
            return scope.First(g => g.Guid == guid);
        }
        private static List<Guided> scope = new List<Guided>();
    }
}
