using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;

namespace mac_register.Models.DTOS
{
    public class CreateDeviceDTO
    {
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; } = "";
        public List<UsedAtWrapper> UserdAt { get; set; } = new List<UsedAtWrapper>();
        public List<ProblemTreatWrapper> Problem { get; set; } = new List<ProblemTreatWrapper>();
    }
}
