using System.Linq;
using DeviceContext;
using DeviceModel;
using MacSave.Funcs;
using MacSave.Models.Categories.Models_of_Devices;
using MacSave.Models.SinalHistory;
using mac_register.Models.FullDeviceCreate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;

namespace Controller.DeviceActionsController
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceActionsController : ControllerBase
    {
        private readonly ILogger<DeviceActionsController> _logger;
        private readonly DeviceDb _db;
        private readonly RegexService _regexService;

        public DeviceActionsController(
            DeviceDb db,
            RegexService regexService,
            ILogger<DeviceActionsController> logger
        )
        {
            _regexService = regexService;
            _db = db;
            _regexService = regexService;
            _logger = logger;
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
                            p.ProblemName,
                            p.ProblemDescription,
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
                .Problems.Where(p => p.ProblemName.Contains(param))
                .Where(p => p.ProblemDescription.Contains(param))
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

        [HttpPost("/create_new_Device")]
        public async Task<ActionResult<object>> CreateNewDevice(
            [FromBody] FullDeviceCreate deviceDity
        )
        {
            try
            {
                if (deviceDity == null)
                {
                    return BadRequest("device cannot be null!");
                }

                _logger.LogInformation(
                    "---------------------------\n\nDevice Raw: \nCategory_Id_Device={ Category_Id_Device }, \nDeviceCategoryName= {DeviceCategoryName}, \nMac={ Mac }, \nRemoteAcess={ RemoteAcess }, \nProblemName={ ProblemName }, \nProblemDescription={ ProblemDescription }, \nUserName={ UserName }, \nSinalRX={ SinalRX }, \nSinalTX={ SinalTX }",
                    deviceDity.Category_Id_Device,
                    deviceDity.DeviceCategoryName,
                    deviceDity.Mac,
                    deviceDity.RemoteAcess,
                    deviceDity.ProblemName,
                    deviceDity.ProblemDescription,
                    deviceDity.UserName,
                    deviceDity.SinalRX,
                    deviceDity.SinalTX
                );

                var deviceCategory = await _db.DeviceCategories.FirstOrDefaultAsync(dc =>
                    dc.DeviceCategoryId == deviceDity.Category_Id_Device
                );
                //check if device is null and loggin it
                if (deviceCategory == null)
                {
                    return BadRequest("category not found!");
                }
                ;
                _logger.LogInformation(
                    "\n\nloggin device category info:\nDeviceCategoryName = {},\nDeviceCategoryID = {}\n\n",
                    deviceCategory.DeviceCategoryName,
                    deviceCategory.DeviceCategoryId
                );

                //instance a new device create and pass device category name him
                var MacDevice = new DeviceCreate
                {
                    Mac = deviceDity.Mac,
                    Model = deviceDity.Category_Id_Device,
                    RemoteAcess = deviceDity.RemoteAcess,
                    DeviceCategoryId = deviceDity.DeviceCategoryName
                };
                _logger.LogInformation(
                    " \nchecking status of device create\n\nMacDevice Mac = {}, \nMacDevice model = {}\n, MacDevice Remote Acess = {}, \nMacDevice.DeviceCategoryId = {}\n\n\n\n---------------------------\n\n\n",
                    MacDevice.Mac,
                    MacDevice.Model,
                    MacDevice.RemoteAcess,
                    MacDevice.DeviceCategoryId
                );
                await _db.Devices.AddAsync(MacDevice);
                await _db.SaveChangesAsync();

                deviceCategory.AddDeviceToCategory(MacDevice);
                await _db.SaveChangesAsync();

                return Ok(deviceDity);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar novo dispositivo");
                return BadRequest(ex.Message);
            }
        }

        // [HttpPost("CreateDevice")]
        // //return to FullDeviceCreate
        // public async Task<ActionResult<object>> CreateDevice(
        //     [FromBody] FullDeviceCreate fullDeviceDiry
        // )
        // {
        //     if (fullDeviceDiry == null)
        //     {
        //         return BadRequest("Device cannot be null");
        //     }

        //     var device = new FullDeviceCreate
        //     {
        //         Model = _regexService.SanitizeInput(fullDeviceDiry.Model),
        //         Mac = _regexService.SanitizeInput(fullDeviceDiry.Mac),
        //         RemoteAcess = fullDeviceDiry.RemoteAcess,
        //         ProblemName = _regexService.SanitizeInput(fullDeviceDiry.ProblemName),
        //         ProblemDescription = _regexService.SanitizeInput(fullDeviceDiry.ProblemDescription),
        //         UserName = _regexService.SanitizeInput(fullDeviceDiry.UserName),
        //         SinalRX = _regexService.SanitizeInput(fullDeviceDiry.SinalRX),
        //         SinalTX = _regexService.SanitizeInput(fullDeviceDiry.SinalTX),
        //     };

        //     try
        //     {
        //         //get maker id to device category
        //         var deviceCategory = await _db.DeviceCategories.FirstOrDefaultAsync(dc =>
        //             dc.DeviceCategoryName == device.Model
        //         );

        //         //  !!! FIND DDIRY PARAM
        //         //check if is null

        //         if (deviceCategory == null)
        //         {
        //             return BadRequest("Device Name Not Found!");
        //         }

        //         //creating a device and save it
        //         var deviceMac = new DeviceCreate
        //         {
        //             Model = device.Model,
        //             Mac = device.Mac,
        //             RemoteAcess = device.RemoteAcess,
        //             DeviceCategoryName = deviceCategory.DeviceCategoryName,
        //         };
        //         await _db.Devices.AddAsync(deviceMac);
        //         await _db.SaveChangesAsync();

        //         deviceCategory.AddDeviceToCategory(deviceMac);
        //         await _db.SaveChangesAsync();

        //         //instance a new problem and save it
        //         var problem = new ProblemTreatWrapper
        //         {
        //             ProblemName = device.ProblemName,
        //             ProblemDescription = device.ProblemDescription,
        //             DeviceId = deviceMac.DeviceId
        //         };
        //         deviceMac.Problems.Add(problem);
        //         await _db.Problems.AddAsync(problem);

        //         var sinalHistory = new SinalHistory
        //         {
        //             SinalRX = device.SinalRX,
        //             SinalTX = device.SinalTX,
        //             DeviceId = deviceMac.DeviceId,
        //         };
        //         deviceMac.AddSinal(sinalHistory);

        //         //instance a new user and save it
        //         var usedAt = new UsedAtWrapper
        //         {
        //             Name = device.UserName,
        //             DeviceId = deviceMac.DeviceId
        //         };
        //         deviceMac.UsedAtClients.Add(usedAt);

        //         var responde = new
        //         {
        //             deviceMac,
        //             usedAt,
        //             problem
        //         };

        //         return Ok(responde);
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(
        //             $"Failed to create device: {ex.Message}, {ex.StackTrace}, {ex.GetBaseException()}"
        //         );
        //     }
        // }
    }
}
