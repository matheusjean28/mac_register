using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DeviceModel;

namespace Models.UsedAtWrapper.UsedAtWrapper
{
    public class UsedAtWrapper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        [ForeignKey("DeviceCreate")]
        public string DeviceId { get; set; }

        [JsonIgnore]
        public DeviceCreate DeviceCreate { get; set; } = null!;
    }
}
