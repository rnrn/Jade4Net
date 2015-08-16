using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class LargeBodyTextParserTest : ParserTest
    {
        private Node block;
        private TagNode pTag;

        [TestMethod]
        public void test()
        {
            loadInParser("large_body_text_with_pipes.jade");
            pTag = (TagNode) root.pollNode();
            block = pTag.getBlock();
            Assert.IsNotNull(block.getNodes());
            Assert.IsNotNull(pTag);

            block = pTag.getBlock();
            Assert.IsNotNull(block.getNodes());

            Assert.IsNotNull(block.pollNode().getValue(), "Hello World!");
            Assert.IsNotNull(block.pollNode().getValue(), " Here comes the Message!");

            Assert.IsFalse(block.hasNodes());
        }
    }
}