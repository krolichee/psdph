using Photoshop;
using psdPH.Logic;
using System.IO;

namespace psdPH.Views.WeekView.Logic
{
    public class OutputSaver
    {
        string _path;
        public OutputSaver(string path)
        {
            _path = path;
        }
        private OutputSaver() { }
        public void Save(Document doc)
        {
            var outputName = Path.GetFileName(_path);
           
            var pngPath = Path.Combine(_path, outputName+ ".png");
            var psdPath = Path.Combine(_path, outputName+ ".psd");
            try
            {
                doc.SaveAs(pngPath, new PNGSaveOptions(), true, PsExtensionType.psLowercase);
            }
            catch
            {
                //Interop даже успешно сохраняет с ошибкой. это фотошоп, детка
            }
            doc.SaveAs(psdPath, new PhotoshopSaveOptions(),true,PsExtensionType.psLowercase);
        }

    }
}
