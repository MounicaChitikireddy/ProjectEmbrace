using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{
    
    [Index(nameof(CategoryNormalized),nameof(TypeNormalized),nameof(SizeNormalized), IsUnique = true)]
    public class DeviceType
    {
        
        public int DeviceTypeId { get; set; }

        public string Category { get; set; } = null!;
        public string CategoryNormalized { get; set; } = null!;

        public string Type { get; set; } = null!;
        public string TypeNormalized {get; set; } = null!;

        public string Size { get; set; } = null!;
        public string SizeNormalized { get; set; } = null!;

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        public ICollection<CurrentDevice>? CurrentDevices {get; set; }

        public ICollection<ChangelogBatchDevice>? CurrentDeviceHistories { get; set; }
        
        public ICollection<AggregatedDevice>? AggregatedDevices { get; set; }
    }
}
