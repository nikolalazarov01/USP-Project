using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USP_Project.Core.Contracts;
using USP_Project.Data.Models;
using USP_Project.Web.Models;
using USP_Project.Web.Models.Brands;
using USP_Project.Web.Models.Cars;

namespace USP_Project.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICarsService _carsService;
    
    private const string ErrorsKey = "Errors";

    public HomeController(
        ILogger<HomeController> logger,
        ICarsService carsService)
    {
        _logger = logger;
        _carsService = carsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var brandsAndExtras = await GetAllBrandsAndExtras();

        var allCars = await _carsService
            .SearchAsync(string.Empty, string.Empty);
        
        var carSearchModel = new CarSearchInputModel
        {
            AllBrands = brandsAndExtras.brands,
            AllExtras = brandsAndExtras.extras, 
            FeaturedCars = allCars.Data.ToList()
        };

        return View(carSearchModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromForm] CarSearchInputModel searchInputModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData[ErrorsKey] = ModelState.SelectMany(x =>
                x.Value?.Errors.Select(e => e.ErrorMessage)
                ?? Array.Empty<string>());
            
            return View("Index", searchInputModel);
        }
        
        var searchResult = await _carsService.SearchAsync(
            searchInputModel.Brand ?? string.Empty,
            searchInputModel.Model ?? string.Empty,
            searchInputModel.YearOfProduction,
            searchInputModel.MinEngineSize,
            searchInputModel.EngineType,
            searchInputModel.Transmission);

        // TODO: Add proper view models to be consumed by the client ...
        if (!searchResult.IsSuccessfull)
        {
            ViewData[ErrorsKey] = searchResult.Errors;
            searchInputModel.FeaturedCars = new List<Car>();
        }
        else
        {
            searchInputModel.FeaturedCars = searchResult.Data.ToList();
        }
        
        var (brands, extras) = await GetAllBrandsAndExtras();
        
        searchInputModel.AllBrands = brands;
        searchInputModel.AllExtras = extras;
        
        return View("Index", searchInputModel);
    }

    private async Task<(ICollection<BrandViewModel> brands, ICollection<ExtraViewModel> extras)> GetAllBrandsAndExtras()
    {
        var allBrands = (await _carsService.AllBrands())
            .Data
            .Select(b => new BrandViewModel
            {
                Id = b.Id.ToString(),
                Name = b.Name
            }).ToList();
        var allExtras = (await _carsService.AllExtras())
            .Data
            .Select(b => new ExtraViewModel
            {
                Id = b.Id.ToString(),
                Name = b.Name
            }).ToList();

        return (allBrands, allExtras);
    }

    public IActionResult Privacy()
        => View();

    [HttpGet]
    [Authorize]
    public IActionResult Secret()
        => Ok("This is a secret endpoint!");


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}