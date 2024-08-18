using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MacSave.Models.Categories.Models_of_Devices;

namespace MacSave.Models.Categories
{
    public class Maker
    {

        [Key]
        
        public string MakerId { get; set; } = Guid.NewGuid().ToString();

        public string MakerName { get; set; }

        [JsonIgnore]
        public ICollection<DeviceCategory> DeviceCategories { get; set; } =
            new List<DeviceCategory>();

        public void AddDeviceCategory(DeviceCategory deviceCategory)
        {
            DeviceCategories.Add(deviceCategory);
        }
    }
}
