using DeviceCreator.Models.Web;
using Shared.Models;

namespace DeviceCreator.Models.Factory
{
    /// <summary>
    /// Interface for the device factory.
    /// </summary>
    public interface IDeviceFactory
    {
        /// <summary>
        /// Creates a device.
        /// </summary>
        /// <returns>The device.</returns>
        /// <param name="form">The data of the device to create.</param>
        IDevice CreateDevice(DeviceForm form);
    }
}