using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    class DtoConverterRegistrator
    {
        public static void InitializeRegistry()
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
                        if (typeof(DtoConverter).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            //TODO добавить фабрику или вроде того
                            var converter = Activator.CreateInstance(type,
    nonPublic: true) as DtoConverter;
                            DtoConvertersRegistry.Register(converter);
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
