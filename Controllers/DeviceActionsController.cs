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
    }
}
