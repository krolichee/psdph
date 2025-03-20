using psdPH.Logic.Compositions;
using psdPH.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH
{
    partial class MainWindow
    {
        private void saveBlob(Blob blob)
        {
            string xmlFilePath = Path.Combine(Directories.ProjectDirectory, "template.xml");
            DiskOperations.saveXml<Blob>(xmlFilePath, blob);
        }
        private Blob openMainBlob()
        {
            Blob blob;
            string xmlFilePath = Path.Combine(Directories.ProjectDirectory, "template.xml");
            string psdFilePath = Path.Combine(Directories.ProjectDirectory, "template.psd");
            if (File.Exists(xmlFilePath))
            {
                blob = DiskOperations.openXml<Blob>(xmlFilePath);
                blob.Restore();
            }
            else
                blob = Blob.PathBlob(psdFilePath);
            return blob;
        }

    }
}
