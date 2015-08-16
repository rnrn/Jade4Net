using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Jade.Exceptions;
using Jade.Util;
using Jade.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Exceptions
{
    [TestClass]
    public class JadeExceptionTest
    {

        [TestMethod]
        public void test()
        {
            String errorJade = TestFileHelper.getCompilerResourcePath("exceptions/error.jade");
            String exceptionHtml = TestFileHelper.getCompilerResourcePath("exceptions/error.html");
            try
            {
                Jade4Net.render(errorJade, new Dictionary<String, Object>());
                Assert.Fail();
            }
            catch (JadeException e)
            {
                Assert.IsTrue(e.Message.StartsWith("unable to evaluate [non.existing.query()]"));
                Assert.AreEqual(9, e.getLineNumber());
                Assert.AreEqual(errorJade, e.getFilename());
                String expectedHtml = readFile(exceptionHtml);
                String html = e.toHtmlString("<html><head><title>broken");
                Assert.AreEqual(removeAbsolutePath(expectedHtml), removeAbsolutePath(html));
            }
        }

        private String removeAbsolutePath(String html)
        {
            html = html.replaceAll("(<h2>In ).*(compiler/exceptions/error\\.jade at line 9\\.</h2>)", "$1\\.\\./$2");
            return html;
        }

        private String readFile(String fileName)
        {
            try
            {
                return (File.ReadAllText(fileName));
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            return "";
        }
    }
}