using Microsoft.Build.Framework;

namespace USP_Project.Web.Models.Brands;

public class BrandViewModel
{
    [Required]
    public string Id { get; set; } = default!;
    
    [Required]
    public string Name { get; set; } = default!;
}