using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic.Parameters
{
    [Serializable]
    public class ParameterSet: ObservableCollection<Parameter>,ISerializable
    {
        public event Action Updated;
        public void Add(Parameter[] rules)
        {
            foreach (var rule in rules)
                Add(rule);
        }
        public ParameterSet():base(){
            CollectionChanged += (_, __) => Updated?.Invoke();
        }
        public T[] GetByType<T>()
        {
            return this.Where(l => l is T).Cast<T>().ToArray();
        }
    }
}
