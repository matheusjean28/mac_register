using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;

namespace DeviceModel;

public class Device
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Model { get; set; } = string.Empty;
    public string Mac { get; set; } = string.Empty;
    public bool RemoteAcess { get; set; } = false;
    public List<ProblemTreatWrapper> Problem { get; set; }
    public List<UsedAtWrapper> UsedAtWrapper { get; set; }
}



// checkDate, mac, model, problem, remoteAccess, signalRX id: 123, name
