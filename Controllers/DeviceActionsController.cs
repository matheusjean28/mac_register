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
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            var device = await _db.Devices.ToListAsync();
            return device;
        }

        [HttpPost("/CreateDevice")]
        public async Task<ActionResult<Device>> CreateDevice([FromBody] Device device)
        {
            try
            {   var problem = new ProblemTreatWrapper{
                Name = device.Problem[0].Name,
                Description = device.Problem[0].Description,
                };

                var usedAtWrapper = new UsedAtWrapper{
                        
                    Name = device.UsedAtWrapper[0].Name,
                };
                
                if (device == null)
                {
                    return BadRequest("Device Params cannot be null");
                }
                if ( IsValidParam(device.Model) && IsValidParam(device.Mac) )
                {
                    
                    return BadRequest("somenthing went error");
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

        public bool IsValidParam(string param)
        {
            if (param.Length == 0 || param.Length < 5)
            {
                return false;
            }
            else
            {
                return true;
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
