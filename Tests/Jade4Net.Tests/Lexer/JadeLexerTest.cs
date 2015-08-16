using Jade.Lexer;
using Jade.Lexer.Tokens;
using Jade.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer
{
    [TestClass]
    public class JadeLexerTest
    {

        private JadeLexer lexer1;
        private JadeLexer lexer2;

        public JadeLexerTest() //throws Exception
        {
            FileTemplateLoader loader1 = new FileTemplateLoader(TestFileHelper.getLexerResourcePath(""), "UTF-8");
            lexer1 = new JadeLexer("test_file1.jade", loader1);

            FileTemplateLoader loader2 = new FileTemplateLoader(TestFileHelper.getLexerResourcePath(""), "UTF-8");
            lexer2 = new JadeLexer("empty_file.jade", loader2);

        }

        [TestMethod]
        public void shouldReturnALookaheadToken() //throws Exception
        {
            Token token = lexer1.next();
            assertThat(token.ToString(), ("div"));
            Assert.AreEqual(token.GetType(), typeof (Tag));

            token = lexer1.next();
            assertThat(token.ToString(), ("indent"));
            Assert.AreEqual(token.GetType(), typeof (Indent));

            token = lexer1.next();
            assertThat(token.ToString(), ("h1"));
            Assert.AreEqual(token.GetType(), typeof (Tag));

            token = lexer1.next();
            assertThat(token.ToString(), ("outdent"));
            Assert.AreEqual(token.GetType(), typeof (Outdent));

            token = lexer1.next();
            Assert.AreEqual(token.GetType(), typeof (Eos));
        }

        private void assertThat(string toString, string div)
        {
            Assert.AreEqual(toString, div);
        }

        [TestMethod]
        public void shouldReturnATagToken() //throws Exception
        {
            Token token = lexer1.next();
            Assert.IsNotNull(token);
            assertThat(token.getValue(), ("div"));
        }

        [TestMethod]
        public void shouldReturnAnEOFTagIfEmptyFile() //throws Exception 
        {
            assertThat(lexer2.next().getValue(), ("eos"));
        }

    }
}