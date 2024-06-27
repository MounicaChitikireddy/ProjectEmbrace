using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTypesController : ControllerBase
    {
        private readonly webapiContext _context;

        public DeviceTypesController(webapiContext context)
        {
            _context = context;
        }

        // GET: api/DeviceTypes
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<DeviceType>>> GetDeviceTypes()
        {
            if (_context.DeviceTypes == null)
            {
                return NotFound();
            }

            var deviceTypesQuery = from types in _context.DeviceTypes
                orderby types.Category, types.Type, types.Size
                select new
                {
                    types.DeviceTypeId,
                    types.Category,
                    types.Type,
                    types.Size,
                    types.Description,
                };

            return Ok(await deviceTypesQuery.ToArrayAsync());
        }

        // GET: api/DeviceTypes/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<DeviceType>> GetDeviceType(int id)
        {
            if (_context.DeviceTypes == null)
            {
                return NotFound();
            }
            var deviceType = (from types in _context.DeviceTypes
                             where types.DeviceTypeId == id
                             select types).FirstOrDefault();

            if (deviceType == null)
            {
                return NotFound();
            }

            return Ok(deviceType);
        }

        // PUT: api/DeviceTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditDeviceType")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutDeviceType(DeviceTypeRequest deviceType)
        {

            var deviceTypeQuery = from types in _context.DeviceTypes
                                  where types.DeviceTypeId == deviceType.DeviceTypeId
                                  select types;

            if (deviceTypeQuery.Any())
            {
                var deviceTypeResult = deviceTypeQuery.First();


                if (deviceType.Category != null)
                {
                    deviceTypeResult.Category = deviceType.Category;
                    deviceTypeResult.CategoryNormalized = deviceType.Category.ToUpper();
                }

                if (deviceType.Type != null)
                {
                    deviceTypeResult.Type = deviceType.Type;
                    deviceTypeResult.TypeNormalized = deviceType.Type.ToUpper();
                }

                if (deviceType.Size != null)
                {
                    deviceTypeResult.Size = deviceType.Size;
                    deviceTypeResult.SizeNormalized = deviceType.Size.ToUpper();
                }

                deviceTypeResult.Description = deviceType.Description;

                _context.DeviceTypes.Update(deviceTypeResult);
                try
                {
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest("No Device Type Exists with given ID");

        }

        // POST: api/DeviceTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateDeviceType")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeviceType>> PostDeviceType(DeviceTypeRequest deviceType)
        {


            DeviceType d = new DeviceType
            {
                Category = deviceType.Category,
                CategoryNormalized = deviceType.Category.ToUpper(),
                Type = deviceType.Type,
                TypeNormalized = deviceType.Type.ToUpper(),
                Size = deviceType.Size,
                SizeNormalized = deviceType.Size.ToUpper(),
                Description = deviceType.Description,
            };
            _context.DeviceTypes.Add(d);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddManyDeviceType")]
        public async Task<IActionResult> CreateManyDeviceType([FromBody] List<DeviceTypeRequest> additions)
        {
            foreach (var addition in additions)
            {
                DeviceType d = new DeviceType
                {
                    Category = addition.Category,
                    CategoryNormalized = addition.Category.ToUpper(),
                    Type = addition.Type,
                    TypeNormalized = addition.Type.ToUpper(),
                    Size = addition.Size,
                    SizeNormalized = addition.Size.ToUpper(),
                    Description = addition.Description,
                };
                _context.DeviceTypes.Add(d);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpDelete("DeleteManyDeviceType")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteManyDeviceType([FromBody] int[] deletions)
        {
            foreach (var deletion in deletions)
            {
                //Include NavProperties to check for foreign key references
                var deviceType = (from types in _context.DeviceTypes
                        .Include(x => x.CurrentDeviceHistories)
                        .Include(x => x.CurrentDevices)
                                  where types.DeviceTypeId == deletion
                                  select types).FirstOrDefault();

                if (deviceType == null)
                    return BadRequest("No DeviceTypeFound with ID:" + deletion);

                if (!HasDependencies(deviceType))
                {
                    _context.DeviceTypes.Remove(deviceType);
                }
                else
                {
                    return BadRequest("Trying to remove DeviceType with ID: " + deletion +
                                      ".\nThis DeviceType has associated data and cannot be deleted");
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // DELETE: api/DeviceTypes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDeviceType(int id)
        {
            if (_context.DeviceTypes == null)
            {
                return NotFound();
            }
            var deviceType = await _context.DeviceTypes.FindAsync(id);
            if (deviceType == null)
            {
                return NotFound();
            }

            _context.DeviceTypes.Remove(deviceType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceTypeExists(int id)
        {
            return (_context.DeviceTypes?.Any(e => e.DeviceTypeId == id)).GetValueOrDefault();
        }


        private bool HasDependencies(DeviceType? type)
        {
            if (type.CurrentDevices.Count != 0 || type.CurrentDevices.Count != 0)
            {
                return true;
            }
            return false;


        }
    }
}
