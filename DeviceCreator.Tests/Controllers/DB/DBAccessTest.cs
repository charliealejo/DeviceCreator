using DatabaseService.DB;
using NUnit.Framework;
using Shared.Models;
using System.Linq;

namespace DeviceCreator.Tests.Controllers.DB
{
    [TestFixture]
    public class DBAccessTest
    {
        DBAccess dbAccessUT;

        [SetUp]
        public void SetUp()
        {
            dbAccessUT = new DBAccess();
        }

        [Test]
        public void ShouldSaveADevice()
        {
            dbAccessUT.SaveDevice(new WaterMeter());

            var actual = dbAccessUT.GetDevices().Count();
            var expected = 1;

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldNotSaveADuplicatedDevice()
        {
            var device = new ElectricityMeter { ID = "1", SerialNumber = "1" };

            Assert.True(dbAccessUT.SaveDevice(device));
            Assert.False(dbAccessUT.SaveDevice(device));

            var actual = dbAccessUT.GetDevices().Count();
            var expected = 1;

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldNotSaveADeviceWithExistingIDAndSerialNumber()
        {
            dbAccessUT.SaveDevice(new ElectricityMeter { ID = "1", SerialNumber = "1" });
            dbAccessUT.SaveDevice(new WaterMeter { ID = "2", SerialNumber = "2" });
            dbAccessUT.SaveDevice(new Gateway { ID = "1", SerialNumber = "1" });

            var actual = dbAccessUT.GetDevices().Count();
            var expected = 2;

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
