using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USP_Project.Data.Contracts;

namespace USP_Project.Data.Models;

public class Model : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    [Required]
    [ForeignKey(nameof(Brand))]
    public Guid BrandId { get; set; }
    
    public Brand Brand { get; set; }
    
    public ICollection<Car>? Cars { get; set; }
}