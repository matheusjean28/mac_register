using Model.ProblemTreatWrapper;
using System.Collections.Generic;

namespace MacToDatabaseInterface.Interface
{
    public interface IMacToDatabase
    {
        int Id { get; set; }
        string Model { get; set; }
        string Mac { get; set; }
        List<ProblemTreatWrapper> Problems { get; set; }
        bool RemoteAccess { get; set; }
    }
}
