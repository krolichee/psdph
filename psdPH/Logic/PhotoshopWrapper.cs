using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using Photoshop;
using System.Reflection.Emit;
using psdPH.Logic;

namespace psdPH
{
    public class PhotoshopWrapper : IDisposable
    {
        private Photoshop.Application _psApp;
        private bool _disposed = false;
        public Photoshop.Application App => _psApp;

        // Конструктор: создает экземпляр Photoshop
        public PhotoshopWrapper()
        {
            Type psType = Type.GetTypeFromProgID("Photoshop.Application");
            _psApp = Activator.CreateInstance(psType) as Photoshop.Application;
            _psApp.Visible = true;
        }

        // Открывает PSD-файл
        public Document OpenDocument(string filePath)
        {
            if (_disposed)
                throw new ObjectDisposedException("PhotoshopWrapper");

            _psApp.Open(filePath);
            return _psApp.ActiveDocument;
        }
        public PhotoshopDocumentWrapper Doc => new PhotoshopDocumentWrapper(_psApp.ActiveDocument);
        
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

        // Деструктор (на случай, если Dispose не был вызван)
        ~PhotoshopWrapper()
        {
            Dispose();
        }
    }
}
