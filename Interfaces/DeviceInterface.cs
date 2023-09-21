namespace DeviceInterface.Interfaces
{
    public interface IDevice
    {
        int Id { get; set; }
        string Model { get; set; }
        string Mac { get; set; }
        string Data { get; set; }
        bool RemoteAccess { get; set; } 
        bool Problem { get; set; }
    }
}