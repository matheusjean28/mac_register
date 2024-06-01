using DeviceContext;
using DeviceModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            {

                if (device == null)
                {
                    return BadRequest("Device Params cannot be null");
                }
                if( device.Model.Length <= 2 ){
                    Console.WriteLine(device.Model);
                    return BadRequest("lowersss");
                }
                return Ok(device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
