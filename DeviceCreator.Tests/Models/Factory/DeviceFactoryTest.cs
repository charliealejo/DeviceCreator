using DeviceCreator.Models.Factory;
using DeviceCreator.Models.Web;
using NUnit.Framework;
using Shared.Models;

namespace DeviceCreator.Tests.Models.Factory
{
    [TestFixture]
    public class DeviceFactoryTest
    {
        DeviceFactory deviceFactoryUT;

        [SetUp]
        public void SetUp()
        {
            deviceFactoryUT = new DeviceFactory();
        }

        [Test]
        public void ShouldCreateAWaterMeter()
        {
            var form = new DeviceForm
            {
                Type = DeviceType.WATER_METER,
                ID = "42",
                SerialNumber = "42",
                FirmwareVersion = "1.0",
                State = "Connected"
            };

            var actual = deviceFactoryUT.CreateDevice(form);

            Assert.That(actual, Is.InstanceOf<WaterMeter>());

            var actualDevice = actual as WaterMeter;
            Assert.That(actualDevice.ID, Is.EqualTo(form.ID));
            Assert.That(actualDevice.SerialNumber, Is.EqualTo(form.SerialNumber));
            Assert.That(actualDevice.FirmwareVersion, Is.EqualTo(form.FirmwareVersion));
            Assert.That(actualDevice.State, Is.EqualTo(form.State));
        }

        [Test]
        public void ShouldCreateAnElectricityMeter()
        {
            var form = new DeviceForm
            {
                Type = DeviceType.ELECTRICITY_METER,
                ID = "42",
                SerialNumber = "42",
                FirmwareVersion = "1.1",
                State = "Disconnected"
            };

            var actual = deviceFactoryUT.CreateDevice(form);

            Assert.That(actual, Is.InstanceOf<ElectricityMeter>());

            var actualDevice = actual as ElectricityMeter;
            Assert.That(actualDevice.ID, Is.EqualTo(form.ID));
            Assert.That(actualDevice.SerialNumber, Is.EqualTo(form.SerialNumber));
            Assert.That(actualDevice.FirmwareVersion, Is.EqualTo(form.FirmwareVersion));
            Assert.That(actualDevice.State, Is.EqualTo(form.State));
        }

        [Test]
        public void ShouldCreateAGateway()
        {
            var form = new DeviceForm
            {
                Type = DeviceType.GATEWAY,
                ID = "42",
                SerialNumber = "42",
                FirmwareVersion = "1.2",
                State = "Online",
                IP = "5.6.7.8",
                Port = 666
            };

            var actual = deviceFactoryUT.CreateDevice(form);

            Assert.That(actual, Is.InstanceOf<Gateway>());

            var actualDevice = actual as Gateway;
            Assert.That(actualDevice.ID, Is.EqualTo(form.ID));
            Assert.That(actualDevice.SerialNumber, Is.EqualTo(form.SerialNumber));
            Assert.That(actualDevice.FirmwareVersion, Is.EqualTo(form.FirmwareVersion));
            Assert.That(actualDevice.State, Is.EqualTo(form.State));
            Assert.That(actualDevice.IP, Is.EqualTo(form.IP));
            Assert.That(actualDevice.Port, Is.EqualTo(form.Port));
        }
    }
}
