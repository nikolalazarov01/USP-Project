using System.ComponentModel.DataAnnotations;

namespace USP_Project.Data.Models;

public class Brand
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string? Description { get; set; } 
    
    public ICollection<Car>? Cars { get; set; }
}