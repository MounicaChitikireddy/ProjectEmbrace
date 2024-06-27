using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using webapi.Data;
using webapi.Models;
using Action = webapi.Models.Action;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentDevicesController : ControllerBase
    {
        public class CurrentDeviceRequest
        {
            public int? DeviceTypeId { get; set; }

            public int? Count { get; set; }
            public string Grade { get; set; } = null!;
        }

        public class UpdatedDevices
        {
            public string Location { get; set; } = null!;
            public List<int> ContactIds { get; set; }
            public int CampaignId { get; set; }
            public bool? BatchApproved { get; set; }
            
            public string? AdditionalInfo { get; set; }
            
            public IEnumerable<CurrentDeviceRequest>? Additions { get; set; }
            public IEnumerable<CurrentDeviceRequest>? Deletions { get; set; }
        }


        public class GradeChangeDevice
        {
            public int DeviceTypeId { get; set; }
            public string OldGrade { get; set; }
            public string NewGrade { get; set; }
            public int Count { get; set; }
            public string Location { get; set; }
        }


        private readonly webapiContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReceiptService _receiptservice;

        public CurrentDevicesController(webapiContext context, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet("GetCurrentDevices")]
        // [Authorize]
        public ActionResult<object> GetCurrentDevices()
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            //Query joins CurrentDevices and DeviceTypes to get the sum the count of each unique device type
            var deviceQuery = from cD in _context.CurrentDevices
                join dT in _context.DeviceTypes on cD.DeviceType.DeviceTypeId equals dT.DeviceTypeId
                group cD by new { dT.Category, dT.Type, dT.Size, cD.Grade, cD.Location, dT.DeviceTypeId }
                into grouped
                orderby grouped.Key.Category, grouped.Key.Type, grouped.Key.Size, grouped.Key.Grade
                select new
                {
                    Category = grouped.Key.Category,
                    Type = grouped.Key.Type,
                    Size = grouped.Key.Size,
                    Grade = grouped.Key.Grade,
                    Location = grouped.Key.Location,
                    Count = grouped.Count(),
                    TypeID = grouped.Key.DeviceTypeId,
                };

            return Ok(deviceQuery);
        }


        // GET: api/CurrentDevices/5
        [HttpGet("GetById")]
        [Authorize]
        public async Task<ActionResult<CurrentDevice>> GetCurrentDeviceById(int id)
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            var currentDevice = await _context.CurrentDevices.FindAsync(id);

            if (currentDevice == null)
            {
                return NotFound("There are no devices in inventory by that Id.");
            }

            return currentDevice;
        }

        /// <summary>
        /// Gets a device type by primary key attributes
        /// </summary>
        /// <param name="category">Device Category</param>
        /// <param name="type">Device Type</param>
        /// <param name="size">Device Size</param>
        /// <param name="location">Device Location</param>
        /// <returns>Ok if found</returns>
        [HttpGet("GetByPK")]
        [Authorize]
        public async Task<ActionResult<CurrentDevice>> GetCurrentDeviceByPK(string category, string type, string size,
            string location)
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            var query = from c in _context.CurrentDevices
                join t in _context.DeviceTypes on c.DeviceType equals t
                where c.Location == location && t.CategoryNormalized == category.ToUpper()
                                             && t.TypeNormalized == type.ToUpper() && t.SizeNormalized == size.ToUpper()
                select c;
            if (query.Any())
            {
                return Ok(query.First());
            }
            else
            {
                return NotFound("Could not find any devices with these parameters.");
            }
        }
    
        [HttpPut("UpdateDeviceGrades")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDeviceGrades([FromBody] List<GradeChangeDevice> updates)
        {
            foreach (var update in updates)
            {
                var matchingDevices = (from c in _context.CurrentDevices.Include(x => x.DeviceType)
                    join t in _context.DeviceTypes on c.DeviceType.DeviceTypeId equals t.DeviceTypeId
                    where c.DeviceType.DeviceTypeId == update.DeviceTypeId && update.OldGrade == c.Grade &&
                          c.Location == update.Location
                    select c).ToList();
                if (matchingDevices.Count >= update.Count)
                {
                    for (int i = 0; i < update.Count; i++)
                    {
                        matchingDevices[i].Grade = update.NewGrade;
                    }
                }
                else
                {
                    return BadRequest("Trying to remove too many devices with DeviceTypeId:" + update.DeviceTypeId +
                                      " ensure the type exists or you have enough devices of the old grade");
                }
            }


            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Error saving changes");
            }
        }

        /// <summary>
        /// Updates current device counts and adds new devices types as needed
        /// </summary>
        /// <param name="updates">Wrapper Type that contains a list of additions and deletions</param>
        /// <returns>
        /// 200 Ok if update sucessful
        /// 400 BadRequest if it fails at anypoint
        /// </returns>
        [HttpPut("UpdateCurrentDevices")]
        [Authorize]
        public async Task<ActionResult> UpdateCurrentDevices([FromBody] UpdatedDevices updates)
        {
            var additions = updates.Additions!.ToList();
            var deletions = updates.Deletions!.ToList();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.FindByIdAsync(userId!).Result;

            

            
            var campaign = (from campaigns in _context.Campaigns
                where campaigns.CampaignId == updates.CampaignId
                select campaigns).FirstOrDefault();

            if (campaign == null)
            {
                return NotFound("Campaign with ID:" + updates.CampaignId + " could not be found ensure you have a " +
                                "valid campaign");
            }

            var contactsList = (from contacts in _context.Contacts
                where updates.ContactIds.Contains(contacts.ContactId)
                select contacts).ToList();
            
            
            if (contactsList.Count == 0 && updates.ContactIds.Count > 0)
            {
                return NotFound("Contact with IDs: " + string.Join(" ", updates.ContactIds)+
                                " could not be found ensure you have a valid contact");
            }

            if (user is null)
            {
                return Unauthorized();
            }

            ChangelogBatch batch = new()
            {
                CreatedOn = DateTime.Now,
                BatchCreatedBy = user,
                ReviewStatus = ReviewStatus.Pending,
                AdditionalInformation = updates.AdditionalInfo,
                Updates = new List<ChangelogBatchDevice>(),
                Campaign = campaign,
                Contacts = contactsList
            };

            if (await _userManager.IsInRoleAsync(user!, "Admin") && updates.BatchApproved != null &&
                updates.BatchApproved.Value)
            {
                return await AdminInventoryUpdate(additions, deletions, batch, user, updates.Location, campaign);
            }

            return await CreateBatch(additions, deletions, batch, updates.Location);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="additions"></param>
        /// <param name="deletions"></param>
        /// <param name="batch"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private async Task<ActionResult> CreateBatch(List<CurrentDeviceRequest>? additions,
            List<CurrentDeviceRequest>? deletions,
            ChangelogBatch batch, string location)
        {
            _context.ChangelogBatches.Add(batch);
            if (additions is { Count: > 0 })

            {
                batch.Action = Action.Addition;
                foreach (var addition in additions)
                {
                    if (addition.Count <= 0)
                    {
                        return BadRequest("Values for count that are less than 0 are not allowed.");
                    }
                    
                    var deviceTypeQuery = (from t in _context.DeviceTypes
                        where t.DeviceTypeId == addition.DeviceTypeId
                        select t).First();
                    if (deviceTypeQuery != null)
                    {
                        for (int i = 0; i < addition.Count; i++)
                        {
                            ChangelogBatchDevice history = new()
                            {
                                DeviceType = deviceTypeQuery,
                                Grade = addition.Grade,
                                Location = location
                            };
                            batch.Updates!.Add(history);
                        }
                    }
                }
            }
            else if (deletions is { Count: > 0 })
            {
                batch.Action = Action.Removal;
                foreach (var deletion in deletions)
                {
                    if (deletion.Count <= 0)
                    {
                        return BadRequest("Values for count that are less than 0 are not allowed.");
                    }
                    
                    var deviceTypeQuery = (from t in _context.DeviceTypes
                        where t.DeviceTypeId == deletion.DeviceTypeId
                        select t).First();
                    if (deviceTypeQuery != null)
                    {
                        for (int i = 0; i < deletion.Count; i++)
                        {
                            ChangelogBatchDevice history = new ChangelogBatchDevice()
                            {
                                DeviceType = deviceTypeQuery,
                                Grade = deletion.Grade,
                                Location = location
                            };
                            batch.Updates!.Add(history);
                        }
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Successfully Saved Changes.");
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, "Error saving changes.");
            }
        }

        /// <summary>
        /// Process an inventory update if you are an admin and want to approve a batch automatically
        /// </summary>
        /// <param name="additions">Items you will be adding</param>
        /// <param name="deletions">Items you will be removing</param>
        /// <param name="batch">Batch object created from UpdateCurrentDevices</param>
        /// <param name="user">User who created and will approve the batch</param>
        /// <param name="location">Storage location</param>
        /// <param name="campaign">Campaign the batch will be associated with</param>
        /// <returns>
        /// 200 Ok if successfully created and added to the inventory
        /// 400 BadRequest if trying to remove more devices than there are currently in the inventory
        /// 500 InternalServerError if the database encounters any errors saving changes
        /// </returns>
        private async Task<ActionResult> AdminInventoryUpdate(List<CurrentDeviceRequest>? additions,
            List<CurrentDeviceRequest>? deletions,
            ChangelogBatch batch, User user, string location, Campaign campaign)
        {
            //Set basic batch properties
            batch.BatchApprovedBy = user;
            batch.ApprovedOn = DateTime.Now;
            batch.ReviewStatus = ReviewStatus.Approved;
            if (additions is { Count: > 0 })
            {
                batch.Action = Action.Addition;
                foreach (var addition in additions)
                {
                    if (addition.Count <= 0)
                    {
                        return BadRequest("Values for count that are less than 0 are not allowed.");
                    }
                    //Check if device type in payload exists
                    var deviceTypeQuery = (from t in _context.DeviceTypes
                        where t.DeviceTypeId == addition.DeviceTypeId
                        select t).First();
                    if (deviceTypeQuery != null)
                    {
                        //Create and add number specified in addition.Count
                        for (var i = 0; i < addition.Count; i++)
                        {
                            var c = new CurrentDevice
                            {
                                DeviceType = deviceTypeQuery,
                                Grade = addition.Grade,
                                Location = location,
                                Campaign = campaign
                            };
                            _context.CurrentDevices.Add(c);

                            //Does the same for the batch
                            ChangelogBatchDevice tracker = new ChangelogBatchDevice
                            {
                                Batch = batch,
                                DeviceType = deviceTypeQuery,
                                Grade = addition.Grade,
                                Location = location,
                            };
                            _context.ChangelogBatchDevices.Add(tracker);
                        }
                    }
                    else
                    {
                        return BadRequest("No device types found with associated ID");
                    }
                }

                _context.ChangelogBatches.Add(batch);
            }
            else if (deletions is { Count: > 0 })
            {
                batch.Action = Action.Removal;
                foreach (var deletion in deletions)
                {
                    if (deletion.Count <= 0)
                    {
                        return BadRequest("Values for count that are less than 0 are not allowed.");
                    }
                    var matchingDevices = (from c in _context.CurrentDevices.Include(x => x.DeviceType)
                        join t in _context.DeviceTypes on c.DeviceType.DeviceTypeId equals t.DeviceTypeId
                        where c.DeviceType.DeviceTypeId == deletion.DeviceTypeId && deletion.Grade == c.Grade &&
                              c.Location == location
                        select c).ToList();
                    if (matchingDevices.Count() >= deletion.Count)
                    {
                        for (int i = 0; i < deletion.Count; i++)
                        {
                            _context.CurrentDevices.Remove(matchingDevices[i]);

                            ChangelogBatchDevice tracker = new ChangelogBatchDevice
                            {
                                Batch = batch,
                                DeviceType = matchingDevices.First().DeviceType,
                                Grade = deletion.Grade,
                                Location = location,
                            };
                            _context.ChangelogBatchDevices.Add(tracker);
                        }
                    }
                    else
                    {
                        return NotFound(
                            "Number of deletions exceed what is currently in the inventory for DeviceTypeID: " +
                            deletion.DeviceTypeId
                            + "\r\nProcess Cancelled.");
                    }
                }
                _context.ChangelogBatches.Add(batch);
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, "Error Saving Changes");
            }
        }

        // QUERIES FOR ANALYTIC VISUALIZATIONS

        // Returns a total count of all devices grouped by type
        [HttpGet("GetDeviceCount")]
        public ActionResult<object> GetDeviceCount()
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            var deviceQuery = from cD in _context.CurrentDevices
                join dT in _context.DeviceTypes on cD.DeviceType.DeviceTypeId equals dT.DeviceTypeId
                group cD by new { dT.Category, dT.Type, dT.Size, dT.DeviceTypeId }
                into grouped
                select new
                {
                    Category = grouped.Key.Category,
                    Count = grouped.Count(),
                    TypeID = grouped.Key.DeviceTypeId,
                    Type = grouped.Key.Type,
                    Size = grouped.Key.Size,
                };

            return Ok(deviceQuery);
        }

        [HttpGet("GetGroupedDeviceCount")]
        public ActionResult<object> GetGroupedDeviceCount()
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            var deviceQuery = from cD in _context.CurrentDevices
                join dT in _context.DeviceTypes on cD.DeviceType.DeviceTypeId equals dT.DeviceTypeId
                group cD by new { dT.Category }
                into grouped
                select new
                {
                    Category = grouped.Key.Category,
                    Count = grouped.Count(),
                };

            return Ok(deviceQuery);
        }

        // Returns a total count of devices grouped by grade
        [HttpGet("GetGradeCount")]
        public ActionResult<object> GetGradeCount()
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            var deviceQuery = from cD in _context.CurrentDevices
                join dT in _context.DeviceTypes on cD.DeviceType.DeviceTypeId equals dT.DeviceTypeId
                group cD by new { cD.Grade }
                into grouped
                select new
                {
                    Grade = grouped.Key.Grade,
                    Count = grouped.Count(),
                };

            return Ok(deviceQuery);
        }

        // Returns a total count of devices grouped by location
        [HttpGet("GetLocationCount")]
        public ActionResult<object> GetLocationCount()
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            var deviceQuery = from cD in _context.CurrentDevices
                join dT in _context.DeviceTypes on cD.DeviceType.DeviceTypeId equals dT.DeviceTypeId
                group cD by new { cD.Location }
                into grouped
                select new
                {
                    Location = grouped.Key.Location,
                    Count = grouped.Count(),
                };

            return Ok(deviceQuery);
        }

        // Returns a total count of all devices from a specified campaign
        [HttpGet("GetDeviceByCampaign/{campaignId}")]
        public ActionResult<object> GetDeviceByCampaign(int campaignId)
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            var deviceQuery = from cD in _context.CurrentDevices
                join dT in _context.DeviceTypes on cD.DeviceType.DeviceTypeId equals dT.DeviceTypeId
                where cD.Campaign.CampaignId == campaignId
                group cD by new { dT.Category, dT.Type, dT.Size, dT.DeviceTypeId }
                into grouped
                select new
                {
                    Category = grouped.Key.Category,
                    Count = grouped.Count(),
                    TypeID = grouped.Key.DeviceTypeId,
                    Type = grouped.Key.Type,
                    Size = grouped.Key.Size,
                };

            return Ok(deviceQuery);
        }

        [HttpGet("GetGroupedDeviceByCampaign/{campaignId}")]
        public ActionResult<object> GetGroupedDeviceByCampaign(int campaignId)
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            var deviceQuery = from cD in _context.CurrentDevices
                join dT in _context.DeviceTypes on cD.DeviceType.DeviceTypeId equals dT.DeviceTypeId
                where cD.Campaign.CampaignId == campaignId
                group cD by new { dT.Category }
                into grouped
                select new
                {
                    Category = grouped.Key.Category,
                    Count = grouped.Count(),
                };

            return Ok(deviceQuery);
        }

        // Returns a total count of devices grouped by grade
        [HttpGet("GetGradeCountByCampaign/{campaignId}")]
        public ActionResult<object> GetGradeCountByCampaign(int campaignId)
        {
            if (_context.CurrentDevices == null)
            {
                return NotFound("Current device table is currently empty.");
            }

            var deviceQuery = from cD in _context.CurrentDevices
                join dT in _context.DeviceTypes on cD.DeviceType.DeviceTypeId equals dT.DeviceTypeId
                where cD.Campaign.CampaignId == campaignId
                group cD by new { cD.Grade }
                into grouped
                select new
                {
                    Grade = grouped.Key.Grade,
                    Count = grouped.Count(),
                };

            return Ok(deviceQuery);
        }
    }
}