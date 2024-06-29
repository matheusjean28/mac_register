using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MacSave.Models.Categories.OperationModelEnums;

namespace MacSave.Models.Categories
{
    public class DevicesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DeviceId { get; set; }

        public string ModelOfDevice { get; set; }

        public string Ownew { get; set; }

        public OperationModelEnum OperationMode { get; set; } = OperationModelEnum.None;
    }
}
