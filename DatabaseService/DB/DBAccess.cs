using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DatabaseService.DB
{
    /// <summary>
    /// Class to access the DB.
    /// </summary>
    /// <remarks>In this case the DB is going to be represented by a collection
    /// in memory.</remarks>
    public class DBAccess : IDBAccess
    {
        /// <summary>
        /// Instance of the collection of saved devices. This is only a way to save the devices
        /// in a list in memory while the server is running. It should be changed for a 
        /// connection to DB in order to permanently save the devices.
        /// </summary>
        readonly IList<IDevice> deviceList;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DeviceCreator.Controllers.DB.DBAccess"/> class.
        /// </summary>
        public DBAccess()
        {
            deviceList = new List<IDevice>();
        }

        /// <summary>
        /// Saves a devices into the database.
        /// </summary>
        /// <param name="device">Device to save.</param>
        /// <returns><c>true</c> if the device was properly inserted into the
        /// database, <c>false</c> otherwise.</returns>
        /// <remarks>This method is synchronized, hence it acts as a critical section
        /// so that only one device can be added at a given time.</remarks>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SaveDevice(IDevice device)
        {
            if (deviceIsAlreadyInTheList(device))
                return false;
            deviceList.Add(device);
            return true;
        }

        /// <summary>
        /// Gets the devices in the database.
        /// </summary>
        /// <returns>The devices already present in the database.</returns>
        public IEnumerable<IDevice> GetDevices()
        {
            return deviceList;
        }

        /// <summary>
        /// Checks if a device is already present in the list of registered devices.
        /// </summary>
        /// <returns><c>true</c>, if is already in the list was deviced, <c>false</c> otherwise.</returns>
        /// <param name="device">Device to check.</param>
        private bool deviceIsAlreadyInTheList(IDevice device)
        {
            return deviceList.Any(d => d.ID.Equals(device.ID) &&
                                  d.SerialNumber.Equals(device.SerialNumber));
        }
    }
}
