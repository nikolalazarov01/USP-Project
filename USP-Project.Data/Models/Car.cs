using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USP_Project.Data.Models;

public class Car
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [ForeignKey("Brand")]
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; }
    
    [Required]
    [ForeignKey("Model")]
    public Guid ModelId { get; set; }
    public Model Model { get; set; }

    private ICollection<Extra>? Extras { get; set; }
}