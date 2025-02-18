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
    public static class PhotoshopWrapper
    {
        // Конструктор: создает экземпляр Photoshop
        public static Application GetPhotoshopApplication()
        {
            Type psType = Type.GetTypeFromProgID("Photoshop.Application");
            Application psApp = Activator.CreateInstance(psType) as Photoshop.Application;
            psApp.Visible = true;
            return psApp;
        }

        // Открывает PSD-файл
        public static Document OpenDocument(Application psApp, string filePath)
        {  
            psApp.Open(filePath);
            return psApp.ActiveDocument;
        }
    }
}
