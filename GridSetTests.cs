using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GridSet;

namespace Tests
{
    [TestClass()]
    public class GridSetTests
    {
        class X { }

        [TestInitialize()]
        public void Startup()
        {
//            test = new GridSet<X>
        }

        [TestMethod()]
        public void GridSetTest()
        {
            Trace.WriteLine("this is going to work for sure");
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void WithinBoundsTest()
        {
            var w = 5;
            var h = 5;
            var g = new GridSet<X>(w, h);
            Assert.IsFalse(g.WithinBounds(6, 5));
            Assert.IsFalse(g.WithinBounds(5, 6));
            Assert.IsFalse(g.WithinBounds(-1, 0));
            Assert.IsTrue(g.WithinBounds(1, 0));
            Assert.IsTrue(g.WithinBounds(1, 3));
            Assert.IsTrue(g.WithinBounds(4, 4));
            Assert.IsTrue(g.WithinBounds(0, 0));
        }

        [TestMethod()]
        public void FillTest()
        {
            var g = new GridSet<X>(5, 5);
            var x = new X();
            g.Fill(x);
            Assert.IsTrue(g[0, 0] == x);
            Assert.IsTrue(g[4, 4] == x);
            Assert.IsTrue(g[2, 1] == x);
            //Assert.Fail();
        }

        [TestMethod()]
        public void LoopTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSectionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoopTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetReferenceTest()
        {
            Assert.Fail();
        }
    }
}