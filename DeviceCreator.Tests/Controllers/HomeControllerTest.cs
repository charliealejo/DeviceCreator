using DeviceCreator.Controllers;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace DeviceCreator.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Test()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = (ViewResult)controller.Test();

            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            var expectedVersion = mvcName.Version.Major + "." + mvcName.Version.Minor;
            var expectedRuntime = isMono ? "Mono" : ".NET";

            // Assert
            Assert.AreEqual(expectedVersion, result.ViewData["Version"]);
            Assert.AreEqual(expectedRuntime, result.ViewData["Runtime"]);
        }
    }
}
