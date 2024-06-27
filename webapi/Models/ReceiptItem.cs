using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{ 

    public class ReceiptItem
    {
        public int ReceiptItemId { get; set; }
        public DeviceType DeviceType { get; set; } = null!;
        public int Quantity { get; set; }
        public Receipt Receipt { get; set; } = null!;
    }
}
