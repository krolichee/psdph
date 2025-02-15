using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using Photoshop;
using System.Reflection.Emit;

namespace psdPH
{

    public class PSDLayer
    {
        public int id;
        public string name;
        public PsLayerKind kind;
        public dynamic dyn;
        public PSDLayer(int id, string name, PsLayerKind kind, dynamic dyn)
        {
            this.id = id;
            this.name = name;
            this.kind = kind;
            this.dyn = dyn;
        }
        public static PSDLayer[] FilterByKinds(PSDLayer[] layers, PsLayerKind[] kinds)
        {
            bool filter(PSDLayer layer)
            {
                foreach (var kind in kinds) if (layer.kind == kind) return true; return false;
            }
            return layers.Where(filter).ToArray();
        }
        public static string[] GetLayersNames(PSDLayer[] layers)
        {
            string[] layer_names = layers.Select(l => l.name).ToArray();
            return layer_names;
        }
        public PSDLayer(dynamic layer)
        {
            this.id = (int)layer.Id;
            this.name = (string)layer.Name;
            this.kind = (PsLayerKind)layer.Kind;
            this.dyn = layer;
        }
        public PSDLayer(dynamic layer, PsLayerKind viol_kind)
        {
            this.id = (int)layer.Id;
            this.name = (string)layer.Name;
            this.kind = viol_kind;
            this.dyn = layer;
        }
    }
    public class PhotoshopWrapper : IDisposable
    {
        private dynamic _psApp;
        private bool _disposed = false;

        // Конструктор: создает экземпляр Photoshop
        public PhotoshopWrapper()
        {
            Type psType = Type.GetTypeFromProgID("Photoshop.Application");
            _psApp = Activator.CreateInstance(psType);
            _psApp.Visible = true; // Делаем Photoshop видимым
        }

        // Открывает PSD-файл
        public void OpenDocument(string filePath)
        {
            if (_disposed)
                throw new ObjectDisposedException("PhotoshopWrapper");

            _psApp.Open(filePath);
        }

        // Сохраняет активный документ в PSD
        public void SaveDocument(string savePath)
        {
            if (_disposed)
                throw new ObjectDisposedException("PhotoshopWrapper");

            dynamic doc = _psApp.ActiveDocument;
            doc.SaveAs(savePath, new PsSaveOptions(), true, PsExtensionType.psLowercase);
        }

        // Получает имена слоев
        public List<PSDLayer> GetLayers()
        {
            if (_disposed)
                throw new ObjectDisposedException("PhotoshopWrapper");

            dynamic doc = _psApp.ActiveDocument;
            var layerNames = new List<PSDLayer>();

            // Обрабатываем обычные слои
            foreach (dynamic layer in doc.ArtLayers)
            {
                layerNames.Add(new PSDLayer(layer));
            }

            // Обрабатываем группы слоев
            foreach (dynamic layerSet in doc.LayerSets)
            {
                layerNames.Add(new PSDLayer(layerSet,PsLayerKind.psNormalLayer));
                ProcessLayerSet(layerSet, layerNames);
            }

            return layerNames;
        }

        // Рекурсивно обрабатывает группы слоев
        private void ProcessLayerSet(dynamic layerSet, List<PSDLayer> layerNames)
        {
            foreach (dynamic layer in layerSet.ArtLayers)
            {

                layerNames.Add(new PSDLayer(layer));
            }

            foreach (dynamic nestedLayerSet in layerSet.LayerSets)
            {
                layerNames.Add(new PSDLayer(nestedLayerSet,PsLayerKind.psNormalLayer));
                ProcessLayerSet(nestedLayerSet, layerNames);
            }
        }

        // Освобождает ресурсы
        public void Dispose()
        {
            if (!_disposed)
            {
                if (_psApp != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(_psApp);
                    _psApp = null;
                }
                _disposed = true;
            }
        }

        public string GetDocPath()
        {
            dynamic doc = _psApp.ActiveDocument;
            return doc.FullName;
        }
        public dynamic GetLayerByName(string layerName)
        {
            if (_disposed)
                throw new ObjectDisposedException("PhotoshopWrapper");

            dynamic doc = _psApp.ActiveDocument;

            // Ищем слой в обычных слоях
            foreach (dynamic layer in doc.ArtLayers)
            {
                if ((string)layer.Name == layerName)
                    return layer;
            }

            // Ищем слой в группах слоев
            foreach (dynamic layerSet in doc.LayerSets)
            {
                var foundLayer = FindLayerByNameInLayerSet(layerSet, layerName);
                if (foundLayer != null)
                    return foundLayer;
            }

            return null; // Слой не найден
        }

        private dynamic FindLayerByNameInLayerSet(dynamic layerSet, string layerName)
        {
            // Ищем слой в обычных слоях группы
            foreach (dynamic layer in layerSet.ArtLayers)
            {
                if ((string)layer.Name == layerName)
                    return layer;
            }

            // Ищем слой во вложенных группах
            foreach (dynamic nestedLayerSet in layerSet.LayerSets)
            {
                var foundLayer = FindLayerByNameInLayerSet(nestedLayerSet, layerName);
                if (foundLayer != null)
                    return foundLayer;
            }

            return null; // Слой не найден
        }
        public PSDLayer[] GetLayersByKinds(PsLayerKind[] kinds)
        {
            var layers = GetLayers().ToArray();
            layers = PSDLayer.FilterByKinds(layers, kinds);
            return layers;
        }
        public string ExtractSmartObject(int layerId)
        {
            if (_disposed)
                throw new ObjectDisposedException("PhotoshopWrapper");

            dynamic doc = _psApp.ActiveDocument;

            // Находим слой по ID
            dynamic layer = FindLayerById(doc, layerId);

            if (layer == null)
                throw new ArgumentException("Layer with the specified ID not found.");

            // Проверяем, является ли слой смарт-объектом
            if ((PsLayerKind)layer.Kind != PsLayerKind.psSmartObjectLayer)
                throw new InvalidOperationException("The specified layer is not a smart object.");

            // Создаем временный файл для экспорта смарт-объекта
            string tempFilePath = Path.GetTempFileName();
            tempFilePath = Path.ChangeExtension(tempFilePath, ".psb"); // Используем расширение .psb для смарт-объектов
   // Экспортируем смарт-объект во временный файл
            layer.ExportContents(tempFilePath);
            return tempFilePath;
        }
        
        private dynamic FindLayerById(dynamic doc, int layerId)
        {
            // Ищем слой в обычных слоях
            foreach (dynamic layer in doc.ArtLayers)
            {
                if (layer.Id == layerId)
                    return layer;
            }

            // Ищем слой в группах слоев
            foreach (dynamic layerSet in doc.LayerSets)
            {
                dynamic foundLayer = FindLayerInLayerSet(layerSet, layerId);
                if (foundLayer != null)
                    return foundLayer;
            }

            return null;
        }
        private dynamic FindLayerInLayerSet(dynamic layerSet, int layerId)
        {
            // Ищем слой в обычных слоях группы
            foreach (dynamic layer in layerSet.ArtLayers)
            {
                if (layer.Id == layerId)
                    return layer;
            }

            // Ищем слой во вложенных группах
            foreach (dynamic nestedLayerSet in layerSet.LayerSets)
            {
                dynamic foundLayer = FindLayerInLayerSet(nestedLayerSet, layerId);
                if (foundLayer != null)
                    return foundLayer;
            }

            return null;
        }
        static void EditSmartObjectContent(dynamic smartObjectLayer)
        {
            try
            {
                // Извлекаем содержимое смарт-объекта во временный файл
                string tempFilePath = System.IO.Path.GetTempFileName();
                smartObjectLayer.ExportContent(tempFilePath);

                Console.WriteLine($"Содержимое смарт-объекта извлечено в: {tempFilePath}");

                // Открываем извлеченный файл в Photoshop
                Type psType = Type.GetTypeFromProgID("Photoshop.Application");
                dynamic psApp = Activator.CreateInstance(psType);
                psApp.Visible = true;
                dynamic extractedDoc = psApp.Open(tempFilePath);

                Console.WriteLine($"Извлеченный файл открыт: {extractedDoc.Name}");

                // Редактируем извлеченный файл (например, добавляем новый слой)
                dynamic newLayer = extractedDoc.ArtLayers.Add();
                newLayer.Name = "Edited Layer";
                Console.WriteLine("Добавлен новый слой в извлеченный файл.");

                // Сохраняем изменения в извлеченном файле
                extractedDoc.Save();
                extractedDoc.Close(PsSaveOptions.psDoNotSaveChanges);

                // Освобождаем ресурсы
                System.Runtime.InteropServices.Marshal.ReleaseComObject(psApp);
                psApp = null;

                // Обновляем смарт-объект в основном документе
                smartObjectLayer.ReplaceContents(tempFilePath);
                Console.WriteLine("Смарт-объект обновлен.");

                // Удаляем временный файл
                System.IO.File.Delete(tempFilePath);
                Console.WriteLine("Временный файл удален.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе со смарт-объектом: {ex.Message}");
            }
        }

        // Деструктор (на случай, если Dispose не был вызван)
        ~PhotoshopWrapper()
        {
            Dispose();
        }
    }
}
