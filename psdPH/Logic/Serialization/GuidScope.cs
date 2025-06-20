using psdPH.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace psdPH.Logic.Serialization
{
    public static class GuidScope
    {
        static bool loading = false;
        public static event Action GuidsLoaded;
        public static void Add(Guided guided)
        {
            if (loading)
                scope.Add(guided);
        }
        public static void EndOfLoad()
        {
            GuidsLoaded?.Invoke();
            scope.Clear();
            loading = false;
        }
        public static void StartLoad()
        {
            loading = true;
        }
        public static Guided GetByGuid(Guid guid)
        {
            return scope.First(g => g.Guid == guid);
        }
        private static List<Guided> scope = new List<Guided>();
    }
}
