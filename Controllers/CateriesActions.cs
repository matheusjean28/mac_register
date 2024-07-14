using DeviceContext;
using MacSave.Funcs;
using MacSave.Models.Categories;
using MacSave.Models.Categories.Models_of_Devices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MacSave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CateriesActions : ControllerBase
    {
        private readonly DeviceDb _db;
        private readonly RegexService _regexService;

        public CateriesActions(DeviceDb db, RegexService regexService)
        {
            _regexService = regexService;
            _db = db;
        }

        [HttpGet]
        [Route("/GetAllMakers")]
        public async Task<ActionResult<Maker>> GetAllMakers()
        {
            try
            {
                var allMakers = await _db
                    .Makers.Include(m => m.DeviceCategories)
                    .Select(m => new
                    {
                        m.MakerId,
                        m.MakerName,
                        DeviceCategories = m.DeviceCategories.Select(m => new
                        {
                            m.DeviceCategoryId,
                            m.DeviceCategoryName,
                            m.OperationMode
                        }),
                    })
                    .ToListAsync();
                return Ok(allMakers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("/GetAllCategories")]
        public async Task<ActionResult<Maker>> GetAllCategories()
        {
            try
            {
                var allCategories = await _db.DeviceCategories.ToListAsync();
                return Ok(allCategories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("/CreateMaker")]
        public async Task<ActionResult<Maker>> CreateMaker([FromBody] Maker maker)
        {
            try
            {
                if (maker == null)
                {
                    return BadRequest("Maker Cannot be empyt");
                }
                //create a func that recive this data and query them
                if (await _db.Makers.AnyAsync(x => x.MakerName == maker.MakerName))
                {
                    return BadRequest("Maker Already Exist");
                }

                var makerCleaned = new Maker
                {
                    MakerName = maker.MakerName,
                    MakerId = Guid.NewGuid().ToString(),
                };

                await _db.Makers.AddAsync(makerCleaned);
                await _db.SaveChangesAsync();
                return Ok(makerCleaned);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("/CreateCategory")]
        public async Task<ActionResult<DeviceCategory>> CreateCategory(
            [FromBody] DeviceCategory category
        )
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Category cannot be empty");
                }

                if (
                    await _db.DeviceCategories.AnyAsync(x =>
                        x.DeviceCategoryName == category.DeviceCategoryName
                    )
                )
                {
                    return BadRequest("Category Already Exist");
                }

                var maker = await _db.Makers.FindAsync(category.MakerId);
                if (maker == null)
                {
                    return BadRequest("Owner Is a Not Valid Id");
                }

                var categoryCleaned = new DeviceCategory
                {
                    DeviceCategoryName = category.DeviceCategoryName,
                    MakerId = maker.MakerId,
                    OperationMode = category.OperationMode,
                    DeviceCategoryId = Guid.NewGuid().ToString()
                };

                await _db.DeviceCategories.AddAsync(categoryCleaned);
                maker.AddDeviceCategory(categoryCleaned);
                await _db.SaveChangesAsync();

                return Ok(categoryCleaned);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/FindMaker/{paramDirty}")]
        public async Task<ActionResult<object>> GetMakerById(string paramDirty)
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
    }
}
