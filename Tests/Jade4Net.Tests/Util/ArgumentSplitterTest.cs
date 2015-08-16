using System;
using System.Collections.Generic;
using Jade.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Util
{
    [TestClass]
    public class ArgumentSplitterTest
    {

        private List<String> parameters;

        [TestMethod]
        public void testSplit() //throws Exception
        {

            List<String> args;
            args = ArgumentSplitter.split("foo.faa('this is arg1'),'this is arg2'");
            Assert.AreEqual(args.Count, 2);
            Assert.AreEqual("foo.faa('this is arg1')", args[0]);
            Assert.AreEqual("'this is arg2'", args[1]);

            args = ArgumentSplitter.split("foo.faa ( 'this is arg1'), 'this is arg2'");
            Assert.AreEqual(args.Count, 2);
            Assert.AreEqual("foo.faa ( 'this is arg1')", args[0]);
            Assert.AreEqual("'this is arg2'", args[1]);

            args = ArgumentSplitter.split("foo.faa(\"this is arg1\"),\"this is arg2\"");
            Assert.AreEqual(args.Count, 2);
            Assert.AreEqual("foo.faa(\"this is arg1\")", args[0]);
            Assert.AreEqual("\"this is arg2\"", args[1]);

            args = ArgumentSplitter.split("foo.faa ( \"this is arg1\" ) , \"this is arg2\" ");
            Assert.AreEqual(args.Count, 2);
            Assert.AreEqual("foo.faa ( \"this is arg1\" )", args[0]);
            Assert.AreEqual("\"this is arg2\"", args[1]);

            args = ArgumentSplitter.split("1");
            Assert.AreEqual(args.Count, 1);
            Assert.AreEqual("1", args[0]);

            args = ArgumentSplitter.split("1,2,3");
            Assert.AreEqual(args.Count, 3);
            Assert.AreEqual("1", args[0]);
            Assert.AreEqual("2", args[1]);
            Assert.AreEqual("3", args[2]);

            args = ArgumentSplitter.split("1 , 2, 3");
            Assert.AreEqual(args.Count, 3);
            Assert.AreEqual("1", args[0]);
            Assert.AreEqual("2", args[1]);
            Assert.AreEqual("3", args[2]);

            args = ArgumentSplitter.split("1 , '2', 3");
            Assert.AreEqual(args.Count, 3);
            Assert.AreEqual("1", args[0]);
            Assert.AreEqual("'2'", args[1]);
            Assert.AreEqual("3", args[2]);

            args = ArgumentSplitter.split("'1' , '2', '3'");
            Assert.AreEqual(args.Count, 3);
            Assert.AreEqual("'1'", args[0]);
            Assert.AreEqual("'2'", args[1]);
            Assert.AreEqual("'3'", args[2]);

            args = ArgumentSplitter.split("bar(bazz, 'ba(z,z)'), 123");
            Assert.AreEqual(args.Count, 2);
            Assert.AreEqual("bar(bazz, 'ba(z,z)')", args[0]);
            Assert.AreEqual("123", args[1]);

            args = ArgumentSplitter.split("'aaa', bar(bazz, foo('ba(z,z)')), 123");
            Assert.AreEqual(args.Count, 3);
            Assert.AreEqual("'aaa'", args[0]);
            Assert.AreEqual("bar(bazz, foo('ba(z,z)'))", args[1]);
            Assert.AreEqual("123", args[2]);

            args = ArgumentSplitter.split("123, '1,2,3', \"a,b,c\"");
            Assert.AreEqual(args.Count, 3);
            Assert.AreEqual("123", args[0]);
            Assert.AreEqual("'1,2,3'", args[1]);
            Assert.AreEqual("\"a,b,c\"", args[2]);
        }

        [TestMethod]
        public void shouldRecognizeNumberOfParametersWhenUsingDoubleQuotesAndSingleQuotes() //throws Exception
        {
            whenSplitting("\"bla\",'blub',\"boo, hoo\"");
            thenNumberOfParametersShouldBe(3);
        }

        [TestMethod]
        public void shouldRecognizeDoubleQuotedParameterWhenUsingDoubleQuotesAndSingleQuotes() //throws Exception
        {
            whenSplitting("\"bla\",'blub',\"boo, hoo\"");
            thenParametersAtIndexShouldBe(0, "\"bla\"");
        }

        [TestMethod]
        public void shouldRecognizeSingleQuotedParametersWhenUsingDoubleQuotesAndSingleQuotes() //throws Exception
        {
            whenSplitting("\"bla\",'blub',\"boo, hoo\"");
            thenParametersAtIndexShouldBe(1, "'blub'");
        }

        private void whenSplitting(String parameterStringToSplit)
        {
            parameters = ArgumentSplitter.split(parameterStringToSplit);
        }

        private void thenNumberOfParametersShouldBe(int expectedNumberOfParameters)
        {
            Assert.AreEqual(expectedNumberOfParameters, parameters.Count);
        }

        private void thenParametersAtIndexShouldBe(int index, String expectedParameter)
        {
            Assert.AreEqual(expectedParameter, parameters[index]);
        }
    }
}
