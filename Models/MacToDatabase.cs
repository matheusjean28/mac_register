using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using MacToDatabaseInterface.Interface;

namespace MacToDatabaseModel
{
    public class MacToDatabase : IMacToDatabase
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;

        [BooleanTrueValues]
        public bool Problem { get; set; }
        [BooleanTrueValues]

        public bool RemoteAccess { get; set; }


    }
}