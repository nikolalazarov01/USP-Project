using System.ComponentModel.DataAnnotations;

namespace USP_Project.Web.Models.Cars;

public class CreateCarInputModel
{
    [Required]
    [MinLength(3, ErrorMessage = "Brand name must have at least 3 characters.")]
    [MaxLength(50, ErrorMessage = "Brand name should not have more than 50 characters.")]
    public string BrandName { get; set; } = default!;

    public ICollection<string> AllBrands { get; set; } = default!;
    
    [MinLength(3, ErrorMessage = "Brand description must have at least 3 characters.")]
    [MaxLength(50, ErrorMessage = "Brand description should not have more than 50 characters.")]
    public string? BrandDescription { get; set; } = default!;
    
    [MinLength(3, ErrorMessage = "Model name must have at least 3 characters.")]
    [MaxLength(200, ErrorMessage = "Model name should not have more than 200 characters.")]
    public string ModelName { get; set; } = default!;
    
    public ICollection<string> AllModels { get; set; } = default!;

    public ICollection<string> Extras { get; set; } = new List<string>();
}