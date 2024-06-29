using DeviceContext;
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

        public CateriesActions(DeviceDb db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("/GetAllModels")]
        public async Task<ActionResult<Maker>> GetAllModels()
        {
            try
            {
                var allMakers = await _db.Makers.ToListAsync();
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
                if (await _db.Makers.FindAsync(category.MakerId) == null)
                {
                    return BadRequest("Owner Is a Not Valid Id");
                }

                var categoryCleaned = new DeviceCategory
                {
                    DeviceCategoryName = category.DeviceCategoryName,
                    MakerId = category.MakerId,
                    OperationMode = category.OperationMode,
                    DeviceCategoryId = Guid.NewGuid().ToString()
                };
                await _db.DeviceCategories.AddAsync(categoryCleaned);
                await _db.SaveChangesAsync();

                var makerAdd = new Maker { };
                makerAdd.AddDeviceCategory(category);
                await _db.SaveChangesAsync();

                return Ok(categoryCleaned);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Maker>> GetMakerById(string id)
        {
            var maker = await _db.Makers.FindAsync(id);
            if (maker == null)
            {
                return NotFound("DeviceNotFound");
            }
            return maker;
        }
    }
}
