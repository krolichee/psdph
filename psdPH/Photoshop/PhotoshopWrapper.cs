using Photoshop;
using psdPH.Logic;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Application = Photoshop.Application;

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
        static bool isPathIs(this Document doc, string path)
        {
            if (doc.IsNonFile())
                return false;
            return doc.GetDocPath() == path;
        }
        public static Document Opened(string path)
        {
             bool hasDocs = PhotoshopWrapper.HasOpenDocuments();
            return hasDocs ? PhotoshopWrapper.GetPhotoshopApplication().Documents.Cast<Document>().FirstOrDefault(d => d.isPathIs(path)):null;
        }


        
        // Открывает PSD-файл
        public static Document OpenDocument(string filePath, bool reopenIfOpened = false)
        {
            Document doc = Opened(filePath);
            if (reopenIfOpened ? doc != null : false)
            {
                if (!doc.Saved)
                {
                    var dialogResult = MessageBox.Show("Документ имеет несохранённые изменения. Сохранить?", "", MessageBoxButton.YesNoCancel);
                    if (dialogResult == MessageBoxResult.Yes)
                        doc.Save();
                    else if (dialogResult == MessageBoxResult.Cancel)
                        return null;
                }
                doc.Close(PsSaveOptions.psDoNotSaveChanges);
            }
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
