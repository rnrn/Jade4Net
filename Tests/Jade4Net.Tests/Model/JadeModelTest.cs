using System.Collections.Generic;
using Jade.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jade.Tests.Model
{
    [TestClass]
    public class JadeModelTest
    {
        private JadeModel model;

        public JadeModelTest()
        {
            var map = new Dictionary<string, object>();
            map.Add("hello", "world");
            map.Add("foo", "bar");
            model = new JadeModel(map);
        }

        [TestMethod]
        public void TestScope()
        {
            Assert.AreEqual("world", model.get("hello"));
            model.pushScope();
            model.put("hello", "new world");
            Assert.AreEqual("new world", model.get("hello"));
            model.popScope();
            Assert.AreEqual("world", model.get("hello"));
        }

        [TestMethod]
        public void Test()
        {
            var defaults = new Dictionary<string, object>();
            defaults.Add("hello", "world");

            model = new JadeModel(defaults);
            model.put("new", true);

            Assert.IsFalse(defaults.ContainsKey("new"));
            Assert.IsTrue(model.ContainsKey("new"));
            Assert.AreEqual(model.get("hello"), "world");
        }
    }
}
