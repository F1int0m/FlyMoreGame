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
    class DroneTest
    {
        [TestCase(50,50)]
        [TestCase(0,0)]
        [TestCase(100,100)]
        [TestCase(150,100)]
        [TestCase(-70,0)]

        [Test]
        public void TestOverThrottle(int d, int expected)
        {
            var drone = new Drone();
            drone.Throttle += d;
            Assert.IsTrue(drone.Throttle==expected);
        }

        
    }
}
