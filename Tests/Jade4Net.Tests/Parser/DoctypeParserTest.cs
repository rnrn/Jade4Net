using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class DoctypeParserTest : ParserTest
    {
        [TestMethod]
        public void shouldReturnDoctype()
        {
            loadInParser("doctype.jade");
            var blockNode = (BlockNode) root;
            Assert.IsNotNull(blockNode.getNodes());

            Node node = blockNode.pollNode();
            Assert.AreEqual(node.getValue(), ("strict"));
            Assert.AreEqual(blockNode.hasNodes(), (false));
        }

    }
}