using System;
using System.Collections.Generic;
using System.IO;
using Jade.Filters;
using Jade.Template;
using Jade.Tests.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Compiler
{
    [TestClass]
    public class OriginalJadeTest : ParserTest
    {

        private String[] manualCompared = new String[]
        {
            "attrs", "attrs.js", "code.conditionals", "code.iteration", "comments",
            "escape-chars", "filters.coffeescript", "filters.less", "filters.markdown", "filters.stylus", "html",
            "include-only-text-body",
            "include-only-text", "include-with-text-head", "include-with-text", "mixin.blocks", "mixin.merge", "quotes",
            "script.whitespace", "scripts", "scripts.non-js",
            "source", "styles", "template", "text-block", "text", "vars", "yield-title", "doctype.default"
        };

        [TestMethod]
        public void test()
        {
            var folder = new DirectoryInfo(TestFileHelper.getOriginalResourcePath(""));
            var files = folder.EnumerateFiles("*.jade", SearchOption.TopDirectoryOnly);

            JadeConfiguration jade = new JadeConfiguration();
            jade.setMode(Jade4Net.Mode.XHTML); // original jade uses xhtml by default
            jade.setFilter("plain", new PlainFilter());
            jade.setFilter("cdata", new CDATAFilter());

            foreach (var file in files)
            {
                JadeTemplate template = jade.getTemplate(file.FullName);
                TextWriter writer = new StringWriter();
                jade.renderTemplate(template, new Dictionary<String, Object>(), writer);
                String html = writer.ToString();

                String expected = File.ReadAllText(file.FullName.Replace(".jade", ".html"));
                // Trace.WriteLine("\n>> " + file.getName());
                // Trace.WriteLine(html);
                // Trace.WriteLine("-- " + file.getName());
                // Trace.WriteLine(expected);
                // Trace.WriteLine("<< " + file.getName());

                if (Array.IndexOf(manualCompared, file.Name.Replace(".jade", ""))>=0)
                {
                    Assert.AreEqual(file.FullName, expected, html);
                }
            }
        }

    }

}