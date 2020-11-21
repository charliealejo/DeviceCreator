using System;
using DeviceCreator.Models.Web;
using Shared.Models;

namespace DeviceCreator.Models.Factory
{
    /// <summary>
    /// Class that implements the device factory interface.
    /// </summary>
    public class DeviceFactory : IDeviceFactory
    {
        /// <summary>
        /// <see cref="IDeviceFactory.CreateDevice(DeviceForm)"/>
        /// </summary>
        public IDevice CreateDevice(DeviceForm form)
        {
            switch (form.Type) 
            {
                case DeviceType.WATER_METER:
                    return new WaterMeter
                    {
                        ID = form.ID,
                        SerialNumber = form.SerialNumber,
                        FirmwareVersion = form.FirmwareVersion,
                        State = form.State
                    };
                case DeviceType.ELECTRICITY_METER:
                    return new ElectricityMeter
                    {
                        ID = form.ID,
                        SerialNumber = form.SerialNumber,
                        FirmwareVersion = form.FirmwareVersion,
                        State = form.State
                    };
                case DeviceType.GATEWAY:
                    return new Gateway
                    {
                        ID = form.ID,
                        SerialNumber = form.SerialNumber,
                        FirmwareVersion = form.FirmwareVersion,
                        State = form.State,
                        IP = form.IP,
                        Port = form.Port
                    };
                default:
                    throw new ArgumentException("Unknown device type");
            }
        }
    }
}
