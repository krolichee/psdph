using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace psdPH.Logic.Parameters
{
    public static class ParameterSetExtension
    {
        public static void Add(this ParameterSet ps, Parameter parameter) => ps.AsCollection().Add(parameter);
    }
    [Serializable]
    public class ParameterSet: ISerializable
    {
        public ObservableCollection<Parameter> Parameters = new ObservableCollection<Parameter>();
        public event Action Updated;
        public void Add(Parameter[] rules)
        {
            foreach (var rule in rules)
                Parameters.Add(rule);
        }
        public ParameterSet():base(){
            Parameters.CollectionChanged += (_, __) => Updated?.Invoke();
        }
        public T[] GetByType<T>()
        {
            return Parameters.Where(l => l is T).Cast<T>().ToArray();
        }
        public Parameter[] GetByType(Type type)
        {
            return Parameters.Where(l => l.GetType() == type).ToArray();
        }
        public Dictionary<string,Parameter> ToDictionary()=>
            Parameters.ToDictionary(p => p.Name, p=>p);
        public void Set(string name,object value)
        {
            Parameters.First(p => p.Name == name).Value = value;
            Updated?.Invoke();
        }
        public void Set(Parameter parameter, object value)
        {
            if (!Parameters.Contains(parameter))
                throw new ArgumentException();
            parameter.Value = value;
            Updated?.Invoke();
        }
        public void Import(Parameter parameter)
        {
            GetByType(parameter.GetType()).First(p => p.Name == parameter.Name).Import(parameter);
        }

        public ParameterSet Clone()
        {
            var result = new ParameterSet();
            foreach (var par in Parameters)
                result.Parameters.Add(par.Clone());
            return result;
        }
        public Collection<Parameter> AsCollection() => Parameters;

        internal void Import(ParameterSet savedParameters)
        {
            foreach (var parameter in savedParameters.AsCollection())
                Import(parameter);
            
        }
    }
}
