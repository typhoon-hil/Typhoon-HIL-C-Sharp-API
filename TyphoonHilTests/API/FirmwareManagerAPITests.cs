using Microsoft.VisualStudio.TestTools.UnitTesting;
using TyphoonHil.API;

namespace TyphoonHilTests.API
{
    [TestClass()]
    public class FirmwareManagerAPITests
    {
        public FirmwareManagerAPITests() { }

        // MANUAL TEST: Add HIL to setups before running this test
        [TestMethod()]
        public void GetHilInfoTest()
        {
            var info = new FirmwareManagerAPI().GetHilInfo();
        }
    }
}