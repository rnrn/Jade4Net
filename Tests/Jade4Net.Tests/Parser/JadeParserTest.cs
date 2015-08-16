using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class JadeParserTest : ParserTest
    {

        private Node blockNode;

        [TestMethod]
        public void shouldReturnABlockWithTokens2()
        {
            loadInParser("two_blocks_with_an_outdent.jade");
            blockNode = (BlockNode) root;
            Assert.IsNotNull(blockNode.getNodes());

            Node node = blockNode.pollNode();
            Assert.AreEqual(node.getValue(), ("5"));

            node = blockNode.pollNode();
            Assert.AreEqual(node.getValue(), ("div"));
            Assert.AreEqual(blockNode.hasNodes(), (false));

            blockNode = ((TagNode) node).getBlock();
            node = blockNode.pollNode();
            Assert.AreEqual(node.getValue(), ("p"));
            Node node2 = blockNode.pollNode();
            Assert.AreEqual(node2.getValue(), ("h1"));
            Assert.AreEqual(blockNode.hasNodes(), (false));

            blockNode = ((TagNode) node).getBlock();
            node = blockNode.pollNode();
            Assert.AreEqual(node.getValue(), ("span"));
            Assert.AreEqual(blockNode.hasNodes(), (false));
        }

        [TestMethod]
        public void shouldReturnABlockWithTokens()
        {
            loadInParser("two_blocks_with_a_tag.jade");
            Assert.IsNotNull(((BlockNode) root).getNodes());
            Node node = ((BlockNode) root).pollNode();
            Assert.AreEqual(node.getValue(), ("div"));
            Node blockNode = ((TagNode) node).getBlock();
            node = blockNode.pollNode();
            Assert.AreEqual(node.getValue(), ("h1"));
            Assert.AreEqual(blockNode.hasNodes(), (false));
        }

    }

}