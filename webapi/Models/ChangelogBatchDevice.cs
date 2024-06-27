using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{

    [PrimaryKey(nameof(Id))]
    public class ChangelogBatchDevice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public virtual DeviceType DeviceType { get; set; } = null!;

        public string Location { get; set; }

        public string Grade { get; set; } = null!;
        
        
        public ChangelogBatch Batch { get; set; }


    }
}
