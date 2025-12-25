using Photoshop;
using psdPH.Logic;
using psdPH.Photoshop;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Application = Photoshop.Application;

namespace psdPH.Photoshop
{
    //TODO Одиночка
    //TODO Диспоз маршала после использования
    public class PhotoshopWrapper : IPhotoshopWrapper
    {
        Application psApp;
        static PhotoshopWrapper instance;
        public static PhotoshopWrapper Instance => instance ?? (instance = new PhotoshopWrapper());
        public Application GetPhotoshopApplication()
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
        public void Dispose()
        {
            if (psApp != null)
                Marshal.ReleaseComObject(psApp);
        }
        //TODO Переименовать
        public DocumentWr Opened(string path)
        {
            bool hasDocs = HasOpenDocuments();
            var docs = DocumentWr.GetDocs(GetPhotoshopApplication());
            return hasDocs ? docs.FirstOrDefault(d => d.IsPathPresent(path)) : null;
        }



        // Открывает PSD-файл
        public DocumentWr OpenDocumentWr(string filePath, bool reopenIfOpened = false)
        {
            DocumentWr docWr = Opened(filePath);
            if (reopenIfOpened ? docWr != null : false)
            {
                if (!docWr.Saved)
                {
                    var dialogResult = MessageBox.Show("Документ имеет несохранённые изменения. Сохранить?", "", MessageBoxButton.YesNoCancel);
                    if (dialogResult == MessageBoxResult.Yes)
                        docWr.Save();
                    else if (dialogResult == MessageBoxResult.Cancel)
                        return null;
                }
                docWr.Close(PsSaveOptions.psDoNotSaveChanges);
            }
            GetPhotoshopApplication().Open(filePath);
            return psApp.ActiveDocument.Wrapper();
        }
        public Document OpenDocument(Application psApp, string filePath)
        {
            psApp.Open(filePath);
            return psApp.ActiveDocument;
        }
        // Проверяет, открыто ли приложение Photoshop
        public bool IsPhotoshopRunning()
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
        public bool HasOpenDocuments()
        {
            return HasOpenDocuments(GetPhotoshopApplication());
        }
        public bool HasOpenDocuments(Application psApp)
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

        public DocumentWr GetActiveDocument()
        {
            return GetPhotoshopApplication().ActiveDocument.Wrapper();
        }
    }
}
