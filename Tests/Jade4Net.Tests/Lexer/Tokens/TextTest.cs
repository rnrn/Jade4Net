using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class TextTest : TokenTest 
    {


        [TestMethod]
        public void shouldReturnIndentTag() //throws Exception 
        {
            lexer = initLexer("text.jade");
            assertToken(typeof(Tag),     "p");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Text),    "Hello World");
            assertToken(typeof(Newline), "newline");
            assertToken(typeof(Text),    "Hello Berlin");
            assertToken(typeof(Newline), "newline");
            assertToken(typeof(Text),    "Hello Tokyo");
            assertToken(typeof(Outdent), "outdent");

            assertToken(typeof(Tag),     "div");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Tag),     "p");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Text),    "Hello World");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Text),    "Hello Berlin");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Text),    "Hello Tokyo");
            assertToken(typeof(Outdent), "outdent");


            assertToken(typeof(Tag),     "div");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Tag),     "div");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Tag),     "p");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Text),    "Hello World");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Tag),     "p");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Text),    "Hello Berlin");
            assertToken(typeof(Indent),  "indent");
            assertToken(typeof(Text),    "Hello Tokyo");
            assertToken(typeof(Newline), "newline");
            assertToken(typeof(Outdent), "outdent");
            assertToken(typeof(Eos),     "eos");
        }
    }
}
