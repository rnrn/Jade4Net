using System;
using Jade.Parser.Nodes;
using Jade.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser.Nodes
{
    [TestClass]
    public class TagNodeTest
    {

        private TagNode tagNode;
        private JadeTemplate template;
        private String[] selfClosing = {"meta", "img", "link", "input", "area", "base", "col", "br", "hr", "source"};
        private String[] notSelfClosing = {"div", "table", "span"};

        public TagNodeTest()
        {
            tagNode = new TagNode();
            template = new JadeTemplate();
            template.setMode(Jade4Net.Mode.XHTML);
        }

        [TestMethod]
        public void testThatTagNodeIsTerse()
        {
            template.setMode(Jade4Net.Mode.HTML);
            for (int i = 0; i < selfClosing.Length; i++)
            {
                tagNode.setName(selfClosing[i]);
                Assert.IsTrue(tagNode.isTerse(template));
            }
        }

        [TestMethod]
        public void testThatTagNodeIsNotSelfclosingIfTheTagIsNotSelfclosing()
        {
            template.setMode(Jade4Net.Mode.HTML);
            for (int i = 0; i < notSelfClosing.Length; i++)
            {
                tagNode.setName(notSelfClosing[i]);
                Assert.IsFalse(tagNode.isTerse(template));
            }
        }

        [TestMethod]
        public void testThatTagNodeIsNotTerseIfTempalteSettingIsNotTerse()
        {
            template.setMode(Jade4Net.Mode.XHTML);
            for (int i = 0; i < selfClosing.Length; i++)
            {
                tagNode.setName(selfClosing[i]);
                Assert.IsFalse(tagNode.isTerse(template));
            }
        }

        [TestMethod]
        public void testThatTagNodeIsSelfClosing()
        {
            for (int i = 0; i < selfClosing.Length; i++)
            {
                tagNode.setName(selfClosing[i]);
                Assert.IsTrue(tagNode.isSelfClosing(template));
            }
        }

        [TestMethod]
        public void testThatTagNodeIsNotSelfClosingIfXmlDoctype()
        {
            template.setMode(Jade4Net.Mode.XML);
            for (int i = 0; i < selfClosing.Length; i++)
            {
                tagNode.setName(selfClosing[i]);
                Assert.IsFalse(tagNode.isSelfClosing(template));
            }
        }

        [TestMethod]
        public void testThatTagNodeIsNotSelfClosingIfNotSelfClosingtag()
        {
            for (int i = 0; i < notSelfClosing.Length; i++)
            {
                tagNode.setName(notSelfClosing[i]);
                Assert.IsFalse(tagNode.isSelfClosing(template));
            }
        }

    }

}