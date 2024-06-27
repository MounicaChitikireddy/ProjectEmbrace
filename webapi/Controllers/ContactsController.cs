using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly webapiContext _context;

        public ContactsController(webapiContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet("GetContacts")]
        //[Authorize(Roles = "Admin,Reviewer,Employee")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContact()
        {
          if (_context.Contacts == null)
          {
              return NotFound("The contacts table is currently empty.");
          }
            return await _context.Contacts.ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,Reviewer,Employee")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
          if (_context.Contacts == null)
          {
              return NotFound("The contacts table is currently empty.");
          }
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound("Could not find a contact with Id " + id);
            }

            return contact;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Edit/{id}")]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.ContactId)
            {
                return BadRequest("Provided Id does not match Id of the provided contact.");
            }
            try
            {
                var oldContact = await _context.Contacts.FindAsync(id);
                if (oldContact is not null)
                {
                    oldContact.FullName = contact.FullName;
                    oldContact.PhoneNum = contact.PhoneNum;
                    oldContact.Email = contact.Email;
                    oldContact.Organization = contact.Organization;
                    oldContact.Role = contact.Role;
                }
                var result = await _context.SaveChangesAsync();
                return Ok("Contact updated successfully!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound("A contact with that Id does not exist.");
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
          if (_context.Contacts == null)
          {
              return Problem("Entity set 'webapiContext.Contact'  is null.");
          }
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return Ok("Contact created successfully!");
        }

        // POST: api/Contacts
        [HttpPost("ListImport")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PostListOfContact(Contact[] contactList)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'webapiContext.Contact'  is null.");
            }

            foreach (Contact contact in contactList)
            {
                _context.Contacts.Add(contact);
            }

            await _context.SaveChangesAsync();

            return Ok("Contacts imported successfully!");
        }


        // DELETE: api/Contacts/5
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            if (_context.Contacts == null)
            {
                return NotFound("Contacts table is currently empty.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound("Could not find a contact with the provided Id.");
            }

            _context.Contacts.Remove(contact);
            var result = await _context.SaveChangesAsync();

            return Ok("Contact deleted successfully!");
        }

        [HttpDelete("DeleteMultiple")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteContacts(int[] idArray)
        {
            if (_context.Contacts == null)
            {
                return NotFound("The contact table is currently empty.");
            }
            foreach (int id in idArray)
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    return NotFound("There was no contact found with Id" + id);
                }
                _context.Contacts.Remove(contact);
                var result = await _context.SaveChangesAsync();
            }
            return Ok("All contacts with those Id's were successfully deleted.");
        }

        private bool ContactExists(int id)
        {
            return (_context.Contacts?.Any(e => e.ContactId == id)).GetValueOrDefault();
        }
    }
}
