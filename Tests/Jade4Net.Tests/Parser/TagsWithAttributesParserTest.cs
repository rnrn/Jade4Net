using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class TagsWithAttributesParserTest : ParserTest
    {

        private Node tag1;
        private Node tag2;
        private Node tag3;
        private Node block;

        [TestMethod]
        public void shouldReturnABlockWithTokens2()
        {
            loadInParser("tags_with_attributes.jade");
            block = (BlockNode) root;
            Assert.IsNotNull(block.getNodes());

            // .myclass(title="my first div" alt="alt does not fit here")
            tag1 = block.pollNode();
            Assert.AreEqual(((TagNode) tag1).getAttribute("class"), ("myclass"));
            Assert.AreEqual(((TagNode) tag1).getAttribute("title"), ("my first div"));
            Assert.AreEqual(((TagNode) tag1).getAttribute("alt"), ("alt does not fit here"));
            Assert.AreEqual(block.hasNodes(), (true));

            // #myid.c1.c2.c3(title="the third div with attribute")
            tag2 = block.pollNode();
            Assert.AreEqual(((TagNode) tag2).getAttribute("id"), ("myid"));
            Assert.AreEqual(((TagNode) tag2).getAttribute("class"), ("c1 c2 c3"));
            Assert.AreEqual(((TagNode) tag2).getAttribute("title"), ("the third div with attribute"));
            Assert.AreEqual(block.hasNodes(), (false));

            // .c1.c2.c3(title="the second div with attribute")
            block = ((TagNode) tag1).getBlock();
            tag3 = block.pollNode();
            Assert.AreEqual(((TagNode) tag3).getAttribute("class"), ("c1 c2 c3"));
            Assert.AreEqual(((TagNode) tag3).getAttribute("title"), ("the second div with attribute"));
            Assert.AreEqual(block.hasNodes(), (true));

            // #myid        
            tag3 = block.pollNode();
            Assert.AreEqual(((TagNode) tag3).getAttribute("id"), ("myid"));
            Assert.AreEqual(block.hasNodes(), (false));

            // div#id1
            block = ((TagNode) tag2).getBlock();
            tag1 = block.pollNode();
            Assert.AreEqual(((TagNode) tag1).getAttribute("id"), ("id1"));
            Assert.AreEqual(block.hasNodes(), (false));

            // span#id2.c1.c2.c3.c4(alt="alt")
            block = ((TagNode) tag1).getBlock();
            tag1 = block.pollNode();
            Assert.AreEqual(((TagNode) tag1).getValue(), ("span"));
            Assert.AreEqual(((TagNode) tag1).getAttribute("id"), ("id2"));
            Assert.AreEqual(((TagNode) tag1).getAttribute("class"), ("c1 c2 c3 c4"));
            Assert.AreEqual(((TagNode) tag1).getAttribute("alt"), ("alt"));
            Assert.AreEqual(block.hasNodes(), (false));
        }
    }

}