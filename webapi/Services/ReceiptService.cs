using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using webapi.Data;
using webapi.Interfaces;
using webapi.Models;

public class ReceiptService : IReceiptService
{
    private readonly webapiContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public ReceiptService(webapiContext context, UserManager<User> userManager, IEmailService emailService)
    {
        _context = context;
        _userManager = userManager;
        _emailService = emailService;
    }

    public QuestPDF.Fluent.Document CreateReceipt(int batchId, User approvedBy, string Organization, string? comments)
    {
        var query = from cbd in _context.ChangelogBatchDevices
                    join cb in _context.ChangelogBatches on cbd.Batch.Id equals cb.Id
                    join dt in _context.DeviceTypes on cbd.DeviceType.DeviceTypeId equals dt.DeviceTypeId
                    where cb.Id == batchId
                    group cbd by new { cbd.Batch.Id, cbd.Location, cbd.Grade, dt.Category, dt.Size, dt.Type } into grouped
                    select new
                    {
                        Id = grouped.Key.Id,
                        Location = grouped.Key.Location,
                        Grade = grouped.Key.Grade,
                        Category = grouped.Key.Category,
                        Dtype = grouped.Key.Type,
                        Size = grouped.Key.Size,
                        DeviceCount = grouped.Count()
                    };
        var deviceList = query.ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Inch);
                page.DefaultTextStyle(TextStyle.Default.FontSize(16));
                page.PageColor(Colors.White);


                page.Header()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Receipt of Donation").Bold().FontSize(36).FontFamily(Fonts.Arial);
                        text.Span("www.projectembrace.org - 385.351.9101\r\nEIN: 82-1814956\r\n").FontSize(16).FontFamily(Fonts.Arial);
                        text.AlignCenter();
                    });

                page.Content()
                .Table(table =>
                {
                    int total = 0;
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(100);
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                        columns.RelativeColumn();
                    });

                    table.ExtendLastCellsToTableBottom();

                    table.Cell().RowSpan(1).LabelCell("Donor Org.");
                    table.Cell().RowSpan(1).ShowEntire().ValueCell(Organization);

                    table.Cell().LabelCell("Donation No.");
                    table.Cell().ValueCell(batchId.ToString());

                    table.Cell().LabelCell("Date");
                    table.Cell().ValueCell(DateTime.Now.ToString());

                    table.Cell().LabelCell("Employee");
                    table.Cell().ValueCell(approvedBy.FullName);

                    table.Cell().ColumnSpan(3).LabelCell("Device Name");
                    table.Cell().ColumnSpan(1).LabelCell("Quantity");

                    foreach (var item in deviceList)
                    {
                        table.Cell().ColumnSpan(3).ValueCell($"{item.Category}, {item.Dtype}, {item.Size}");
                        table.Cell().ColumnSpan(1).ValueCell(item.DeviceCount.ToString());
                        total += item.DeviceCount;
                    }
                    table.Cell().ColumnSpan(3).LabelCell("Total Devices Donated: ");
                    table.Cell().ColumnSpan(1).LabelCell(total.ToString());
                    if (!comments.IsNullOrEmpty())
                    {
                        table.Cell().ColumnSpan(4).ValueCell($"Additional Comments: \r\n{comments}");
                    }



                    page.Footer().AlignBottom().Dynamic(new FooterWithUniqueLastPage());
                    using var stream = new FileStream("Resources\\Logo.png", FileMode.Open);
                    page.Background().Width(125).Height(325).PaddingTop(50).PaddingLeft(40).Image(stream);

                    
                });
            });
        });

        

        return document;
    }

    public class FooterWithUniqueLastPage : IDynamicComponent<int>
    {
        public int State { get; set; }

        public DynamicComponentComposeResult Compose(DynamicContext context)
        {
            var content = context.CreateElement(element =>
            {
                element
                    .Text(x =>
                    {
                        x.CurrentPageNumber().FontSize(12);
                        x.Span(" of ").FontSize(12);
                        x.TotalPages().FontSize(12);
                        x.AlignCenter();
                    });
            });

            return new DynamicComponentComposeResult
            {
                Content = content,
                HasMoreContent = false
            };
        }
    }

   
}
