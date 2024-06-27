using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models;

public class AggregatedDevice
{
    public int Id { get; set; }
    public DeviceType DeviceType { get; set; }
    [ForeignKey(nameof(DeviceType))]
    public int DeviceTypeId {get; set; }
    public string Grade { get; set; }
    public int Count { get; set; }
    public string Location { get; set; }
    [Column(TypeName = "date")]
    public DateTime TimeStamp { get; set; }
}