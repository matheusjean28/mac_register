using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mac_register.Models.DTOS
{
    public class GetAllDevicesDTO
    {
        public class DeviceDTO
        {
            public string DeviceId { get; set; }
            public string Model { get; set; }
            public string Mac { get; set; }
            public bool RemoteAccess { get; set; }
        }

    }
}
