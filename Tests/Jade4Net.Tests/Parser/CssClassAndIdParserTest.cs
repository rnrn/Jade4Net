using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class CssClassAndIdParserTest : ParserTest
    {

        private Node tag1;
        private Node tag2;
        private Node tag3;
        private Node block;

        [TestMethod]
        public void shouldReturnABlockWithTokens2()
        {
            loadInParser("css_class_and_id.jade");
            block = (BlockNode) root;
            Assert.IsNotNull(block.getNodes());

            // .myclass
            tag1 = block.pollNode();
            Assert.AreEqual(((TagNode) tag1).getAttribute("class"), ("myclass"));
            Assert.AreEqual(block.hasNodes(), (true));

            // #myid.c1.c2.c3
            tag2 = block.pollNode();
            Assert.AreEqual(((TagNode) tag2).getAttribute("id"), ("myid"));
            Assert.AreEqual(((TagNode) tag2).getAttribute("class"), ("c1 c2 c3"));
            Assert.AreEqual(block.hasNodes(), (false));

            // .c1.c2.c3
            // #myid
            block = ((TagNode) tag1).getBlock();
            tag3 = block.pollNode();
            Assert.AreEqual(((TagNode) tag3).getAttribute("class"), ("c1 c2 c3"));
            Assert.AreEqual(block.hasNodes(), (true));
            tag3 = block.pollNode();
            Assert.AreEqual(((TagNode) tag3).getAttribute("id"), ("myid"));
            Assert.AreEqual(block.hasNodes(), (false));

            // div#id1
            block = ((TagNode) tag2).getBlock();
            tag1 = block.pollNode();
            Assert.AreEqual(((TagNode) tag1).getAttribute("id"), ("id1"));
            Assert.AreEqual(block.hasNodes(), (false));

            // span#id2.c1.c2.c3.c4
            block = ((TagNode) tag1).getBlock();
            tag1 = block.pollNode();
            Assert.AreEqual(((TagNode) tag1).getValue(), ("span"));
            Assert.AreEqual(((TagNode) tag1).getAttribute("id"), ("id2"));
            Assert.AreEqual(((TagNode) tag1).getAttribute("class"), ("c1 c2 c3 c4"));
            Assert.AreEqual(block.hasNodes(), (false));
        }
    }
}