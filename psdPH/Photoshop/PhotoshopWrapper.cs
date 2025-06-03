using Photoshop;
using System;
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
                var psAppCom__ = Activator.CreateInstance(psType);
               psApp = psAppCom__ as Application;
            }
            if (psApp == null)
            {
                var psAppCom__ = Marshal.GetActiveObject("Photoshop.Application");
                psApp = psAppCom__ as Application;
            }
            psApp.DisplayDialogs = PsDialogModes.psDisplayNoDialogs;
            psApp.Visible = true;
            return psApp;
        }
        public static void Dispose()
        {
            if (psApp != null)
                Marshal.ReleaseComObject(psApp);
        }

        // Открывает PSD-файл
        public static Document OpenDocument(string filePath)
        {
            GetPhotoshopApplication().Open(filePath);
            return psApp.ActiveDocument;
        }
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
