using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{
    [PrimaryKey(nameof(Id))]
    public class CurrentDevice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DeviceType DeviceType { get; set; }
        

        [Required]
        public string Grade { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        public Campaign? Campaign { get; set; }

    }
}
