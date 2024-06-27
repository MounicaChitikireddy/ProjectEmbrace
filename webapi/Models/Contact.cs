using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{
    [PrimaryKey(nameof(ContactId))]
    public class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        [Required]
        [PersonalData]
        public string FullName { get; set; }

        [Required]
        [Phone]
        [PersonalData]
        public string? PhoneNum { get; set; }

        [Required]
        [EmailAddress]
        [PersonalData]
        public string? Email { get; set; }

        [Required]
        public string? Role { get; set; }
        
        public ICollection<ChangelogBatch>? ChangelogBatches { get; set; }

        public string Organization { get; set; } = null!;

    }
}
