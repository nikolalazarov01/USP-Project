using System.ComponentModel.DataAnnotations.Schema;

namespace USP_Project.Data.Models;

public class CarsExtras
{
    [ForeignKey(nameof(Car))]
    public Guid CarId { get; set; }
    public Car Car { get; set; }
    
    [ForeignKey(nameof(Extra))]
    public Guid ExtraId { get; set; }
    public Extra Extra { get; set; }
}