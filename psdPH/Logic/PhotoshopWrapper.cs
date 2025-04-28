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
using psdPH.CollectionEditor;
using System.Runtime.InteropServices;

namespace psdPH
{
    public static class PhotoshopWrapper
    {
        private static Application psApp;
        
        // Конструктор: создает экземпляр Photoshop
        public static Application GetPhotoshopApplication()
        {
            if (psApp == null)
            {
                Type psType = Type.GetTypeFromProgID("Photoshop.Application");
                psApp = Activator.CreateInstance(psType) as Application;
                psApp.Visible = true;
            }
            return psApp;
        }
        public static void Dispose()
        {
            if (psApp!= null)
                Marshal.ReleaseComObject(psApp);
        }

        // Открывает PSD-файл
        public static Document OpenDocument(Application psApp, string filePath)
        {  
            psApp.Open(filePath);
            return psApp.ActiveDocument;
        }
        // Проверяет, открыто ли приложение Photoshop
        public static bool IsPhotoshopRunning()
        {
            try
            {
                // Пытаемся получить запущенный экземпляр Photoshop
                psApp = Marshal.GetActiveObject("Photoshop.Application") as Application;
                return psApp != null;
            }
            catch (COMException)
            {
                // Photoshop не запущен
                return false;
            }
        }

        // Проверяет, есть ли открытые документы в Photoshop
        public static bool HasOpenDocuments()
        {
            return HasOpenDocuments(GetPhotoshopApplication());
        }
        public static bool HasOpenDocuments(Application psApp)
        {
            if (psApp == null)
                return false;

            try
            {
                return psApp.Documents.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
