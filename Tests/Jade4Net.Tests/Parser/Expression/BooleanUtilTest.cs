using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jade.Expression;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Parser.Expression
{
    [TestClass]
    public class BooleanUtilTest
    {

        [TestMethod]
        public void convert() {
        List<Object> falses = new List<Object>();
        List<Object> trues = new List<Object>();

        falses.Add(0);
        falses.Add(0.0d);
        falses.Add("");
        falses.Add(false);
        falses.Add(new List<String>());
        falses.Add(new int[] {});

        trues.Add(1);
        trues.Add(0.5d);
        trues.Add("a");
        trues.Add(" ");
        trues.Add(true);
        trues.Add(new []{"a"});
        trues.Add(new [] { 1, 2 });
        trues.Add(new Object());

        foreach (Object obj in falses) {
            Assert.IsFalse(BooleanUtil.convert(obj), obj + " (" + obj.GetType().FullName + ") should be false");
        }
        foreach (Object obj in trues) {
            Assert.IsTrue(BooleanUtil.convert(obj), obj + " (" + obj.GetType().FullName + ") should be true");
        }
    }
    }

}
