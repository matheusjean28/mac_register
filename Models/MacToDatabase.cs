using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using MacToDatabaseInterface.Interface;
using Model.ProblemTreatWrapper;

namespace MacToDatabaseModel
{
    public class MacToDatabase
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;

        // public List<ProblemTreatWrapper> Problems { get; set; } = new List<ProblemTreatWrapper>();

        [BooleanTrueValues]
        public bool RemoteAccess { get; set; }
    }
}
