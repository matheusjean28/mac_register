using DeviceContext;
using MacSave.Funcs;
using MacSave.Models.Categories;
using MacSave.Models.Categories.Models_of_Devices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MacSave.Models.FromBodyParamsDarty;
using Microsoft.AspNetCore.Identity;


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
                var allCategories = await _db.DeviceCategories.Include(d => d.Devices)
                .Select(d => new
                {
                    d.DeviceCategoryId,
                    d.DeviceCategoryName,
                    d.OperationMode,
                    Devices = d.Devices.Select(d => new
                    {
                        d.DeviceId,
                        d.Mac,
                        d.Model,
                        d.RemoteAcess
                    })
                })
                .ToListAsync();
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
            [FromBody] CreateCategoryDarty category
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

        [HttpDelete]
        [Route("/DeleteCategory")]
        public async Task<ActionResult> DeleteCategory(string CategoryIdDirty)
        {
            try
            {//looking for user permission level, and check if he can delete category before do task
                var CategoryId = _regexService.SanitizeInput(CategoryIdDirty);
                        
                var category = await _db.DeviceCategories
                .Where(d => d.DeviceCategoryId == CategoryId).FirstOrDefaultAsync();

                if (category == null)
                {
                    return BadRequest("Category not Found!");
                }

                _db.DeviceCategories.Remove(category);
                await _db.SaveChangesAsync();

                object responseOK = new { Sucess = $"DeviceID: {CategoryId}, deleted with sucess!$" };
                return Ok(responseOK);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
