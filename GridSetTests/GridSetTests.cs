using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using GridSet;
using System.Linq;

namespace Tests
{
    [TestClass()]
    public class GridSetTests
    {
        class X {
            public bool test = false;
            public string secret;
        }

        GridSet<X> n = new GridSet<X>(0,0);

        
        [TestInitialize()]
        public void Startup()
        {
            n = new GridSet<X>(5, 5, (int x, int y) => {
                var t = new X();
                t.secret = x + ":" + y;
                return t;
            });
            //            test = new GridSet<X>
        }

        [TestMethod()]
        public void GridSetTest()
        {
            Trace.WriteLine("this is going to work for sure");
            Assert.IsTrue(true);
            Trace.WriteLine(n);
        }

        [TestMethod()]
        public void InitializerConstructorTest()
        {
            var z = new GridSet<X>(5, 5, (int x, int y) => {
                var t = new X();
                t.secret = x + ":" + y;
                return t;
            });
            Assert.AreEqual(z[0, 0].secret, "0:0");
            Assert.AreEqual(z[0, 4].secret, "0:4");
            Assert.AreEqual(z[1, 2].secret, "1:2");
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
        public void NeighoursCountTest()
        {
           Assert.AreEqual(8, n.GetNeighbours(2, 2).Length);
           Assert.AreEqual(5, n.GetNeighbours(0, 2).Length);
           Assert.AreEqual(3, n.GetNeighbours(0, 0).Length);
           Assert.AreEqual(3, n.GetNeighbours(4, 4).Length);
           Assert.AreEqual(2, n.GetNeighbours(0, 0,false).Length);

            Assert.AreEqual(4, n.GetNeighbours(2, 2, false).Length);
        }

        [TestMethod()]
        public void NeighoursValuesTest()
        {
            var c = n.GetNeighbours(2,2);
            Assert.IsTrue(c.Any(n => n.element.secret == "1:1"));
            Assert.IsTrue(c.Any(n => n.element.secret == "3:3"));
            Assert.IsFalse(c.Any(n => n.element.secret == "2:2"));

            Assert.IsTrue(c.Any(n => n.offsetX == -1));
            Assert.IsTrue(c.Any(n => n.offsetX == 1));

            Assert.IsTrue(c.All(v => v.original == n[2,2]));

        }

        [TestMethod()]
        public void LoopTest()
        {
            var g = new GridSet<X>(5, 5);
            int count = 0;
            g.Loop((X val, int x, int y) =>
            {
                count++;
            });
            Assert.AreEqual(count, 25);
        }

        [TestMethod()]
        public void FillAccessorTest()
        {
            var g = new GridSet<X>(5,5);
            g.Fill((int x, int y) =>
            {
                var t = new X();
                t.secret = x + ":" + y;
                return t;
            });

            Assert.AreEqual(g[0, 1].secret, "0:1");
            Assert.AreEqual(g[2, 1].secret, "2:1");
        }

        [TestMethod()]
        public void GetSectionTest()
        {
            n[2, 2].test = true;
            var l = n.GetSection(2, 2, 2, 2);
            Assert.IsTrue(l[0, 0].test);
            Assert.AreEqual(l.width, 2);
            Assert.AreEqual(l[0, 0].secret, n[2, 2].secret);

            try
            {
                var t = n.GetSection(3, 0, 5, 1);
                Assert.Fail();
            }
            catch (System.Exception e)
            {
           //     Assert.Fail();
            }
        }
    }
}