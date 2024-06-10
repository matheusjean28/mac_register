using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("DeviceId")]
        public string DeviceId { get; set; }
        public DeviceCreate Device { get; set; }

        public ProblemTreatWrapper() { }
    }
}