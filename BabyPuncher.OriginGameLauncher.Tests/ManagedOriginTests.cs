using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BabyPuncher.OriginGameLauncher.ManagedOrigin;

namespace BabyPuncher.OriginGameLauncher.Tests
{
    [TestClass]
    public class ManagedOriginTests
    {
        [TestMethod]
        public void Origin_Start_Should_Start_Origin_Process()
        {
            var origin = new Origin();
            origin.StartOrigin();
            Assert.IsTrue(Origin.IsOriginRunning());
            Thread.Sleep(new TimeSpan(0, 0, 20));
            origin.KillOrigin();
        }
    }
}
