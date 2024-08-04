using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MacSave.Models.Categories.Models_of_Devices;
using MacSave.Models.SinalHistory;
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

    //reference Device Category
    [ForeignKey("DeviceCategory")]
    public string DeviceCategoryId { get; set; }

    [JsonIgnore]
    public DeviceCategory DeviceCategory { get; set; } = null!;

    [JsonIgnore]
    public ICollection<ProblemTreatWrapper> Problems { get; set; } =
        new List<ProblemTreatWrapper>();

    [JsonIgnore]
    public ICollection<UsedAtWrapper> UsedAtClients { get; set; } = new List<UsedAtWrapper>();

    [JsonIgnore]
    public ICollection<SinalHistory> SinalHistory { get; set; } = new List<SinalHistory>();

    public void AddProblems(ProblemTreatWrapper problem)
    {
        Problems.Add(problem);
    }

    public void AddClients(UsedAtWrapper user)
    {
        UsedAtClients.Add(user);
    }

    public void AddSinal(SinalHistory sinal)
    {
        SinalHistory.Add(sinal);
    }
}


// checkDate, mac, model, problem, remoteAccess, signalRX id: 123, name
