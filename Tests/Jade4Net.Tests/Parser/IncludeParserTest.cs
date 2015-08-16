using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class IncludeParserTest : ParserTest
    {
        private Node textNode;
        private TagNode tagNode;
        private BlockNode yieldNode;
        private BlockNode includeNode;

        [TestMethod]
        public void test()
        {
            loadInParser("include_1.jade");

            tagNode = (TagNode) root.pollNode();
            Assert.AreEqual(tagNode.getName(), ("p"));
            textNode = (TextNode) tagNode.getTextNode();
            Assert.AreEqual(textNode.getValue(), ("Before Include"));

            includeNode = (BlockNode) root.pollNode();
            tagNode = (TagNode) includeNode.pollNode();
            Assert.AreEqual(tagNode.getName(), ("span"));
            textNode = (TextNode) tagNode.getTextNode();
            Assert.AreEqual(textNode.getValue(), ("Hello Include"));

            yieldNode = (BlockNode) includeNode.pollNode();
            Assert.IsNotNull(yieldNode);
            
        /*var blockNode = (BlockNode) yieldNode.pollNode();
        tagNode = (TagNode)blockNode.pollNode();
        Assert.AreEqual(blockNode.getName(), ("p"));
        textNode = (TextNode) tagNode.getTextNode();
        Assert.AreEqual(textNode.getValue(), ("After Include"));

        Assert.AreEqual(yieldNode.hasNodes(), (false));
        Assert.AreEqual(includeNode.hasNodes(), (false));
        Assert.AreEqual(root.hasNodes(), (false));
        */
        }
    }
}