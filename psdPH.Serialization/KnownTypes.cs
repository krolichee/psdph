using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace psdPH
{
    public static class KnownTypes
    {
        public static void Initialize() { }
        static KnownTypes()
        {
            // Загружаем все сборки, на которые есть ссылки
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

            toLoad.ForEach(path => {
                try { loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))); }
                catch { /* ignore */ }
            });

            foreach (var assembly in loadedAssemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(ISerializable).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            Types.Add(type);
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // Логируйте ошибку для диагностики
                    Console.WriteLine($"Ошибка загрузки типов из сборки {assembly.FullName}: {ex}");
                }
            }
        }
        //TODO Искоренить это безумие
        public static HashSet<Type> Types = new HashSet<Type>();
        //public static void AddTypeToKnownTypes(this object obj)
        //{
        //    Types.Add(obj.GetType());
        //}
    }
}
