using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPHTest.Logic.Compositions
{
    [TestClass]
    public class BlobTest
    {

        [TestMethod]
        public void Clone()
        {
            var blob = Blob.PathBlob("---").Clone();
        }
    }
}
