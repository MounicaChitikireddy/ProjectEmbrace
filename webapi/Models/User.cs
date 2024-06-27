using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{
    public class User : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DisplayId { get; set; }

        public string? FullName { get; set; }
        
        public string? Title { get; set; }
        
        public string? Department { get; set; }

        //[Required]
        //public string Role { get; set; }

        //TODO: Add permissions based on how we want to represent them
        
        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }


        [InverseProperty("BatchCreatedBy")]
        public virtual ICollection<ChangelogBatch>? CreatedCurrentDeviceBatches { get; set; }
        
        [InverseProperty("BatchApprovedBy")]
        public List<ChangelogBatch>? ApprovedCurrentDeviceBatches { get; set; }

        [InverseProperty("User")]
        [NotMapped]
        public virtual UserSettings? UserSettings { get; set; }

        
    }
}
