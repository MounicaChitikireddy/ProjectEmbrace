using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Interfaces
{
    public interface IReceiptService
    {
        QuestPDF.Fluent.Document CreateReceipt(int batchId, User approvedBy, string Organization, string? additionalComments);
    }
}
