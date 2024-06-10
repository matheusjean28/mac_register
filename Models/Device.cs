using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;

namespace DeviceModel;

public class DeviceCreate
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string DeviceId { get; set; }

    public string Model { get; set; } = string.Empty;
    public string Mac { get; set; } = string.Empty;
    public bool RemoteAcess { get; set; } = false;
    public List<ProblemTreatWrapper> Problems { get; set; }
    public List<UsedAtWrapper> UsedAtWrappers { get; set; }
}



// checkDate, mac, model, problem, remoteAccess, signalRX id: 123, name
