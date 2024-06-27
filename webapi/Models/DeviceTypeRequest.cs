using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [NotMapped]
    public class DeviceTypeRequest
    {
        public int DeviceTypeId { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public string? Size { get; set; }
        public string? Description { get; set; }

    }
}
