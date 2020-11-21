using System;

namespace Shared.Models
{
    /// <summary>
    /// The interface that represents a device.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Gets or sets the identifier for the device.
        /// </summary>
        /// <value>The identifier for the device.</value>
        string ID { get; set; }

        /// <summary>
        /// Gets or sets the serial number of the device.
        /// </summary>
        /// <value>The serial number of the device.</value>
        string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the firmware version of the device.
        /// </summary>
        /// <value>The firmware version of the device.</value>
        string FirmwareVersion { get; set; }

        /// <summary>
        /// Gets or sets the state of the device.
        /// </summary>
        /// <value>The state of the device.</value>
        string State { get; set; }
    }
}
