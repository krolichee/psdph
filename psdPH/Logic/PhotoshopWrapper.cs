using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH
{
    using System;
    using System.Windows.Controls;
    using Photoshop;
    public class PSDLayer
    {
        public int id;
        public string name;
        public PsLayerKind kind;
        public PSDLayer(int id, string name, PsLayerKind kind)
        {
            this.id = id;
            this.name = name;
            this.kind = kind;
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
        public List<PSDLayer> GetLayerNames()
        {
            if (_disposed)
                throw new ObjectDisposedException("PhotoshopWrapper");

            dynamic doc = _psApp.ActiveDocument;
            var layerNames = new List<PSDLayer>();

            // Обрабатываем обычные слои
            foreach (dynamic layer in doc.ArtLayers)
            {
                layerNames.Add(new PSDLayer(layer.Id,(string)layer.Name,(PsLayerKind)layer.Kind));
            }

            // Обрабатываем группы слоев
            foreach (dynamic layerSet in doc.LayerSets)
            {
                layerNames.Add(new PSDLayer(layerSet.Id, (string)layerSet.Name,PsLayerKind.psNormalLayer));
                ProcessLayerSet(layerSet, layerNames);
            }

            return layerNames;
        }

        // Рекурсивно обрабатывает группы слоев
        private void ProcessLayerSet(dynamic layerSet, List<PSDLayer> layerNames)
        {
            foreach (dynamic layer in layerSet.ArtLayers)
            {

                layerNames.Add(new PSDLayer(layerSet.Id, (string)layerSet.Name, PsLayerKind.psNormalLayer));
            }

            foreach (dynamic nestedLayerSet in layerSet.LayerSets)
            {
                layerNames.Add(new PSDLayer(layerSet.Id, (string)layerSet.Name, PsLayerKind.psNormalLayer));
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

        // Деструктор (на случай, если Dispose не был вызван)
        ~PhotoshopWrapper()
        {
            Dispose();
        }
    }
}
