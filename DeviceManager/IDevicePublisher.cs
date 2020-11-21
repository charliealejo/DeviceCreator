using System.Collections.Generic;
using Shared.Models;

namespace DeviceManager
{
    public interface IDevicePublisher
    {
        IEnumerable<IDevice> GetDevices();
        bool SaveDevice(IDevice device);
    }
}