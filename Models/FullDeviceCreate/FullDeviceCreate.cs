using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Any;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;

namespace mac_register.Models.FullDeviceCreate
{
    public class FullDeviceCreate
    {
        public string Model { get; set; }
        public string Mac { get; set; }
        public bool RemoteAcess { get; set; }

        //map array of the problems that come from array of params
        public string Name { get; set; }
        public string Description { get; set; }

        //map array of the old users that come from array of params
        public string UserName { get; set; }
    }
}

// {
//   "deviceId": "string",
//   "model": "string",
//   "mac": "string",
//   "remoteAcess": true,
//   "problems": [
//     {
//       "id": "string",
//       "name": "string",
//       "description": "string",
//       "deviceId": "string",
//       "device": "string"
//     }
//   ],
//   "usedAtWrappers": [
//     {
//       "id": "string",
//       "name": "string",
//       "deviceId": "string",
//       "device": "string"
//     }
//   ]
// }
