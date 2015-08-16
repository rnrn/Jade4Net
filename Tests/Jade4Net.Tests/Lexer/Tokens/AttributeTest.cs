using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Attribute = Jade.Lexer.Tokens.Attribute;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class AttributeTest : TokenTest
    {

        [TestMethod]
        public void shouldReturnAnAttributesToken() //throws Exception
        {
            lexer = initLexer("attribute_1.jade");
            assertToken(typeof (Tag),        "img");
            assertToken(typeof (Attribute), "src='http://example.com/spacer.gif', title='u cant c me'");
        }

        [TestMethod]
        public void shouldScanTagsWithAttributes() //throws Exception
        {
            lexer = initLexer("attribute_2.jade");
            assertToken(3, typeof (Newline),    "newline");
            assertToken(3, typeof (Tag),        "p");
            assertToken(3, typeof (CssId),      "red");
            assertToken(3, typeof (Attribute),  "title='my special title', alt='some alt text'");
            assertToken(6, typeof (Indent),     "indent");
            assertToken(6, typeof (Text),       "Hello World");
            assertToken(8, typeof (Newline),    "newline");
            assertToken(8, typeof (Text),       "Hello Berlin");
            assertToken(9, typeof (Newline),    "newline");
            assertToken(9, typeof (Text),       "Hello Tokyo");
            assertToken(12, typeof (Outdent),    "outdent");
            assertToken(12, typeof (Tag),        "div");
            assertToken(12, typeof (CssId),      "content");
            assertToken(12, typeof (Attribute),  "title='test title', alt = 'alt text'");
            assertToken(14, typeof (Indent),     "indent");
            assertToken(14, typeof (Tag),        "p");
            assertToken(14, typeof (Attribute),  "title='test title', alt = 'alt text'");
            assertToken(15, typeof (Indent),     "indent");
            assertToken(15, typeof (Text),       "Hello World");
            assertToken(16, typeof (Indent),     "indent");
            assertToken(16, typeof (Text),       "Hello Berlin");
            assertToken(17, typeof (Indent),     "indent");
            assertToken(17, typeof (Text),       "Hello Tokyo");
            assertToken(20, typeof (Outdent),    "outdent");
            assertToken(20, typeof (Tag),        "div");
            assertToken(20, typeof (CssId),      "footer");
            assertToken(20, typeof (Attribute),  "title='test title', alt = 'alt text'");
            assertToken(23, typeof (Indent),     "indent");
            assertToken(23, typeof (Tag),        "div");
            assertToken(23, typeof (CssId),      "left");
            assertToken(23, typeof (Attribute),  "title='test title', alt = 'alt text'");
            assertToken(24, typeof (Indent),     "indent");
            assertToken(24, typeof (Tag),        "p");
            assertToken(24, typeof (CssId),      "red");
            assertToken(24, typeof (Attribute),  "title='test title', alt = 'alt text'");
            assertToken(25, typeof (Indent),     "indent");
            assertToken(25, typeof (Text),       "Hello World");
            assertToken(27, typeof (Indent),     "indent");
            assertToken(27, typeof (Tag),        "p");
            assertToken(27, typeof (CssId),      "green");
            assertToken(27, typeof (Attribute),  "title='test title', alt = 'alt text'");
            assertToken(28, typeof (Indent),     "indent");
            assertToken(28, typeof (Text),       "Hello Berlin");
            assertToken(29, typeof (Indent),     "indent");
            assertToken(29, typeof (Text),       "Hello Tokyo");
            assertToken(30, typeof (Outdent),    "outdent");
            assertToken(30, typeof (Eos),        "eos");
        }
    }
}
