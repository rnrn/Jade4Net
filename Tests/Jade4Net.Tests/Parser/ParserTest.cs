using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Jade.Parser;
using Jade.Parser.Nodes;
using Jade.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser
{
    public class ParserTest
    {

        protected JadeParser parser;
        protected Node root;

        protected void loadInParser(String fileName)
        {

            try
            {
                FileTemplateLoader loader = new FileTemplateLoader(
                        TestFileHelper.getParserResourcePath(""), "UTF-8");
                parser = new JadeParser(fileName, loader);
            }
            catch (IOException e)
            {
                Trace.WriteLine(e);
                Assert.Fail("template " + fileName + " was not found");
            }
            root = parser.parse();
        }
    }

}
