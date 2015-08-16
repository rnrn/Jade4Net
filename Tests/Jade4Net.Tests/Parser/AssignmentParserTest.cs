using System.Collections.Generic;
using System.Linq;
using Jade.Parser.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    [TestClass]
    public class AssignmentParserTest : ParserTest
    {

        private BlockNode block;

        [TestMethod]
        public void shouldReturnTagsWithTexts()
        {
            loadInParser("assignment.jade");
            block = (BlockNode) root;
            LinkedList<Node> nodes = block.getNodes();
            Assert.AreEqual(2, nodes.Count);

            AssigmentNode assignment = (AssigmentNode)block.getNodes().ElementAt(0);
            Assert.AreEqual("hello", assignment.getName());
            Assert.AreEqual("\"world\"", assignment.getValue());

            TagNode tag = (TagNode) block.getNodes().ElementAt(1);
            Assert.IsNotNull(tag);
        }
    }
}