using System.Collections;
using System.ComponentModel.DataAnnotations;
using USP_Project.Data.Models;
using USP_Project.Data.Models.Enums;
using USP_Project.Web.Models.Brands;

namespace USP_Project.Web.Models.Cars;

public class CarSearchInputModel
{
    private const int MinYear = 1970;
    
    private const int MaxYear = 2023;

    public ICollection<BrandViewModel> AllBrands { get; set; } = new List<BrandViewModel>();
    
    public ICollection<ExtraViewModel> AllExtras { get; set; } = new List<ExtraViewModel>();
    
    [MinLength(2, ErrorMessage = "Brand search term must be more than {1} characters long.")]
    public string? Brand { get; set; } = string.Empty;
    
    [MinLength(2, ErrorMessage = "Model search term must be more than {1} characters long.")]
    public string? Model { get; set; } = string.Empty;
    
    [Range(MinYear, MaxYear, ErrorMessage = "The year must be between {1} and {2}!")]
    public int YearOfProduction { get; set; }

    public ICollection<string> Extras { get; set; } = new List<string>();
    
    public decimal? MinEngineSize { get; set; }
    
    public EngineType EngineType { get; set; }
    
    public Transmission Transmission { get; set; }

    public ICollection<Car> FeaturedCars { get; set; } = new List<Car>();
}