using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly webapiContext _context;

        public CampaignsController(webapiContext context)
        {
            _context = context;
        }

        // GET: api/Campaigns
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin,Reviewer,Employee")]
        public async Task<ActionResult> GetCampaign()
        {
          if (_context.Campaigns == null)
          {
              return NotFound("Campaigns table is currently empty.");
          }
            return Ok(await _context.Campaigns
                .Select(x => new {x.CampaignId, x.CampaignName, x.StartDate, x.EndDate})
                .ToListAsync());
        }

        // GET: api/Campaigns/5
        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<ActionResult<Campaign>> GetCampaign(int id)
        {
          if (_context.Campaigns == null)
          {
              return NotFound("Campaigns table is currently empty.");
          }
            var campaign = await _context.Campaigns.FindAsync(id);

            if (campaign == null)
            {
                return NotFound("Could not find a campaign with that Id.");
            }

            return campaign;
        }

        // PUT: api/Campaigns/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Edit/{id}")]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> PutCampaign(int id, Campaign campaign)
        {
            if (id != campaign.CampaignId)
            {
                return BadRequest("Provided Id and updated campaign Id do not match.");
            }

            _context.Entry(campaign).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampaignExists(id))
                {
                    return NotFound("There are no campaigns with that Id.");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Campaign updated successfully.");
        }

        // POST: api/Campaigns
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create")]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<ActionResult<Campaign>> PostCampaign(Campaign campaign)
        {
            if (_context.Campaigns == null)
            {
                return Problem("Entity set 'webapiContext.Campaign'  is null.");
            }
            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return Ok("Campaign created successfully.");
        }

        [HttpPost("ListImport")]
        public async Task<ActionResult> PostListOfCampaign(Campaign[] campaignList)
        {
            if (_context.Campaigns == null)
            {
                return Problem("Entity set 'webapiContext.Campaign'  is null.");
            }
            foreach(Campaign campaign in campaignList) {
                _context.Campaigns.Add(campaign);
            }
            await _context.SaveChangesAsync();

            return Ok("Campaigns imported successfully.");
        }


        // DELETE: api/Campaigns/5
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            if (_context.Campaigns == null)
            {
                return BadRequest("Campaigns table is currently empty.");
            }
            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null)
            {
                return BadRequest("Could not find a campaign with that Id.");
            }

            var result = _context.Campaigns.Remove(campaign);
            await _context.SaveChangesAsync();

            return Ok("Campaign deleted successfully.");
        }

        [HttpDelete("DeleteMultiple")]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> DeleteCampaigns(int[] idArray)
        {
            if (_context.Campaigns == null)
            {
                return NotFound("The campaigns table is currently empty.");
            }
            foreach (int id in idArray)
            {
                var campaign = await _context.Campaigns.FindAsync(id);
                if (campaign == null)
                {
                    return NotFound("There was no campaign found with Id" + id);
                }
                var result = _context.Campaigns.Remove(campaign);
            }
            await _context.SaveChangesAsync();
            return Ok("All campaigns with those Id's were successfully deleted.");
        }

        private bool CampaignExists(int id)
        {
            return (_context.Campaigns?.Any(e => e.CampaignId == id)).GetValueOrDefault();
        }
    }
}
