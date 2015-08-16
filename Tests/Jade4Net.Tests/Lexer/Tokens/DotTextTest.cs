using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class DotTextTest : TokenTest
    {
        [TestMethod]
        public void shouldScanTagsWithAttributes() //throws Exception
        {
            lexer = initLexer("large_body_text_without_pipes.jade");
            assertToken(typeof(Tag),        "p");
            assertToken(typeof(Dot),        ".");
            assertToken(typeof(Indent),     "indent");
            assertToken(typeof(Tag),        "Hello");
            assertToken(typeof(Text),       "World!");
            assertToken(typeof(Newline),    "newline");
            assertToken(typeof(Tag),        "Here");
            assertToken(typeof(Text),       "comes the Message!");
            assertToken(typeof(Outdent),    "outdent");
            assertToken(typeof(Eos),        "eos");
        }
    }
}
