using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    
    public class UserSettings
    {

        public int UserSettingsId {  get; set; }

        [Column("UserId")]
        [Required]
        public User User { get; set; } = null!;

        public string TableDensity { get; set; } = null!;

        public string AddInventoryDefaultGrade { get; set; } = null!;

        public string RemoveInventoryDefaultGrade { get; set; } = null!;

        public bool TableFullScreenByDefault { get; set; }

        public string AvatarColor { get; set; } = null!;

        public string AvatarText { get; set; } = null!;
    }
}
