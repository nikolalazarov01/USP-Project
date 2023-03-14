using System.ComponentModel.DataAnnotations.Schema;

namespace USP_Project.Data.Models;

public class CarsExtras
{
    [ForeignKey("Car")]
    public Guid CarId { get; set; }
    public Car Car { get; set; }
    
    [ForeignKey("Extra")]
    public Guid ExtraId { get; set; }
    public Extra Extra { get; set; }
}