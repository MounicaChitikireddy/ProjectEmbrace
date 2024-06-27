using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models
{
    [PrimaryKey(nameof(CampaignId))]
    public class Campaign
    {
        public int CampaignId { get; set; }
        
        [Required]
        public string CampaignName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        

        
        public ICollection<ChangelogBatch>? ChangelogBatches { get; set; }

    }
}
