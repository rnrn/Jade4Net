using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Jade.Exceptions;
using Jade.Filters;
using Jade.Model;
using Jade.Parser;
using Jade.Parser.Nodes;
using Jade.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Jade.Tests.Compiler
{
    //[TestClass]
    public class CompilerIndentationErrorTest
    {

        //[TestMethod]
        public void testTagsWithErrors()
        {
            run("indentation_errors");
        }

        private void run(String testName)
        {
            run(testName, false);
        }

        private void run(String testName, bool pretty)
        {
            JadeModel model = new JadeModel(getModelMap(testName));
            run(testName, pretty, model);
        }

        private void run(String testName, bool pretty, JadeModel model)
        {
            JadeParser parser = null;
            try
            {
                FileTemplateLoader loader = new FileTemplateLoader(TestFileHelper.getCompilerErrorsResourcePath(""),
                    "UTF-8");
                parser = new JadeParser(testName, loader);
            }
            catch (IOException e)
            {
                Trace.WriteLine(e);
            }
            Node root = parser.parse();
            Jade.Compiler.Compiler compiler = new Jade.Compiler.Compiler(root);
            compiler.setPrettyPrint(pretty);
            String expected = readFile(testName + ".html");
            model.addFilter("markdown", new MarkdownFilter());
            model.addFilter("plain", new PlainFilter());
            String html;
            try
            {
                html = compiler.compileToString(model);
                Assert.AreEqual(expected.Trim(), html.Trim(), testName);
                Assert.Fail();
            }
            catch (JadeCompilerException e)
            {
                Trace.WriteLine(e);
            }
        }

        private Dictionary<String, Object> getModelMap(String testName)
        {
            String json = readFile(testName + ".json");
            var model = JsonConvert.DeserializeObject<Dictionary<String, Object>>(json);
            if (model == null)
            {
                model = new Dictionary<String, Object>();
            }
            return model;
        }

        private String readFile(String fileName)
        {
            try
            {
                return File.ReadAllText(TestFileHelper.getCompilerErrorsResourcePath(fileName));
            }
            catch (Exception)
            {
                // Trace.WriteLine(e);
            }
            return "";
        }
    }
}