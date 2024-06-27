using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using webapi.Data;
using webapi.Interfaces;
using webapi.Models;
using webapi.Services;
using Action = webapi.Models.Action;


namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChangelogBatchesController : ControllerBase
{
    private readonly webapiContext _context;
    private readonly ApplicationUserManager _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChangelogBatchesController(webapiContext context, ApplicationUserManager userManager,
        IHttpContextAccessor httpContextAccessor, IReceiptService receiptService)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    //TODO: adjust query to return action
    //TODO: REMOVE COUNT FIELD
    //TODO: HANDLE ERROR HANDLING

    public class BatchDeviceResponse
    {
        public int DeviceTypeId { get; set; }
        public string Grade { get; set; }
        public int Count { get; set; }
    }

    private class GroupedDevice
    {
        public DeviceType DeviceType { get; set; }
        public string Grade { get; set; }
        public string Location { get; set; }
        public int Count { get; set; }
    }

    public class BatchResponse
    {
        public int Id { get; set; }

        public List<int> ContactIds { get; set; }

        public int CampaignId { get; set; }

        public ReviewStatus ReviewStatus { get; set; }

        public Action Action { get; set; }

        public string Location { get; set; }


        
        public string? AdditionalInfo { get; set; }
        
        public List<BatchDeviceResponse>? Updates { get; set; }
    }

    [Authorize(Roles = "Admin,Reviewer")]
    [HttpGet("GetBatches")]
    public IActionResult GetBatches()
    {
        var query = from b in _context.ChangelogBatches.Include(x => x.Contacts)
            orderby b.CreatedOn descending
            select new
            {
                b.Id,
                Action = b.Action.ToString(),
                b.Contacts,
                b.Campaign.CampaignId,
                b.CreatedOn,
                CreatedBy = b.BatchCreatedBy.FullName,
                ApprovedBy = b.BatchApprovedBy.FullName,
                b.ApprovedOn,
                AdditionalInfo = b.AdditionalInformation,
                ReviewStatus = b.ReviewStatus.ToString()
            };
        return Ok(query.ToList());
    }

    [Authorize(Roles = "Admin,Reviewer")]
    [HttpGet("GetBatchDetails")]
    public async Task<IActionResult> GetBatchDetails(int batchId)
    {
        var query = from cD in _context.ChangelogBatchDevices
            join dT in _context.DeviceTypes on cD.DeviceType.DeviceTypeId equals dT.DeviceTypeId
            where cD.Batch.Id == batchId
            group cD by new { dT.Category, dT.Type, dT.Size, cD.Grade, cD.Location, dT.DeviceTypeId }
            into grouped
            select new
            {
                grouped.Key.Category,
                grouped.Key.Type,
                grouped.Key.Size,
                grouped.Key.Grade,
                grouped.Key.Location,
                TypeID = grouped.Key.DeviceTypeId,
                Count = grouped.Count()
            };


        if (await query.AnyAsync())
            return Ok(await query.ToListAsync());

        return NotFound("No devices found with associated batch ID");
    }

    [HttpPut("EditBatch")]
    [Authorize(Roles = "Admin,Reviewer")]
    public ActionResult EditBatch(BatchResponse batchResponse)
    {
        var batch = (from batches in _context.ChangelogBatches
                .Include(x => x.Updates)
                .Include(x=>x.Contacts)
                     where batches.Id == batchResponse.Id
                     select batches).FirstOrDefault();

        if (batch == null)
        {
            return NotFound("Batch with ID: " + batchResponse.Id + " could not be found");
        }

        var contactsQuery = (from contacts in _context.Contacts
            where batchResponse.ContactIds.Contains(contacts.ContactId)
            select contacts).ToList();
        if (contactsQuery.Count == 0 && batchResponse.ContactIds.Count > 0)
        {
            return NotFound("No contact found with IDs: " + batchResponse.ContactIds);
        }

        var campaign = (from campaigns in _context.Campaigns
            where batchResponse.CampaignId == campaigns.CampaignId
            select campaigns).FirstOrDefault();
        if (campaign == null)
        {
            return NotFound("No campaign found with ID: " + batchResponse.CampaignId);
        }
        
        
        batch.Contacts?.Clear();
        batch.Contacts = contactsQuery;
        batch.Campaign = campaign;
        
        batch.AdditionalInformation = batchResponse.AdditionalInfo;        
        
        batch.ReviewStatus = batchResponse.ReviewStatus;
        batch.Action = batchResponse.Action;


        return ProcessBatchEdits(batchResponse.Updates, batch, batchResponse.Location);
    }


    private ActionResult ProcessBatchEdits(List<BatchDeviceResponse>? updates, ChangelogBatch batch, string location)
    {
        batch.Updates?.Clear();
        if (updates != null)
        {
            foreach (var update in updates)
            {
                var deviceType = (from deviceTypes in _context.DeviceTypes
                    where deviceTypes.DeviceTypeId == update.DeviceTypeId
                    select deviceTypes).FirstOrDefault();
                if (deviceType == null)
                    return NotFound("Device type with ID:" + update.DeviceTypeId +
                                    " was not found ensure you are using an existing device type");
                for (int i = 0; i < update.Count; i++)
                {
                    ChangelogBatchDevice device = new ChangelogBatchDevice()
                    {
                        DeviceType = deviceType,
                        Batch = batch,
                        Grade = update.Grade,
                        Location = location
                    };
                    batch.Updates?.Add(device);
                }
            }
        }

        try
        {
            _context.SaveChanges();
            return Ok();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Error updating the database");
        }
    }

    [HttpDelete("DeleteBatch")]
    [Authorize(Roles = "Admin,Reviewer")]
    public ActionResult DeleteBatch(int batchId)
    {
        var batch = (from batches in _context.ChangelogBatches
            where batches.Id == batchId
            select batches).FirstOrDefault();
        if (batch == null)
        {
            return BadRequest("Batch with ID: " + batchId + " could not be found");
        }

        _context.Remove(batch);
        try
        {
            _context.SaveChanges();
            return Ok();
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Error updating the database");
        }
    }

    [HttpPut("SetBatchStatus")]
    [Authorize(Roles = "Admin,Reviewer")]
    public async Task<ActionResult> SetBatchStatus(int batchId, ReviewStatus status)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _userManager.FindByIdAsync(userId!).Result;
        if (user is null)
        {
            return Unauthorized();
        }

        var batch = await (from batches in _context.ChangelogBatches
                .Include(x => x.Campaign)
                .Include(x => x.Updates)
                .ThenInclude(x => x.DeviceType)
            where batches.Id == batchId
            select batches).FirstOrDefaultAsync();
        if (batch == null)
        {
            return NotFound("No batch corresponds to the given ID");
        }

        var groupedUpdates = batch.Updates
            .GroupBy(x => new { x.DeviceType, x.Grade, x.Location })
            .Select(x => new GroupedDevice
                { Location = x.Key.Location, DeviceType = x.Key.DeviceType, Grade = x.Key.Grade, Count = x.Count() })
            .ToList();


        if (batch.ReviewStatus != ReviewStatus.Pending)
        {
            return Forbid("Batch Status has already been set");
        }

        if (status == ReviewStatus.Approved)
        {
            foreach (var update in groupedUpdates)
            {
                if (ProcessBatchUpdates(update, batch.Action, batch.Campaign) == false)
                {
                    return BadRequest("Not enough devices in the inventory for removal");
                }
            }

            batch.ReviewStatus = ReviewStatus.Approved;
            batch.BatchApprovedBy = user;
            batch.ApprovedOn = DateTime.Now;
        }
        else if (status == ReviewStatus.Denied)
        {
            batch.ReviewStatus = ReviewStatus.Denied;
            batch.BatchApprovedBy = user;
            batch.ApprovedOn = DateTime.Now;
        }
        try
        {
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Process the entry of one batch
    /// Prepares to add it to the database if it is addition
    /// Looks for a device to remove from the database and prepares to remove it if found
    /// </summary>
    /// <param name="groupedDevices">Changelog entry to be processed</param>
    /// <param name="action">Specifies whether it is an addition or removal</param>
    /// <param name="campaign">campaign the batch belongs to</param>
    /// <returns></returns>
    private bool ProcessBatchUpdates(GroupedDevice device, Action action, Campaign campaign)
    {
        if (action == Action.Addition)
        {
            for (int i = 0; i < device.Count; i++)
            {
                _context.CurrentDevices.Add(new CurrentDevice
                {
                    Campaign = campaign,
                    DeviceType = device.DeviceType,
                    Grade = device.Grade,
                    Location = device.Location
                });
            }
            return true;
        }

        
        
        
        var devicesToBeRemoved = _context.CurrentDevices
            .Where(x => x.DeviceType == device.DeviceType && x.Location == device.Location && x.Grade == device.Grade)
            //.OrderBy(i=>i.Campaign.CampaignId == campaign.CampaignId)
            //.ThenBy(i=>i)
            .Take(device.Count);

        
        if (devicesToBeRemoved.Count() < device.Count)
            return false;
        
        _context.CurrentDevices.RemoveRange(devicesToBeRemoved);
        return true;
    }
}