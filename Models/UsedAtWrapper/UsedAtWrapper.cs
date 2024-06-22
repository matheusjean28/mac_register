using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeviceModel;

namespace Models.UsedAtWrapper.UsedAtWrapper
{
    public class UsedAtWrapper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("DeviceCreate")]
        public string DeviceId { get; set; }
        public DeviceCreate DeviceCreate { get; set; }  = null!;
    }
}