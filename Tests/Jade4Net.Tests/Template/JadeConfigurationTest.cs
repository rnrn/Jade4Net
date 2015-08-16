using System;
using System.Diagnostics;
using System.IO;
using Jade.Parser;
using Jade.Parser.Nodes;
using Jade.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Template
{
    [TestClass]
    public class JadeConfigurationTest
    {

        protected JadeParser parser;
        protected Node root;

        [TestMethod]
        public void testGetTemplate()
        {
            JadeConfiguration config = new JadeConfiguration();
            JadeTemplate template = config.getTemplate(getParserResourcePath("assignment"));
            Assert.IsNotNull(template);
        }

        [TestMethod]
        public void testCache()
        {
            JadeConfiguration config = new JadeConfiguration();
            config.setCaching(true);
            JadeTemplate template = config.getTemplate(getParserResourcePath("assignment"));
            Assert.IsNotNull(template);
            JadeTemplate template2 = config.getTemplate(getParserResourcePath("assignment"));
            Assert.IsNotNull(template2);
            Assert.AreEqual(template, template2);
        }

        [TestMethod]
        public void testExceptionOnUnknowwTemplate()
        {
            JadeConfiguration config = new JadeConfiguration();
            JadeTemplate template = null;
            try
            {
                template = config.getTemplate("UNKNOWN_PATH");
                Assert.Fail("Did expect TemplatException!");
            }
            catch (IOException ignore)
            {

            }
            Assert.IsNull(template);
        }

        [TestMethod]
        public void testPrettyPrint()
        {
            JadeConfiguration config = new JadeConfiguration();
            config.setPrettyPrint(true);
            JadeTemplate template = config.getTemplate(getParserResourcePath("assignment"));
            Assert.IsTrue(template.isPrettyPrint());
        }

        [TestMethod]
        public void testRootNode()
        {
            JadeConfiguration config = new JadeConfiguration();
            JadeTemplate template = config.getTemplate(getParserResourcePath("assignment"));
            Assert.IsNotNull(template.getRootNode());
        }

        public String getParserResourcePath(String fileName)
        {
            try
            {
                return TestFileHelper.getRootResourcePath() + "/parser/" + fileName;
            }
            catch (FileNotFoundException e)
            {
                Trace.WriteLine(e);
            }
            return null;
        }
    }
}
