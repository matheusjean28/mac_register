namespace MacToDatabaseInterface.Interface
{
    public interface IMacToDatabase
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Mac { get; set; }
        public bool Problem { get; set; }
        public bool RemoteAccess { get; set; }

    }
}