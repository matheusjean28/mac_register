using DeviceContext;
using mac_register.Models.FullDeviceCreate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//database tasks import and regex service
using MacSave.Funcs;
using MacSave.Funcs.Database;


namespace Controller.DeviceActionsController
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceActionsController : ControllerBase
    {
        private readonly ILogger<DeviceActionsController> _logger;
        private readonly DeviceDb _db;
        private readonly RegexService _regexService;
        private readonly DatabaseTasks _databaseTasks;
        readonly bool _logStatus = true;


        public DeviceActionsController(
        DeviceDb db,
        RegexService regexService,
        ILogger<DeviceActionsController> logger,
        DatabaseTasks databaseTasks)
        {
            _regexService = regexService;
            _db = db;
            _logger = logger;
            _databaseTasks = databaseTasks;
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


        //this action get a param, clean it and fetch for that at database
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

            var usedAtTask = _db.UsedAtClient.Where(u => u.Name.Contains(param))
                .ToListAsync();

            var MakersTask = _db.Makers.Where(h => h.MakerName.Contains(param))
                .ToListAsync();

            var DeviceCategoriesTaks = _db.DeviceCategories
            .Where(c => c.DeviceCategoryName.Contains(param))
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


        [HttpPost("/CreateNewDevice")]
        public async Task<ActionResult<object>> CreateNewDevice(
            [FromBody] FullDeviceCreate deviceDity
        )
        {
            try
            {
                if (deviceDity == null)
                {
                    if (_logStatus)
                    {
                        _logger.LogWarning("Received a null FullDeviceCreate object.");
                    }
                    return BadRequest("device cannot be null!");
                }

                var deviceCategory = await _db.DeviceCategories.FirstOrDefaultAsync(dc =>
                    dc.DeviceCategoryId == deviceDity.Category_Id_Device
                );
                //check if device is null and loggin it
                if (deviceCategory == null)
                {
                    return BadRequest("category not found!");
                };

                //create a new instance of device DI Database Tasroks
                var DatabaseTaskNewDevice = _databaseTasks.CreateDevice(deviceDity);
                await _db.Devices.AddAsync(DatabaseTaskNewDevice);
                if (_logStatus)
                {
                    _logger.LogInformation("\n\n\nSaving new instance of device at database\n {}", deviceCategory.DeviceCategoryId);
                }

                await _databaseTasks.CreateRelatedEntities(deviceDity, DatabaseTaskNewDevice);

                return Ok(DatabaseTaskNewDevice);
            }
            catch (InvalidOperationException ex)
            {
                if (_logStatus)
                {
                    _logger.LogWarning(ex, "\n\n\nInvalid operation during device creation: {Message}", ex.Message);
                }
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (_logStatus)
                {
                    _logger.LogError(ex, "\n\n\nError occurred while creating a new device");
                }
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete]
        [Route("/DeleteDevice")]
        public async Task<ActionResult> DeleteDevice(string deviceId)
        {
            try
            {
                //next step, check if user that call delete have permission to delete
                //if(user.permissionLeve == adm )...(action);

                //check if DeviceID is valid
                var device = await _db.Devices.Where(d => d.DeviceId == deviceId).FirstOrDefaultAsync();
                if (device == null)
                {
                    return BadRequest("DeviceID not found!");
                }

                _db.Devices.Remove(device);
                object responseOK =new  { Sucess= $"DeviceID: {deviceId}, deleted with sucess!$"};
            return Ok(responseOK);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
