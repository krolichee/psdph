using Photoshop;

namespace psdPH.Photoshop
{
    public interface IPhotoshopWrapper
    {
        void Dispose();
        DocumentWr GetActiveDocument();
        Application GetPhotoshopApplication();
        bool HasOpenDocuments();
        bool HasOpenDocuments(Application psApp);
        bool IsPhotoshopRunning();
        Document OpenDocument(Application psApp, string filePath);
        DocumentWr OpenDocumentWr(string filePath, bool reopenIfOpened = false);
        DocumentWr Opened(string path);
    }
}