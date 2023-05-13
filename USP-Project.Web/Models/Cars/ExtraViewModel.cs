using System.ComponentModel.DataAnnotations;

namespace USP_Project.Web.Models.Cars;

public class ExtraViewModel
{
    [Required]
    public string Id { get; set; } = default!;
    
    [Required]
    public string Name { get; set; } = default!;
}