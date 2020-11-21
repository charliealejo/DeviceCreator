using DeviceCreator.App_LocalResources;
using System.ComponentModel.DataAnnotations;

namespace DeviceCreator.Models.Web
{
    /// <summary>
    /// An enumeration for the device types.
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// A water meter.
        /// </summary>
        [Display(Name = "WaterMeter", ResourceType = typeof(Resources))]
        WATER_METER,

        /// <summary>
        /// An electricity meter.
        /// </summary>
        [Display(Name = "ElectricityMeter", ResourceType = typeof(Resources))]
        ELECTRICITY_METER,

        /// <summary>
        /// A gateway.
        /// </summary>
        [Display(Name = "Gateway", ResourceType = typeof(Resources))]
        GATEWAY
    }
}
