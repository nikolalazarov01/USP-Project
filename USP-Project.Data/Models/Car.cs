using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USP_Project.Data.Contracts;
using USP_Project.Data.Models.Enums;

namespace USP_Project.Data.Models;

public class Car : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [ForeignKey(nameof(Brand))]
    public Guid BrandId { get; set; }
    
    public Brand Brand { get; set; }
    
    [Required]
    [ForeignKey(nameof(Model))]
    public Guid ModelId { get; set; }
    
    public Model Model { get; set; }

    public ICollection<Extra>? Extras { get; set; }
    
    public List<CarsExtras> CarsExtras { get; set; }
    
    public EngineType Engine { get; set; }
    
    public Transmission Transmission { get; set; }
    
    public decimal? EngineSize { get; set; }

    public string[] ImagePaths { get; set; }
}