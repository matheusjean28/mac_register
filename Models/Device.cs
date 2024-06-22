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

    public ICollection<ProblemTreatWrapper> Problems { get; set; } = new List<ProblemTreatWrapper>();

    public ICollection<UsedAtWrapper> UsedAtClients { get; set; } = new List<UsedAtWrapper>();

    public void AddProblems(ProblemTreatWrapper problem){
    Problems.Add(problem);
    }

    public void AddClients(UsedAtWrapper user){
        UsedAtClients.Add(user);
    }
    
}



// checkDate, mac, model, problem, remoteAccess, signalRX id: 123, name
