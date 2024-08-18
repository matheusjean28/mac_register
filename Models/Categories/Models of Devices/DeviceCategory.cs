using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DeviceModel;
using MacSave.Models.Categories;
using MacSave.Models.Categories.OperationModelEnums;

namespace MacSave.Models.Categories.Models_of_Devices
{
    public class DeviceCategory
    {
       

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DeviceCategoryId { get; set; } = Guid.NewGuid().ToString();

        public required string DeviceCategoryName { get; set; } 

        //if Operation mode was not provide, then default value is set to NONE, but can change before
        public OperationModelEnum OperationMode { get; set; } = OperationModelEnum.None;

        [ForeignKey("Maker")]
        public required string MakerId { get; set; }

        //comented to avoid error at controller, must be fixed
        // [JsonIgnore]
        // public Maker Maker { get; set; } = null!;

        [JsonIgnore]
        public ICollection<DeviceCreate> Devices { get; set; } = new List<DeviceCreate>();
        
        

        public void AddDeviceToCategory(DeviceCreate deviceCreate)
        {
            Devices.Add(deviceCreate);
        }
    }
}
