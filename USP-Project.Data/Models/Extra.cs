using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace USP_Project.Data.Models;

public class Extra
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public ICollection<Car>? Cars { get; set; }
    public List<CarsExtras> CarsExtras { get; set; }
}