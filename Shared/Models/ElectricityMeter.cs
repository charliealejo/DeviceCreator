﻿namespace Shared.Models
{
    /// <summary>
    /// Class that implements an electricity meter.
    /// </summary>
    public class ElectricityMeter : IDevice
    {
        /// <summary>
        /// <see cref="IDevice.ID"/>
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// <see cref="IDevice.SerialNumber"/>
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// <see cref="IDevice.FirmwareVersion"/>
        /// </summary>
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// <see cref="IDevice.State"/>
        /// </summary>
        public string State { get; set; }

        public ElectricityMeter()
        {
        }
    }
}
