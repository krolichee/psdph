using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    class DtoTypesRegistrator
    {
        public static void RegisterInitialize() {
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
                        if (typeof(Dto).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            DtoTypesRegistry.Add(type);
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
    }
}
