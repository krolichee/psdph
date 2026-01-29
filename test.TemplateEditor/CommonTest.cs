using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH;
using psdPH.Nodes;
using psdPH.Nodes.Core;
using psdPH.Project;
using psdPH.TemplateEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace test.TemplateEditor

{
    [TestClass]
    public class CommonTest
    {
        [TestMethod]
        public void testOpen()
        {
            PsdPhDirectories.SetBaseDirectory(Directory.GetCurrentDirectory());
            var project =  PsdPhProject.MakeInstance("testProject");
            var blob = project.createMainBlob();
            blob.NodeSet.Nodes.Add(new MuxNode());
            project.saveBlob(blob);
            var te_w = TemplateEditorWindow.OpenFromDisk();
            te_w.ShowDialog();
        }
    }
}
