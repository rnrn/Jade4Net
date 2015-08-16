using Jade.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class FileNameBuilderTest
    {

        [TestMethod]
        public void testIncludeFileName()
        {
            var fileNameBuilder = new FileNameBuilder("includes/layout");
            Assert.AreEqual("includes/layout.jade", fileNameBuilder.build());
        }
    }
}
