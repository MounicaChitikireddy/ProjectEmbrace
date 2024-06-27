using FluentEmail.Core;
using FluentEmail.Core.Models;
using QuestPDF.Fluent;
using Microsoft.AspNetCore.Mvc;
using webapi.Interfaces;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IFluentEmailFactory _fluentEmailFactory;

    public EmailService(ILogger<EmailService> logger, IFluentEmailFactory fluentEmailFactory)
    {
        _logger = logger;
        _fluentEmailFactory = fluentEmailFactory;
    }

    public async Task Send(EmailMessageModel emailMessageModel)
    {
        try
        {
            _logger.LogInformation("Sending email");
            var result = _fluentEmailFactory.Create().To(emailMessageModel.ToAddress)
                    .Subject(emailMessageModel.Subject)
                    .Body(emailMessageModel.Body, true);
            if (emailMessageModel.AttachmentPath != null) 
            {
                string filePath = $"{emailMessageModel.AttachmentPath}";
                result.AttachFromFilename(filePath, "application/pdf", "Receipt");
            }
            await result.SendAsync();
        }
        catch (Exception ex) {
            throw ex;
        }
    }

    public async Task SendWithPdfAttachment(EmailMessageModel emailMessageModel, QuestPDF.Fluent.Document document)
    {
        try
        {
            _logger.LogInformation("Sending email with attachment");
            var result = _fluentEmailFactory.Create().To(emailMessageModel.ToAddress)
                    .Subject(emailMessageModel.Subject)
                    .Body(emailMessageModel.Body, true);
            if (document != null)
            {
                Attachment pdfAttachment = new Attachment();
                MemoryStream stream = new MemoryStream();
                
                document.GeneratePdf(stream);
                stream.Flush();
                stream.Position = 0;

                pdfAttachment.Data = stream;
                pdfAttachment.IsInline = false;
                pdfAttachment.Filename = "PE_batch_reciept.pdf";
                pdfAttachment.ContentId = Guid.NewGuid().ToString();
                pdfAttachment.ContentType = "application/pdf";
                result.Attach(pdfAttachment);
                await result.SendAsync();
                await stream.DisposeAsync();
            }
            
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }
}

public class EmailMessageModel
{
    public string ToAddress { get; set; }
    public string Subject { get; set; }
    public string? Body { get; set; }
    public string? AttachmentPath { get; set; }
    public EmailMessageModel(string toAddress, string subject, string? body = "")
    {
        ToAddress = toAddress;
        Subject = subject;
        Body = body;
    }
}