using MacSave.Models.Categories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MacSave.Models.Categories.OperationModelEnums;

namespace MacSave.Models.Categories.Models_of_Devices
{
    public class DeviceCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DeviceCategoryId { get; set; } = Guid.NewGuid().ToString();

        public string DeviceCategoryName { get; set; }

        //if Operation mode was not provide, then default value is set to NONE, but can change before
        public OperationModelEnum OperationMode { get; set; } = OperationModelEnum.None;

        [ForeignKey("Maker")]
        public  string MakerId { get; set; }

        [JsonIgnore]
        public Maker Maker { get; set; } = null!;
    }
}
