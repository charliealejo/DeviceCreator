namespace Shared.Models
{
    /// <summary>
    /// Class that implements a gateway.
    /// </summary>
    public class Gateway : IDevice, IEnroutable
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

        /// <summary>
        /// <see cref="IEnroutable.IP"/>
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// <see cref="IEnroutable.Port"/>
        /// </summary>
        public int? Port { get; set; }

        public Gateway()
        {
        }
    }
}
