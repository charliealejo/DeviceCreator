using DeviceCreator.Controllers;
using DeviceCreator.Models.Factory;
using DeviceCreator.Models.Web;
using DeviceManager;
using Moq;
using NUnit.Framework;
using Shared.Models;
using System.Web.Mvc;

namespace DeviceCreator.Tests.Controllers
{
    [TestFixture()]
    public class DeviceControllerTest
    {
        DeviceController controllerUT;
        Mock<IDevicePublisher> dbAccessMock;
        Mock<IDeviceFactory> deviceFactoryMock;

        [SetUp]
        public void SetUp()
        {
            dbAccessMock = new Mock<IDevicePublisher>();
            dbAccessMock.Setup(dam => dam.SaveDevice(It.IsAny<IDevice>()))
                        .Returns<IDevice>(d => !d.ID.Equals("AlreadyExists") ||
                                          !d.SerialNumber.Equals("AlreadyExists"));

            deviceFactoryMock = new Mock<IDeviceFactory>();
            deviceFactoryMock.Setup(dfm => dfm.CreateDevice(
                It.Is<DeviceForm>(f => f.Type == DeviceType.WATER_METER))).Returns<DeviceForm>(
                    f => new WaterMeter { ID = f.ID, SerialNumber = f.SerialNumber });
            deviceFactoryMock.Setup(dfm => dfm.CreateDevice(
                It.Is<DeviceForm>(f => f.Type == DeviceType.ELECTRICITY_METER))).Returns<DeviceForm>(
                    f => new ElectricityMeter { ID = f.ID, SerialNumber = f.SerialNumber });
            deviceFactoryMock.Setup(dfm => dfm.CreateDevice(
                It.Is<DeviceForm>(f => f.Type == DeviceType.GATEWAY))).Returns<DeviceForm>(
                    f => new Gateway { ID = f.ID, SerialNumber = f.SerialNumber });

            controllerUT = new DeviceController(dbAccessMock.Object, deviceFactoryMock.Object);
        }

        [Test]
        public void ShouldCreateTheDeviceListView()
        {
            controllerUT.Index();

            dbAccessMock.Verify(dam => dam.GetDevices(), Times.Once());
        }

        [Test]
        public void ShouldCreateANewWaterMeter()
        {
            var form = new DeviceForm
            {
                Type = DeviceType.WATER_METER,
                ID = "42",
                SerialNumber = "42",
                FirmwareVersion = "1.0",
                State = "Connected"
            };

            var result = (RedirectToRouteResult)controllerUT.Create(form);

            dbAccessMock.Verify(
                dbm => dbm.SaveDevice(It.Is<IDevice>(d => d is WaterMeter)),
                Times.Once());

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void ShouldCreateANewElectricityMeter()
        {
            var form = new DeviceForm
            {
                Type = DeviceType.ELECTRICITY_METER,
                ID = "42",
                SerialNumber = "42",
                FirmwareVersion = "1.1",
                State = "Disconnected"
            };

            var result = (RedirectToRouteResult)controllerUT.Create(form);

            dbAccessMock.Verify(
                dbm => dbm.SaveDevice(It.Is<IDevice>(d => d is ElectricityMeter)),
                Times.Once());

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void ShouldCreateANewGateway()
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

            var result = (RedirectToRouteResult)controllerUT.Create(form);

            dbAccessMock.Verify(
                dbm => dbm.SaveDevice(It.Is<IDevice>(d => d is Gateway)),
                Times.Once());

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void ShouldNotCreateADeviceIfTheIDIsMissing()
        {
            var form = new DeviceForm
            {
                Type = DeviceType.WATER_METER,
                ID = null,
                SerialNumber = "42",
                FirmwareVersion = "1.3",
                State = "ReadyForReconnection"
            };

            controllerUT.ModelState.AddModelError("ID", "Missing ID");
            var result = (ViewResult)controllerUT.Create(form);

            dbAccessMock.Verify(
                dbm => dbm.SaveDevice(It.Is<IDevice>(d => d is WaterMeter)),
                Times.Never());

            Assert.That(result.Model, Is.EqualTo(form));
        }

        [Test]
        public void ShouldNotCreateADeviceIfTheSerialNumberIsMissing()
        {
            var form = new DeviceForm
            {
                Type = DeviceType.WATER_METER,
                ID = "42",
                SerialNumber = null,
                FirmwareVersion = "1.4",
                State = "Connected"
            };

            controllerUT.ModelState.AddModelError("SerialNumber", "Missing serial number");
            var result = (ViewResult)controllerUT.Create(form);

            dbAccessMock.Verify(
                dbm => dbm.SaveDevice(It.Is<IDevice>(d => d is WaterMeter)),
                Times.Never());

            Assert.That(result.Model, Is.EqualTo(form));
        }

        [Test]
        public void ShouldAddAModelStateErrorIfADeviceWithSameIDAndSerialNumberIsPresent()
        {
            var form = new DeviceForm
            {
                Type = DeviceType.WATER_METER,
                ID = "AlreadyExists",
                SerialNumber = "AlreadyExists",
                FirmwareVersion = "1.5",
                State = "Offline"
            };

            var result = (ViewResult)controllerUT.Create(form);

            Assert.That(result.Model, Is.EqualTo(form));
            Assert.False(controllerUT.ModelState.IsValid);
        }

        [Test]
        public void ShouldNotCreateAGatewayIfTheIPIsMissing()
        {
            var form = new DeviceForm
            {
                Type = DeviceType.GATEWAY,
                ID = "42",
                SerialNumber = "42",
                FirmwareVersion = "1.6",
                State = "Connected",
                IP = null,
                Port = null
            };

            var result = (ViewResult)controllerUT.Create(form);

            dbAccessMock.Verify(
                dbm => dbm.SaveDevice(It.Is<IDevice>(d => d is WaterMeter)),
                Times.Never());

            Assert.That(result.Model, Is.EqualTo(form));
        }

        [Test]
        public void ShouldNotCreateAnythingIfTheFormIsNull()
        {
            DeviceForm form = null;

            var result = (ViewResult)controllerUT.Create(form);

            dbAccessMock.Verify(
                dbm => dbm.SaveDevice(It.IsAny<IDevice>()),
                Times.Never());

            Assert.That(controllerUT.ViewData["ErrorMessage"], Is.Not.Null);
            Assert.That(result.Model, Is.Null);
        }
    }
}
