using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class TextParserTest : ParserTest
    {

        private Node block;
        private TagNode tag1;
        private Node tag2;
        private Node tag;

        [TestMethod]
        public void shouldReturnTagsWithTexts()
        {
            loadInParser("tags_with_text.jade");
            block = (BlockNode) root;
            Assert.IsNotNull(block.getNodes());
            Assert.AreEqual(block.getNodes().Count, (2));

            tag1 = (TagNode) block.pollNode();
            Assert.AreEqual(((TagNode) tag1).getAttribute("class"), ("myclass"));
            Assert.IsNotNull(((TagNode) tag1).getTextNode());
            Assert.AreEqual(((TagNode) tag1).getTextNode().getValue(), ("Hello World!"));
            Assert.AreEqual(block.hasNodes(), (true));

            tag2 = block.pollNode();
            Assert.AreEqual(((TagNode) tag2).getAttribute("id"), ("myid2"));
            Assert.AreEqual(((TagNode) tag2).getTextNode().getValue(), ("without words"));
            Assert.AreEqual(block.hasNodes(), (false));

            block = ((TagNode) tag1).getBlock();
            tag = block.pollNode();
            Assert.AreEqual(((TagNode) tag).getAttribute("class"), ("c1"));
            Assert.AreEqual(((TagNode) tag).getTextNode().getValue(), ("The quick brown fox"));
            Assert.AreEqual(block.hasNodes(), (true));

            tag = block.pollNode();
            Assert.AreEqual(((TagNode) tag).getAttribute("class"), ("c2"));
            Assert.AreEqual(((TagNode) tag).getAttribute("id"), ("myid"));
            Assert.AreEqual(((TagNode) tag).getTextNode().getValue(), ("jumpes over the lazy dog"));
            Assert.AreEqual(block.hasNodes(), (false));

            block = ((TagNode) tag2).getBlock();
            tag = block.pollNode();
            Assert.AreEqual(((TagNode) tag).getAttribute("id"), ("id1"));
            Assert.AreEqual(((TagNode) tag).getTextNode().getValue(), ("without music"));
            Assert.AreEqual(block.hasNodes(), (false));
        }

    }

}