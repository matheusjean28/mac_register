using System.Linq;
using DeviceContext;
using DeviceModel;
using MacSave.Models.Categories.Models_of_Devices;
using MacSave.Models.SinalHistory;
using mac_register.Models.FullDeviceCreate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;

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


        public DeviceActionsController(
    DeviceDb db,
    RegexService regexService,
    ILogger<DeviceActionsController> logger,
    DatabaseTasks databaseTasks
)
        {
            _regexService = regexService;
            _db = db;
            _logger = logger;
            _databaseTasks = databaseTasks;  // Corrigido: _databseTasks -> _databaseTasks
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

                var deviceCategory = await _db.DeviceCategories.FirstOrDefaultAsync(dc =>
                    dc.DeviceCategoryId == deviceDity.Category_Id_Device
                );
                //check if device is null and loggin it
                if (deviceCategory == null)
                {
                    return BadRequest("category not found!");
                };

                //instance a new device create and pass device category name him
                var MacDevice = new DeviceCreate
                {
                    DeviceId = Guid.NewGuid().ToString(),
                    Mac = deviceDity.Mac,
                    Model = deviceDity.Category_Id_Device,
                    RemoteAcess = deviceDity.RemoteAcess,
                    DeviceCategoryId = deviceDity.Category_Id_Device
                };
                await _db.Devices.AddAsync(MacDevice);

                _logger.LogInformation($"\n\n\nMacDevice Id = {MacDevice.DeviceId}\n\n\n");

                //add that device to related category and save it
                deviceCategory.AddDeviceToCategory(MacDevice);
                _logger.LogInformation("\n\n\nok until here = add device to category\n\n\n");

                await _db.SaveChangesAsync();

                _logger.LogInformation("\n\n\nok until here = save device \n\n\n");

                //generate a new problem and save it 
                var problem = new ProblemTreatWrapper
                {
                    ProblemName = deviceDity.ProblemName,
                    ProblemDescription = deviceDity.ProblemDescription,
                    DeviceId = MacDevice.DeviceId,
                };
                _logger.LogInformation("\n\n\nok until here = generate problem\n\n\n");

                MacDevice.AddProblems(problem);
                _logger.LogInformation("\n\n\nok until here = add  problem\n\n\n");

                //too darty, optimize before it works... 
                try
                {
                    await _db.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is ProblemTreatWrapper)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // TODO: decide which value should be written to database
                                proposedValues[property] = proposedValue;

                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
                _logger.LogInformation("\n\n\nok until here = save problem at database\n\n\n");


                //generate a new used at wrapper and save it
                var userAtWrapper = new UsedAtWrapper
                {
                    Name = deviceDity.UserName,
                    DeviceId = MacDevice.DeviceId
                };
                MacDevice.AddClients(userAtWrapper);
                await _db.SaveChangesAsync();

                //generate a new signal history and save ir
                var SignalHistory = new SinalHistory
                {
                    SinalRX = deviceDity.SinalRX,
                    SinalTX = deviceDity.SinalTX,
                    DeviceId = MacDevice.DeviceId
                };
                MacDevice.AddSinal(SignalHistory);
                await _db.SaveChangesAsync();

                return Ok(MacDevice);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error at create new device");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/create_new_Device_with_DI")]
        public async Task<ActionResult<object>> CreateNewDeviceWithDI(
            [FromBody] FullDeviceCreate deviceDity
        )
        {
            try
            {
                if (deviceDity == null)
                {
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
                _logger.LogInformation("${}",DatabaseTaskNewDevice);

                //instance a new device create and pass device category name him
                var MacDevice = new DeviceCreate
                {
                    DeviceId = Guid.NewGuid().ToString(),               
                    Mac = deviceDity.Mac,
                    Model = deviceDity.Category_Id_Device,
                    RemoteAcess = deviceDity.RemoteAcess,
                    DeviceCategoryId = deviceDity.Category_Id_Device
                };
                await _db.Devices.AddAsync(MacDevice);

                _logger.LogInformation($"\n\n\nMacDevice Id = {MacDevice.DeviceId}\n\n\n");

                //add that device to related category and save it
                deviceCategory.AddDeviceToCategory(MacDevice);
                _logger.LogInformation("\n\n\nok until here = add device to category\n\n\n");

                await _db.SaveChangesAsync();

                _logger.LogInformation("\n\n\nok until here = save device \n\n\n");

                //generate a new problem and save it 
                var problem = new ProblemTreatWrapper
                {
                    ProblemName = deviceDity.ProblemName,
                    ProblemDescription = deviceDity.ProblemDescription,
                    DeviceId = MacDevice.DeviceId,
                };
                _logger.LogInformation("\n\n\nok until here = generate problem\n\n\n");

                MacDevice.AddProblems(problem);
                _logger.LogInformation("\n\n\nok until here = add  problem\n\n\n");

                //too darty, optimize before it works... 
                try
                {
                    await _db.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is ProblemTreatWrapper)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // TODO: decide which value should be written to database
                                proposedValues[property] = proposedValue;

                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
                _logger.LogInformation("\n\n\nok until here = save problem at database\n\n\n");


                //generate a new used at wrapper and save it
                var userAtWrapper = new UsedAtWrapper
                {
                    Name = deviceDity.UserName,
                    DeviceId = MacDevice.DeviceId
                };
                MacDevice.AddClients(userAtWrapper);
                await _db.SaveChangesAsync();

                //generate a new signal history and save ir
                var SignalHistory = new SinalHistory
                {
                    SinalRX = deviceDity.SinalRX,
                    SinalTX = deviceDity.SinalTX,
                    DeviceId = MacDevice.DeviceId
                };
                MacDevice.AddSinal(SignalHistory);
                await _db.SaveChangesAsync();

                return Ok(MacDevice);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error at create new device");
                return BadRequest(ex.Message);
            }
        }
    }
}
