using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Serialization
{
    [TestClass]
    public partial class IherarchyConverterTests
    {

        [TestMethod]
        public void SerializationTest()
        {
            DtoConverterRegistrator.InitializeRegistry();
            var entity = new RefEntity();
            entity.Ref1 = new SimpleEntity(1);
            entity.Ref2 = new SimpleEntity(2);
            var dtoScope = IherarchyConverter.GetRelatedDtoScopeFromRootEntity(entity);
            Assert.IsTrue(dtoScope.Scope.Any(o => (o is SimpleDto) ? (o as SimpleDto).a == 1 : false));
            Assert.IsTrue(dtoScope.Scope.Any(o => (o is SimpleDto) ? (o as SimpleDto).a == 2 : false));
        }

    }
}
