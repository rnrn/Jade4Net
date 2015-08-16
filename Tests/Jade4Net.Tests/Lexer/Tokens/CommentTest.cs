using Jade.Lexer.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    [TestClass]
    public class CommentTest : TokenTest
    {
        [TestMethod]
        public void shouldScanBufferedComments() //throws Exception
        {
            lexer = initLexer("buffered_comment.jade");
            assertToken(1, typeof(Comment), "this is my first comment");
            assertToken(2, typeof(Newline), "newline");
            assertToken(2, typeof(Tag),     "div");
            assertToken(3, typeof(Indent),  "indent");
            assertToken(3, typeof(Comment), "this is a comment");
            assertToken(4, typeof(Newline), "newline");
            assertToken(4, typeof(Tag),     "div");
            assertToken(5, typeof(Indent),  "indent");
            assertToken(5, typeof(Comment), "so is this");
            assertToken(6, typeof(Indent),  "indent");
            assertToken(6, typeof(Tag),     "p");
            assertToken(7, typeof(Indent),  "indent");
            assertToken(7, typeof(Text),    "my text");
            assertToken(8, typeof(Outdent), "outdent");
            assertToken(8, typeof(Comment), "another comment comes here");
	    }

        [TestMethod]
        public void shouldScanUnbufferedComments() //throws Exception
        {
            lexer = initLexer("unbuffered_comment.jade");
            assertToken(1, typeof(Comment),           "this is my first comment");
            assertToken(2, typeof(Newline),           "newline");
            assertToken(2, typeof(Tag),               "div");
            assertToken(3, typeof(Indent),            "indent");
            assertToken(3, typeof(Comment),           "this is an unbuffered comment");
            assertToken(4, typeof(Newline),           "newline");
            assertToken(4, typeof(Tag),               "div");
            assertToken(5, typeof(Indent),            "indent");
            assertToken(5, typeof(Comment),           "so is this");
            assertToken(6, typeof(Indent),            "indent");
            assertToken(6, typeof(Tag),               "p");
            assertToken(7, typeof(Indent),            "indent");
            assertToken(7, typeof(Text),              "my text");
            assertToken(8, typeof(Outdent),           "outdent");
            assertToken(8, typeof(Comment),           "another comment comes here");
        }
    
        [TestMethod]
        public void shouldScanBlockComment() //throws Exception
        {
            lexer = initLexer("buffered_block_comment.jade");
            assertToken(1, typeof(Comment),         "this is my first comment");
            assertToken(2, typeof(Newline),         "newline");
            assertToken(2, typeof(Tag),             "div");
            assertToken(3, typeof(Indent),          "indent");
            assertToken(3, typeof(Comment),         "");
            assertToken(4, typeof(Indent),          "indent");
            assertToken(4, typeof(Tag),             "div");
            assertToken(5, typeof(Indent),          "indent");
            assertToken(5, typeof(Text),            "so is this");
            assertToken(6, typeof(Indent),          "indent");
            assertToken(6, typeof(Tag),             "p");
            assertToken(7, typeof(Indent),          "indent");
            assertToken(7, typeof(Text),            "my text");
            assertToken(8, typeof(Outdent),         "outdent");
            assertToken(8, typeof(Comment),         "another comment comes here");
        }    
    }

}
