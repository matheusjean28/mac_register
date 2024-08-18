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
        //refer id of the maker that owns this model
        public string Category_Id_Device { get; set; }

        public string Mac { get; set; }
        public bool RemoteAcess { get; set; }
        public string DeviceCategoryName { get; set; }

        //map array of the problems that come from array of params
        public string ProblemName { get; set; }
        public string ProblemDescription { get; set; }

        //map array of the old users that come from array of params
        public string UserName { get; set; }

        //map to array of SinalHistory
        public string SinalRX { get; set; }
        public string SinalTX { get; set; }
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
