using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USP_Project.Core.Contracts;
using USP_Project.Web.Models;
using USP_Project.Web.Models.Brands;
using USP_Project.Web.Models.Cars;

namespace USP_Project.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICarsService _carsService;

    public HomeController(
        ILogger<HomeController> logger,
        ICarsService carsService)
    {
        _logger = logger;
        _carsService = carsService;
    }

    public async Task<IActionResult> Index()
    {
        var carSearchModel = new CarSearchInputModel
        {
            AllBrands = (await _carsService.AllBrands())
                .Data
                .Select(b => new BrandViewModel
                {
                    Id = b.Id.ToString(),
                    Name = b.Name
                }).ToList(),
            AllExtras = (await _carsService.AllExtras())
                .Data
                .Select(b => new ExtraViewModel
                {
                    Id = b.Id.ToString(),
                    Name = b.Name
                }).ToList()
        };

        return View(carSearchModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    [Authorize]
    public IActionResult Secret()
    {
        var user = HttpContext.User;
        return Ok("This is a secret endpoint!");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}