using System;

namespace Shared.Models
{
    /// <summary>
    /// The interface for the enroutable devices.
    /// </summary>
    public interface IEnroutable
    {
        /// <summary>
        /// Gets or sets the IP address of the device.
        /// </summary>
        /// <value>The ip.</value>
        string IP { get; set; }

        /// <summary>
        /// Gets or sets the port to access the device.
        /// </summary>
        /// <value>The port to access the device.</value>
        /// <remarks>This value can be null.</remarks>
        int? Port { get; set; }
    }
}
