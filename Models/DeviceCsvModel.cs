using CsvHelper.Configuration.Attributes;
using DeviceInterface.Interfaces;

namespace DeviceCsv.Model
{
    public class Device : IDevice
    {

        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;


        public string Data { get; set; } = string.Empty;

        [BooleanTrueValues("false")]
        [BooleanFalseValues("true")]
        public bool RemoteAccess { get; set; } = false;

        [BooleanTrueValues("false")]
        [BooleanFalseValues("true")]
        public bool Problem { get; set; } = false;
    }
}