using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class LargeBodyTextWithoutPipesParserTest : ParserTest
    {

        private Node block;
        private Node textNode;
        private TagNode tagNode;


        [TestMethod]
        public void test()
        {
            loadInParser("large_body_text_without_pipes.jade");
            tagNode = (TagNode) root.pollNode();
            Assert.AreEqual(tagNode.getName(), ("p"));

            textNode = (TextNode) tagNode.getTextNode();
            Assert.IsNotNull(textNode.getValue());
            Assert.AreEqual(textNode.getValue(), ("Hello World!\nHere comes the Message!"));
            Assert.AreEqual(textNode.hasNodes(), (false));

            tagNode = (TagNode) root.pollNode();
            Assert.AreEqual(tagNode.getName(), ("div"));
            block = tagNode.getBlock();
            Assert.IsNotNull(block);

            tagNode = (TagNode) block.pollNode();
            Assert.AreEqual(tagNode.getName(), ("h1"));

            textNode = (TextNode) tagNode.getTextNode();
            Assert.IsNotNull(textNode.getValue());
            Assert.AreEqual(textNode.getValue(), ("Hello World!\nHere comes the second Message!"));
            Assert.AreEqual(textNode.hasNodes(), (false));

            tagNode = (TagNode) block.pollNode();
            Assert.AreEqual(tagNode.getName(), ("h2"));

            textNode = (TextNode) tagNode.getTextNode();
            Assert.IsNotNull(textNode.getValue());
            Assert.AreEqual(textNode.getValue(), ("Hello World!\nHere comes the third Message!"));
            Assert.AreEqual(textNode.hasNodes(), (false));

            Assert.AreEqual(block.hasNodes(), (false));
            Assert.AreEqual(root.hasNodes(), (false));
        }
    }
}