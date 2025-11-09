using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using psdPH.Serialization;

namespace test.Serialization
{
    [TestClass]
    public class DtoTypesRegistratorTest
    {
        public class RegistredDto : Dto { }
        [TestMethod]
        public void testRegistration()
        {
            DtoTypesRegistrator.InitializeRegistry();
        }

    }
}
