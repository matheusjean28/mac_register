using DeviceContext;
using DeviceModel;
using mac_register.Models.FullDeviceCreate;
using Microsoft.AspNetCore.Mvc;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost("CreateDevice")]
        public async Task<ActionResult<FullDeviceCreate>> CreateDevice([FromBody] FullDeviceCreate device)
        {
            if (device == null)
            {
                return BadRequest("Device cannot be null");
            }

            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var deviceMac = new DeviceCreate
                {
                    Model = device.Model,
                    Mac = device.Mac,
                    RemoteAcess = device.RemoteAcess,
                };

                await _db.Devices.AddAsync(deviceMac);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                var problem = new ProblemTreatWrapper
                {
                    Name = device.Name,
                    Description = device.Description,
                    DeviceId = deviceMac.DeviceId
                };
                deviceMac.AddProblems(problem);

                var usedAt = new UsedAtWrapper
                {
                    Name = device.UserName,
                    DeviceId = deviceMac.DeviceId
                };
                deviceMac.AddClients(usedAt);

                await _db.SaveChangesAsync();

                return Ok(deviceMac);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Failed to create device: {ex.Message}");
            }
        }
    }
}
