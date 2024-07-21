using DeviceContext;
using MacSave.Models.SinalHistory;
using DeviceModel;
using mac_register.Models.FullDeviceCreate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.ProblemTreatWrapper;
using MacSave.Funcs;
using Models.UsedAtWrapper.UsedAtWrapper;
using   MacSave.Models.Categories.Models_of_Devices;
namespace Controller.DeviceActionsController
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceActionsController : ControllerBase
    {
        private readonly DeviceDb _db;
        private readonly RegexService _regexService;

        public DeviceActionsController(DeviceDb db, RegexService regexService)
        {
            _db = db;
            _regexService = regexService;
        }

        [HttpGet("GetAllDevices")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllDevices()
        {
            try
            {
                var devices = await _db
                    .Devices.Include(d => d.Problems)
                    .Include(d => d.UsedAtClients)
                    .Include(d => d.SinalHistory)
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
                        }),
                        SinalHistory = d.SinalHistory.Select(s => new{
                            s.Id,
                            s.SinalRX,
                            s.SinalTX,
                            s.Timestamp
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
            [FromBody] FullDeviceCreate fullDeviceDiry
        )
        {
            var device = new FullDeviceCreate{
             Model = _regexService.SanitizeInput(fullDeviceDiry.Model),
             Mac = _regexService.SanitizeInput(fullDeviceDiry.Mac),
             RemoteAcess = fullDeviceDiry.RemoteAcess,
             Name = _regexService.SanitizeInput(fullDeviceDiry.Name),
             Description= _regexService.SanitizeInput(fullDeviceDiry.Description),
             UserName = _regexService.SanitizeInput(fullDeviceDiry.UserName),
             SinalRX = _regexService.SanitizeInput(fullDeviceDiry.SinalRX),
             SinalTX = _regexService.SanitizeInput(fullDeviceDiry.SinalTX),
           };

            if (device == null)
            {
                return BadRequest("Device cannot be null");
            }

            try
            {
                //  !!! FIND DDIRY PARAM 
                var deviceCategory = await _db.DeviceCategories.FindAsync(fullDeviceDiry.Model);
                if( deviceCategory == null){
                    return BadRequest("Invalid Device Model");
                }

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

                var sinalHistory = new SinalHistory
                {
                    SinalRX = device.SinalRX,
                    SinalTX = device.SinalTX,
                    DeviceId = deviceMac.DeviceId,
                };
                deviceMac.AddSinal(sinalHistory);

                //instance a new user and save it
                var usedAt = new UsedAtWrapper
                {
                    Name = device.UserName,
                    DeviceId = deviceMac.DeviceId
                };
                deviceMac.UsedAtClients.Add(usedAt);


                deviceCategory.AddDeviceCategory(deviceMac);
                await _db.SaveChangesAsync();


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
