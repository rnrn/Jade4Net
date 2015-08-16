using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class ComplexIndentOutdentParserTest : ParserTest
    {
        private Node head;
        private Node body;
        private Node div1;
        private Node div2;
        private Node div3;
        private Node div4;
        private Node div5;
        private Node div6;
        private Node div7;
        private Node div8;
        private Node ul1;
        private Node ul2;
        private Node span;
        private Node em;
        private Node block;

        [TestMethod]
        public void shouldReturnABlockWithTokens2()
        {
            loadInParser("complex_indent_outdent_file.jade");
            block = (BlockNode) root;
            Assert.IsNotNull(block.getNodes());

            head = block.pollNode();
            body = block.pollNode();
            Assert.AreEqual(head.getValue(), ("head"));
            Assert.AreEqual(body.getValue(), ("body"));
            Assert.AreEqual(block.hasNodes(), (false));

            block = ((TagNode) head).getBlock();
            Assert.AreEqual(block.pollNode().getValue(), ("meta"));
            Assert.AreEqual(block.pollNode().getValue(), ("meta"));
            Assert.AreEqual(block.hasNodes(), (false));

            block = ((TagNode) body).getBlock();
            Assert.AreEqual(block.pollNode().getValue(), ("div0"));

            div1 = block.pollNode();
            Assert.AreEqual(div1.getValue(), ("div1"));

            div2 = block.pollNode();
            Assert.AreEqual(div2.getValue(), ("div2"));

            div3 = block.pollNode();
            Assert.AreEqual(div3.getValue(), ("div3"));

            div4 = block.pollNode();
            Assert.AreEqual(div4.getValue(), ("div4"));

            div5 = block.pollNode();
            Assert.AreEqual(div5.getValue(), ("div5"));

            Assert.AreEqual(block.hasNodes(), (false));

            // ===============================================

            block = ((TagNode) div1).getBlock();
            Assert.AreEqual(block.pollNode().getValue(), ("span"));
            Assert.AreEqual(block.pollNode().getValue(), ("span"));
            Assert.AreEqual(block.hasNodes(), (false));

            // ===============================================

            block = ((TagNode) div2).getBlock();
            ul1 = block.pollNode();
            Assert.AreEqual(ul1.getValue(), ("ul1"));
            ul2 = block.pollNode();
            Assert.AreEqual(ul2.getValue(), ("ul2"));
            Assert.AreEqual(block.hasNodes(), (false));

            // ===============================================

            block = ((TagNode) div3).getBlock();
            Assert.AreEqual(block.pollNode().getValue(), ("span"));
            Assert.AreEqual(block.hasNodes(), (false));

            // ===============================================

            block = ((TagNode) div4).getBlock();
            Assert.AreEqual(block.pollNode().getValue(), ("h1"));
            Assert.AreEqual(block.hasNodes(), (false));

            // ===============================================

            block = ((TagNode) div5).getBlock();
            div6 = block.pollNode();
            Assert.AreEqual(div6.getValue(), ("div6"));
            div7 = block.pollNode();
            Assert.AreEqual(div7.getValue(), ("div7"));
            Assert.AreEqual(block.hasNodes(), (false));

            // ===============================================

            block = ((TagNode) div6).getBlock();
            div8 = block.pollNode();
            Assert.AreEqual(div8.getValue(), ("div8"));
            Assert.AreEqual(block.hasNodes(), (false));

            // ===============================================

            block = ((TagNode) div8).getBlock();
            span = block.pollNode();
            Assert.AreEqual(span.getValue(), ("span"));
            Assert.AreEqual(block.hasNodes(), (false));

            // ===============================================

            block = ((TagNode) span).getBlock();
            em = block.pollNode();
            Assert.AreEqual(em.getValue(), ("em"));
            Assert.AreEqual(block.hasNodes(), (false));

            // ===============================================

            block = ((TagNode) div7).getBlock();
            Assert.AreEqual(block.pollNode().getValue(), ("span"));
            Assert.AreEqual(block.hasNodes(), (false));
        }

    }

}