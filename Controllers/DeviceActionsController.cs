using DeviceContext;
using DeviceModel;
using mac_register.Models.FullDeviceCreate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;

namespace Controller.DeviceActionsController
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceActionsController : ControllerBase
    {
        private readonly DeviceDb _db;

        public DeviceActionsController(DeviceDb db)
        {
            _db = db;
        }

        [HttpGet("GetAllDevices")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllDevices()
        {
            try
            {
                var devices = await _db
                    .Devices.Include(d => d.Problems)
                    .Include(d => d.UsedAtClients)
                    .Select(d => new
                    {
                        d.DeviceId,
                        d.Model,
                        d.Mac,
                        d.RemoteAcess,
                        Problems = d.Problems.Select(p => new
                        {
                            p.Id,
                            p.Name,
                            p.Description,
                            p.DeviceId
                        }),
                        UsedAtClients = d.UsedAtClients.Select(u => new
                        {
                            u.Id,
                            u.Name,
                            u.DeviceId
                        })
                    })
                    .ToListAsync();

                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve devices: {ex.Message}");
            }
        }

        [HttpPost("CreateDevice")]
        public async Task<ActionResult<FullDeviceCreate>> CreateDevice(
            [FromBody] FullDeviceCreate device
        )
        {
            if (device == null)
            {
                return BadRequest("Device cannot be null");
            }

            try
            {
                //creating a device and save it
                var deviceMac = new DeviceCreate
                {
                    Model = device.Model,
                    Mac = device.Mac,
                    RemoteAcess = device.RemoteAcess,
                };
                await _db.Devices.AddAsync(deviceMac);
                await _db.SaveChangesAsync();

                //instance a new problem and save it
                var problem = new ProblemTreatWrapper
                {
                    Name = device.Name,
                    Description = device.Description,
                    DeviceId = deviceMac.DeviceId
                };
                deviceMac.Problems.Add(problem);

                //instance a new user and save it
                var usedAt = new UsedAtWrapper
                {
                    Name = device.UserName,
                    DeviceId = deviceMac.DeviceId
                };
                deviceMac.UsedAtClients.Add(usedAt);

                await _db.SaveChangesAsync();

                var responde = new
                {
                    deviceMac,
                    usedAt,
                    problem
                };

                return Ok(responde);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create device: {ex.Message}");
            }
        }
    }
}
