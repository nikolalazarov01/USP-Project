using System.ComponentModel.DataAnnotations;

namespace USP_Project.Data.Models;

public class Model
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public ICollection<Car> Cars { get; set; }
}