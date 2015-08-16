using System;
using System.Diagnostics;
using Jade.Lexer;
using Jade.Lexer.Tokens;
using Jade.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Lexer.Tokens
{
    public class TokenTest
    {

        protected JadeLexer lexer;

        protected JadeLexer initLexer(String fileName)
        {
            FileTemplateLoader loader;
            try
            {
                loader = new FileTemplateLoader(TestFileHelper.getLexerResourcePath(""), "UTF-8");
                return new JadeLexer(fileName, loader);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return null;
            }
        }

        protected virtual void assertToken(int expectedLineNumber, Type expectedClazz, String expectedValue)
        {
            Token token = lexer.next();
            Assert.AreEqual(token.getValue(), (expectedValue));
            Assert.AreEqual(token.getLineNumber(), (expectedLineNumber));
            Assert.AreEqual(token.GetType(), expectedClazz);
        }

        protected void assertToken(Type clazz, String value)
        {
            Token token = lexer.next();
            Assert.AreEqual(token.getValue(), value);
            Assert.AreEqual(token.GetType(), clazz, "Expected: " + clazz + " got " + token.GetType());
        }

        protected void assertToken(String value)
        {
            Token token = lexer.next();
            Assert.AreEqual(token.getValue(), (value));
        }
    }
}