using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using USP_Project.Core.Contracts;
using USP_Project.Web.Models.Cars;

namespace USP_Project.Web.Controllers;

public class CarsController : Controller
{
    private readonly ICarsService _carsService;
    
    private const string ErrorsKey = "Errors";

    public CarsController(ICarsService carsService)
        => _carsService = carsService;

    // TODO: Create a view with a form for creating a new Car:
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var model = new CreateCarInputModel
        {
            AllBrands = (await _carsService.AllBrands()).Select(b => b.Name).ToList(),
            AllModels = (await  _carsService.AllModels()).Select(m => m.Name).ToList(),
        };
        
        return View(model);
    }
    // => View(new CreateCarInputModel());

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] CreateCarInputModel carInputModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData[ErrorsKey] = ModelState
                .SelectMany(x =>
                    x.Value?.Errors
                        .Select(e => e.ErrorMessage)
                                 ?? Array.Empty<string>());
            return RedirectToAction(nameof(Add));
        }
        
        var result = await _carsService.CreateAsync(
            carInputModel.BrandName,
            carInputModel.BrandDescription,
            carInputModel.ModelName,
            carInputModel.Extras);

        if (!result.IsSuccessfull)
        {
            ViewData[ErrorsKey] = result.Errors;
            return RedirectToAction(nameof(Add));
        }

        // TODO: Potentially add a Success message to the View Data ...
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}