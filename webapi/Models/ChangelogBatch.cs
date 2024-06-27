using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

public enum Action
{
    Addition = 0,
    Removal = 1
}

public enum ReviewStatus
{
    Pending = 0, Approved = 1, Denied = 2
}
public class ChangelogBatch
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [InverseProperty("CreatedCurrentDeviceBatches")]
    public virtual User BatchCreatedBy { get; set; } = null!;

    [Required]
    public DateTime CreatedOn { get; set; }
    
    public DateTime? ApprovedOn { get; set; }

    [InverseProperty("ApprovedCurrentDeviceBatches")]
    public virtual User? BatchApprovedBy { get; set; }

    public List<Contact>? Contacts { get; set; } 

    public Campaign? Campaign { get; set; }
    
    [Column(TypeName = "text")]
    public string? AdditionalInformation { get; set; }

    [Required]
    public ReviewStatus ReviewStatus { get; set; }

    [Required]
    public Action Action { get; set; }

    public virtual List<ChangelogBatchDevice>? Updates { get; set; }
}