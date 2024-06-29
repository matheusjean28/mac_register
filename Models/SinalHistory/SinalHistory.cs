using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DeviceModel;

namespace MacSave.Models.SinalHistory
{
    public class SinalHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string SinalRX { get; set; } = "-00.00";
        public string SinalTX { get; set; } = "-00.00";

        [ForeignKey("DeviceCreate")]
        public required string DeviceId { get; set; }

        [JsonIgnore] //ignore propetie on act at db
        public DeviceCreate DeviceCreate { get; set; } = null!;

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
