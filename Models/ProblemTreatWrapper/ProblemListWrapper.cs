using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DeviceModel;

namespace Model.ProblemTreatWrapper
{
    public class ProblemTreatWrapper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [ForeignKey("DeviceCreate")]
        public required string DeviceId { get; set; }

        [JsonIgnore]//ignore propetie on act at db
        public DeviceCreate DeviceCreate { get; set; } = null!;
    }
}
