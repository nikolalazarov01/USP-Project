using System.ComponentModel.DataAnnotations;
using USP_Project.Data.Contracts;

namespace USP_Project.Data.Models;

public class Model : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public ICollection<Car>? Cars { get; set; }
}