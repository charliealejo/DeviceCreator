using DeviceCreator.App_LocalResources;
using System.ComponentModel.DataAnnotations;

namespace DeviceCreator.Models.Web
{
    public class DeviceForm
    {
        [Display(Name = "Type", ResourceType = typeof(Resources))]
        public DeviceType Type { get; set; }

        [Display(Name = "FirmwareVersion", ResourceType = typeof(Resources))]
        public string FirmwareVersion { get; set; }

        [Display(Name = "State", ResourceType = typeof(Resources))]
        public string State { get; set; }

        [Display(Name = "ID", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
              ErrorMessageResourceName = "IDRequired")]
        public string ID { get; set; }

        [Display(Name = "SerialNumber", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
              ErrorMessageResourceName = "SerialNumberRequired")]
        public string SerialNumber { get; set; }

        [Display(Name = "IP", ResourceType = typeof(Resources))]
        /*[RegularExpression(@"\b(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\." +
          @"(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\." + 
          @"(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\." +
          @"(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\b", 
              ErrorMessageResourceType = typeof(Resources),
              ErrorMessageResourceName = "InvalidIP")]*/
        public string IP { get; set; }

        [Display(Name = "Port", ResourceType = typeof(Resources))]
        [Range(0, 65535, ErrorMessageResourceType = typeof(Resources),
              ErrorMessageResourceName = "PortOutOfRange")]
        public int? Port { get; set; }
    }
}
