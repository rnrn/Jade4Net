using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Jade.Exceptions;
using Jade.Model;
using Jade.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Template
{
    [TestClass]
    public class JadeRunFullTemplateTest
    {

        private JadeConfiguration cfg = new JadeConfiguration();

        [TestMethod]
        public void testFullRun()
        {

            Dictionary<String, Object> root = new Dictionary<String, Object>();
            root.Add("hello", "world");
            root.Add("hallo", null);
            JadeModel model = new JadeModel(root);

            JadeTemplate temp = cfg.getTemplate(getResourcePath("fullrun"));

            StringWriter outStream = new StringWriter();
            try
            {
                temp.process(model, outStream);
            }
            catch (JadeCompilerException e)
            {
                Trace.WriteLine(e);
                Assert.Fail();
            }
            outStream.Flush();
            Assert.AreEqual("<div><div>Hi everybody</div></div>", outStream.ToString());

        }

        [TestMethod]
        public void testEachLoopWithIterableMap()
        {

            Dictionary<string, string> users = new Dictionary<string, string>();
            users.Add("bob", "Robert Smith");
            users.Add("alex", "Alex Supertramp");

            Dictionary<String, Object> root = new Dictionary<String, Object>();
            root.Add("users", users);
            JadeModel model = new JadeModel(root);

            JadeTemplate temp = cfg.getTemplate(getResourcePath("each_loop"));

            StringWriter outStream = new StringWriter();
            try
            {
                temp.process(model, outStream);
            }
            catch (JadeCompilerException e)
            {
                Trace.WriteLine(e);
                Assert.Fail();
            }
            outStream.Flush();
            Assert.AreEqual("<ul><li>Robert Smith</li><li>Alex Supertramp</li></ul>", outStream.ToString());

        }

        public String getResourcePath(String fileName)
        {
            try
            {
                return TestFileHelper.getRootResourcePath() + "/template/" + fileName;
            }
            catch (FileNotFoundException e)
            {
                Trace.WriteLine(e);
            }
            return null;
        }
    }


}