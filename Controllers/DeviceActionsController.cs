using DeviceContext;
using DeviceModel;
using MacSave.Models.SinalHistory;
using mac_register.Models.FullDeviceCreate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;
using MacSave.Funcs;

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
            _regexService = regexService;
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
                        SinalHistory = d.SinalHistory.Select(s => new
                        {
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

        [HttpGet("/SearchDevices/{paramDirty}")]
        public async Task<ActionResult<object>> SearchDevices(string paramDirty)
        {
            if (paramDirty == null || paramDirty.Length < 5)
            {
                return BadRequest("Params length is too small");
            }
            var param = _regexService.SanitizeInput(paramDirty);

            var deviceTask = _db
                .Devices.Where(d => d.Model.Contains(param))
                .Where(d => d.Mac.Contains(param))
                .ToListAsync();

            var problemTask = _db
                .Problems.Where(p => p.Name.Contains(param))
                .Where(p => p.Description.Contains(param))
                .ToListAsync();

            var usedAtTask = _db.UsedAtClient.Where(u => u.Name.Contains(param)).ToListAsync();

            var MakersTask = _db.Makers.Where(h => h.MakerName.Contains(param)).ToListAsync();

            var DeviceCategoriesTaks = _db
                .DeviceCategories.Where(c => c.DeviceCategoryName.Contains(param))
                .ToListAsync();

            await Task.WhenAll(deviceTask, problemTask, usedAtTask, MakersTask);

            var deviceResults = await deviceTask;
            var problemResults = await problemTask;
            var usedAtResults = await usedAtTask;
            var MakersTaskResults = await MakersTask;
            var DeviceCategoriesResults = await DeviceCategoriesTaks;

            var results = new
            {
                Devices = deviceResults,
                Problems = problemResults,
                UsedAtClients = usedAtResults,
                Makers = MakersTaskResults,
                DeviceCategories = DeviceCategoriesResults,
            };

            if (
                deviceResults.Count == 0
                && problemResults.Count == 0
                && usedAtResults.Count == 0
                && MakersTaskResults.Count == 0
                && DeviceCategoriesResults.Count == 0
            )
            {
                return NotFound("DeviceNotFound");
            }

            return Ok(results);
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

                //instance a new problem and save it
                var problem = new ProblemTreatWrapper
                {
                    Name = device.Name,
                    Description = device.Description,
                    DeviceId = deviceMac.DeviceId
                };
                deviceMac.Problems.Add(problem);
                await _db.Problems.AddAsync(problem);

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
                await _db.Devices.AddAsync(deviceMac);
                await _db.SaveChangesAsync();
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
