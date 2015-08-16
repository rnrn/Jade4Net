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
using Jade.Tests.Helper.Beans;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Jade.Tests.Compiler
{
    [TestClass]
    public class CompilerTest
    {

        private String expectedFileNameExtension = ".html";

        [TestMethod]
        public void oneTag()
        {
            run("one_tag");
        }

        [TestMethod]
        public void nestedTags()
        {
            // TODO Reihenfolge der Attribute + CSS-Class + CSS-ID
            run("nested_tags");
        }

        public void lageBodyTextWithoutPipes()
        {
            run("large_body_text_without_pipes");
        }

        [TestMethod]
        public void complexIndentOutdentFile()
        {
            run("complex_indent_outdent_file");
        }

        [TestMethod]
        public void blockQuote()
        {
            run("blockquote");
        }

        [TestMethod]
        public void blockquotes()
        {
            run("blockquotes");
        }

        [TestMethod]
        public void cssClassAndId()
        {
            run("css_class_and_id");
        }

        [TestMethod]
        public void blockExpansionShorthand()
        {
            run("block_expansion_shorthands");
        }

        [TestMethod]
        public void tagWithAttributes()
        {
            run("tags_with_attributes");
        }

        [TestMethod]
        public void tagWithText()
        {
            run("tags_with_text");
        }

        [TestMethod]
        public void blockExpansion()
        {
            run("block_expansion");
        }

        [TestMethod]
        public void whileTag()
        {
            run("while");
        }

        [TestMethod]
        public void caseTag()
        {
            run("case");
        }

        [TestMethod]
        public void scriptTag()
        {
            run("script_tag");
        }

        [TestMethod]
        public void scriptTemplate()
        {
            run("script-template");
        }

        [TestMethod]
        public void variable()
        {
            run("variable");
        }

        [TestMethod]
        public void condition()
        {
            run("condition");
        }

        [TestMethod]
        public void conditionTypes()
        {
            run("condition_types");
        }

        [TestMethod]
        public void escape()
        {
            run("escape");
        }

        [TestMethod]
        public void whitespace()
        {
            run("whitespace");
        }

        [TestMethod]
        public void locals()
        {
            run("locals");
        }

        [TestMethod]
        public void complexCondition()
        {
            run("complex_condition");
        }

        [TestMethod]
        public void doctype()
        {
            run("doctype");
        }

        [TestMethod]
        public void terseDoctype()
        {
            run("terse_doctype");
        }

        [TestMethod]
        public void notTerseDoctype()
        {
            run("not_terse_doctype");
        }

        [TestMethod]
        public void beanPropertyCondition()
        {
            Dictionary<String, Object> map = new Dictionary<String, Object>();
            map.Add("bean", getTestBean("beanie"));
            List<TestBean> moreBeans = new List<TestBean>();
            for (int i = 0; i < 5; i++)
            {
                moreBeans.Add(getTestBean("x"));
            }
            map.Add("beans", moreBeans);
            JadeModel model = new JadeModel(map);
            run("bean_property_condition", false, model);
        }

        private TestBean getTestBean(String name)
        {
            TestBean b = new TestBean();
            Level2TestBean b2 = new Level2TestBean();
            b2.setName(name);
            b.setLevel2(b2);
            return b;
        }

        [TestMethod]
        public void fuzzyBooleanCondition()
        {
            run("fuzzy_bool_condition");
        }

        [TestMethod]
        public void assignment()
        {
            run("assignment");
        }

        [TestMethod]
        public void comment()
        {
            run("comment");
        }

        [TestMethod]
        public void conditionalComment()
        {
            run("conditional_comment");
        }

        [TestMethod]
        public void each()
        {
            run("each");
        }

        [TestMethod]
        public void eachElse()
        {
            run("each_else");
        }

        [TestMethod]
        public void attribute()
        {
            run("attribute");
        }

        [TestMethod]
        public void prettyPrint()
        {
            run("prettyprint", true);
        }

        [TestMethod]
        public void scope()
        {
            run("scope");
        }

        [TestMethod]
        public void mixin()
        {
            run("mixin");
        }

        [TestMethod]
        public void mixinParams()
        {
            run("mixin_params");
        }

        [TestMethod]
        public void mixinBlocks()
        {
            run("mixin_blocks");
        }

        [TestMethod]
        public void mixinMultipleBlocks()
        {
            run("mixin_multiple_blocks");
        }

        [TestMethod]
        public void mixinMultipleBlocksIf()
        {
            run("mixin_multiple_blocks_if");
        }

        [TestMethod]
        public void mixinMultipleBlocksCase()
        {
            run("mixin_multiple_blocks_case");
        }

        [TestMethod]
        public void mixinDefaultBlock()
        {
            run("mixin_default_block");
        }

        [TestMethod]
        public void mixinDefaultBlockNested()
        {
            run("mixin_default_block_nested");
        }

        [TestMethod]
        public void selfClosingTag()
        {
            run("self_closing_tag");
        }

        [TestMethod]
        public void mixinNested()
        {
            run("mixin_nested");
        }

        [TestMethod]
        public void mixinAttrs()
        {
            run("mixin_attrs", true);
        }

        [TestMethod]
        public void mixinMerge()
        {
            run("mixin_merge", true);
        }

        [TestMethod]
        public void mixin_with_conditional()
        {
            run("mixin_with_conditional");
        }

        [TestMethod]
        public void include1()
        {
            run("include_1");
        }

        [TestMethod]
        public void include2()
        {
            run("include_2");
        }

        [TestMethod]
        public void indentTabs()
        {
            run("indent_tabs");
        }

        [TestMethod]
        public void extendsLayout()
        {
            run("extends");
        }

        [TestMethod]
        public void extendsLayoutWithInclude()
        {
            run("extends_layout_include");
        }

        [TestMethod]
        public void mixinWithCommaSinglearg()
        {
            run("mixin_with_comma_singlearg");
        }

        [TestMethod]
        public void mixinWithCommaMorearg()
        {
            run("mixin_with_comma_morearg");
        }

        [TestMethod]
        public void mixinWithComplexParameter()
        {
            run("mixin_with_complex_parameter");
        }

        [TestMethod]
        public void largeBodyTextWithPipes()
        {
            // TODO add missing newline
            run("large_body_text_with_pipes");
        }

        [TestMethod]
        public void filterPlain()
        {
            run("filter_plain");
        }

        [TestMethod]
        [ExpectedException(typeof (JadeCompilerException))]
        public void expressionException() //throws IOException
        {

            tryToRender("expression_exception");
        }

        [TestMethod]
        [ExpectedException(typeof (JadeCompilerException))]
        public void expressionWrongMethodCall() //throws IOException
        {
            tryToRender("expression_method_invocation_exception");
        }

        [TestMethod]
        public void expressionLenientVariableEvaluation() //throws IOException
        {
            run("expression_lenient");
        }

        private void tryToRender(String file) //throws IOException
        {
            Jade4Net.render(TestFileHelper.getCompilerResourcePath(file + ".jade"),
                new Dictionary<String, Object>());
        }


        [TestMethod]
        public void filterMarkdown()
        {
            // TODO add missing newline
            run("filter_markdown");
        }

        [TestMethod]
        public void interpolation()
        {
            run("interpolation");
        }

        [TestMethod]
        public void includeNonJade()
        {
            run("include_non_jade");
        }

        [TestMethod]
        public void mixinVariableAttribute()
        {
            run("mixin_variable_attribute");
        }

        [TestMethod]
        public void inlineTextAndContent()
        {
            run("inline_text_and_content");
        }

        [TestMethod]
        [ExpectedException(typeof (JadeLexerException))]
        public void shouldThrowGoodExceptions()
        {
            run("invalid");
        }

        [TestMethod]
        [ExpectedException(typeof (JadeLexerException))]
        public void shouldThrowGoodExceptions2()
        {
            run("invalid2");
        }

        [TestMethod]
        public void xml()
        {
            String tmp = expectedFileNameExtension;
            expectedFileNameExtension = ".xml";
            run("xml_doctype");
            expectedFileNameExtension = tmp;
        }

        [TestMethod]
        public void reportedIssue90()
        {
            run("reportedIssue89");
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
                FileTemplateLoader loader = new FileTemplateLoader(
                    TestFileHelper.getCompilerResourcePath(""), "UTF-8");
                parser = new JadeParser(testName, loader);
            }
            catch (IOException e)
            {
                Trace.WriteLine(e);
            }
            Node root = parser.parse();
            Jade.Compiler.Compiler compiler = new Jade.Compiler.Compiler(root);
            compiler.setPrettyPrint(pretty);
            String expected = readFile(testName + expectedFileNameExtension);
            model.addFilter("markdown", new MarkdownFilter());
            model.addFilter("plain", new PlainFilter());
            model.addFilter("js", new JsFilter());
            model.addFilter("css", new CssFilter());
            model.addFilter("svg", new PlainFilter());
            String html = null;
            try
            {
                html = compiler.compileToString(model);
            }
            catch (JadeCompilerException e)
            {
                Trace.WriteLine(e);
                Assert.Fail();
            }
            Assert.AreEqual(testName, expected.Trim(), html.Trim());
        }

        private void debugOutput(String testName)
        {
            Trace.WriteLine(testName + " >>>> ");
            Trace.WriteLine("[jade]");
            Trace.WriteLine(readFile(testName + ".jade").Trim() + "\n");
            Trace.WriteLine("[model]");
            Trace.WriteLine(readFile(testName + ".json") + "\n");
            Trace.WriteLine("[html]");
            Trace.WriteLine(readFile(testName + ".html").Trim() + "\n");
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
                return File.ReadAllText(TestFileHelper.getCompilerResourcePath(fileName));
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            return "";
        }

    }
}