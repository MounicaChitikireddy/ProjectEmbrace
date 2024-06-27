using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using webapi.Interfaces;

[Route("api/Email")]
[ApiController]
public class EmailController : IEmailSender
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpGet("SendEmail")]
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        EmailMessageModel emailMessage = new(email,
        subject,
        htmlMessage);

        await _emailService.Send(emailMessage);
    }

    [HttpGet("SendEmailWithAttachment")]
    public async Task SendEmailAsyncWithAttachment(string email, string subject, string htmlMessage, string? attachmentPath)
    {
        EmailMessageModel emailMessage = new(email,
        subject,
        htmlMessage);
        if (attachmentPath != null) { emailMessage.AttachmentPath = attachmentPath; }

        await _emailService.Send(emailMessage);
    }
}