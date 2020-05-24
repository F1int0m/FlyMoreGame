using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FlyMore.Tests
{
    [TestFixture]
    class WorldSimpleTest
    {

        [TestCase(0, 0)]
        [TestCase(-Math.PI, -Math.PI)]

        [Test]
        public void TestAngle(double angleOn, double expected)
        {
            var world = new World();
            world.Update(angleOn, 0, new Size(800, 800), 10);
            Assert.IsTrue(world.drone.Angle == expected);
        }

        [Test]
        public void ElementsTest()
        {
            var w = new World();
            w.Load(Gate.GeneateGate(10));
            Assert.IsTrue(w.Elements!=null&&w.Elements.Count>0);
        }

        [Test]
        public void CheckTest()
        {
            var size = new Size(800, 800);
            var testDrone = new Drone(Vector.Zero, Vector.Zero, Math.PI, 0);
            var w = WorldInit(size, testDrone);
            w.drone = new Drone(new Vector(28, 500), new Vector(1, 0), 0, 40);
            w.Update(0, 0, size, 10);
            w.Update(0, 0, size, 10);
            Assert.IsTrue(w.Score == 1);
            Assert.IsFalse(w.IsWin);


        }

        [Test]
        public void FlyTest()
        {
            var size = new Size(800, 800);
            var testDrone = new Drone(Vector.Zero, Vector.Zero, Math.PI, 0);
            var w = WorldInit(size, testDrone);
            w.Update(Math.PI/4,100,size,10);
            w.Update(Math.PI/4,100,size,10);
            Assert.IsFalse(w.drone.Position.Equals(testDrone.Position));
        }
        

        [Test]
        public void WinTest()
        {
            var size = new Size(800, 800);
            var testDrone = new Drone(Vector.Zero, Vector.Zero, Math.PI, 0);
            var w = WorldInit(size, testDrone);
            w.Load(Gate.GeneateGate(110));
            w.drone = new Drone(new Vector(125, 500), new Vector(1, 0), 0, 40);
            w.Update(0, 0, size, 10);
            w.Update(0, 0, size, 10);
            w.Update(0, 0, size, 10);
            Assert.IsTrue(w.IsWin);
        }





        private static World WorldInit(Size size, Drone testDrone)
        {
            var w = new World {drone = testDrone};
            w.Load(Gate.GeneateGate(10), Gate.GeneateGate(500));
            for (int i = 0; i < 20; i++)
            {
                w.Update(0, 0, size, 10);
            }
            return w;
        }

    }
}
