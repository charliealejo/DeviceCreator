using Shared.Models;
using System.Collections.Generic;

namespace DatabaseService.DB
{
    /// <summary>
    /// Interface with methods to access the device database.
    /// </summary>
    public interface IDBAccess
    {
        /// <summary>
        /// Saves a devices into the database.
        /// </summary>
        /// <param name="device">Device to save.</param>
        /// <returns><c>true</c> if the device was properly inserted into the
        /// database, <c>false</c> otherwise.</returns>
        bool SaveDevice(IDevice device);

        /// <summary>
        /// Gets the devices in the database.
        /// </summary>
        /// <returns>The devices already present in the database.</returns>
        IEnumerable<IDevice> GetDevices();
    }
}
