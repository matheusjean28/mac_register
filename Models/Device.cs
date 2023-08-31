
namespace DeviceModel;

        public class Device{
            public int Id {get; set;}
            public string Model {get; set;} = string.Empty;
            public string Mac {get; set;} = string.Empty;
            public bool RemoteAcess;
            public bool Problem;
        }
