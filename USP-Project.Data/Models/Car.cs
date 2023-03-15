using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USP_Project.Data.Contracts;

namespace USP_Project.Data.Models;

public class Car : IEntity
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

    public ICollection<Extra>? Extras { get; set; }
    public List<CarsExtras> CarsExtras { get; set; }
}