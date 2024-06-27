using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using webapi.Data;
using webapi.Interfaces;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly webapiContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IReceiptService _receiptService;

        public class ReceiptRequest
        {
            public int batchId { get; set; }
            public int[]? contactIds { get; set; }
            public string? additionalComments { get; set; }
            public bool? signatureRequired { get; set; }
        }

        public ReceiptController(webapiContext context, UserManager<User> userManager, IEmailService emailService, IReceiptService receiptService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _receiptService = receiptService;
        }

        [HttpPost("SendReceipt")]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<ActionResult> SendReceipt([FromBody]ReceiptRequest request)
        {
            try
            {
                var contactsQuery = (from b in _context.ChangelogBatches
                                     where b.Id == request.batchId
                                     select b.Contacts).First();

                var approvedBy = (from b in _context.ChangelogBatches
                                  where b.Id == request.batchId
                                  select b.BatchCreatedBy).First();

                var contactsList = contactsQuery.ToList();
                var organization = contactsQuery[0].Organization;
                var document = _receiptService.CreateReceipt(request.batchId, approvedBy, organization, request.additionalComments);

                string email = "ProjectEmbraceTemporary@gmail.com";
                string subject = "Project Embrace Receipt";
                string htmlMessage = "Thank you for your donation! Here is your receipt.";
                EmailMessageModel emailMessage = new(email,
                subject,
                htmlMessage);

                if (!request.contactIds.IsNullOrEmpty())
                {
                    contactsList = (from contacts in _context.Contacts
                                    where request.contactIds.Contains(contacts.ContactId)
                                    select contacts).ToList();

                    foreach (Contact contact in contactsList)
                    {
                        email = contact.Email;
                        emailMessage = new(email,
                        subject,
                        htmlMessage);
                        await _emailService.SendWithPdfAttachment(emailMessage, document);
                    }
                }
                else
                {
                    foreach (Contact contact in contactsQuery)
                    {
                        email = contact.Email;
                        emailMessage = new(email,
                        subject,
                        htmlMessage);
                        await _emailService.SendWithPdfAttachment(emailMessage, document);
                    }
                }

                return Ok("Receipt has been sent.");
            }
            catch
            {
                return BadRequest("Something went wrong while generating the receipt. Please try again.");
            }
        }
    }
}

static class SimpleExtension
{
    private static IContainer Cell(this IContainer container, bool dark)
    {
        return container
            .Border(1)
            .Background(dark ? Colors.Grey.Lighten2 : Colors.White)
            .Padding(5);
    }
    public static void LabelCell(this IContainer container, string text) => container.Cell(true).Text(text).Medium().Bold();
    public static void ValueCell(this IContainer container, string text) => container.Cell(false).Text(text).Medium();
}
