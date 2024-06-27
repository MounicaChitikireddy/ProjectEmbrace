using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Receipt
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceiptId { get; set; }
        public string? DonorOrganization { get; set; }
        public string? Location { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
