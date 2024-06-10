using DeviceContext;
using DeviceModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;

namespace Controller.DeviceActionsController
{
    [ApiController]
    public class DeviceActionsController : ControllerBase
    {
        private readonly DeviceDb _db;

        public DeviceActionsController(DeviceDb db)
        {
            _db = db;
        }

        [HttpGet("/GetDevices")]
        public async Task<ActionResult<IEnumerable<DeviceCreate>>> GetDevices()
        {
            var device = await _db.Devices.ToListAsync();
            return device;
        }

        [HttpPost("/CreateDevice")]
        public async Task<ActionResult<DeviceCreate>> CreateDevice([FromBody] DeviceCreate device)
        {
            try
            {
                var problem = new ProblemTreatWrapper
                {
                    Name = device.Problems[0].Name,
                    Description = device.Problems[0].Description,
                };

                var usedAtWrapper = new UsedAtWrapper { Name = device.UsedAtWrappers[0].Name, };

                if (device == null)
                {
                    return BadRequest("Device Params cannot be null");
                }

                // var problem = new ProblemTreatWrapper{
                // Name = device.Problem[0].Name,
                // Description = device.Problem[0].Description,
                // };



                return Ok(problem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //         {
        //     "id": 0,
        //     "model": "string",
        //     "mac": "string",
        //     "remoteAcess": true,
        //     "problem": [
        //       {
        //         "id": 0,
        //         "name": "string",
        //         "description": "string"
        //       }
        //     ],
        //     "usedAtWrapper": [
        //       {
        //         "id": 0,
        //         "name": "string"
        //       }
        //     ]
        //   }
    }
}
