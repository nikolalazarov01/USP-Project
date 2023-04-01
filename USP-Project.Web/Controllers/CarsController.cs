using Microsoft.AspNetCore.Mvc;
using USP_Project.Core.Contracts;
using USP_Project.Web.Models.Cars;

namespace USP_Project.Web.Controllers;

public class CarsController : Controller
{
    private readonly ICarsService _carsService;
    private readonly IFileService _fileService;
    private readonly IWebHostEnvironment _hostingEnv;
    
    private const string ErrorsKey = "Errors";

    public CarsController(ICarsService carsService, IFileService fileService, IWebHostEnvironment hostingEnv)
    {
        _carsService = carsService;
        _fileService = fileService;
        _hostingEnv = hostingEnv;
    } 

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
        
        const string fileDic = "Images";
        var filePath = Path.Combine(_hostingEnv.WebRootPath, fileDic);
        
        var imagesUploadResult = await _fileService.Upload(carInputModel.Images, filePath);
        if (!imagesUploadResult.IsSuccessfull)
        {
            ViewData[ErrorsKey] = imagesUploadResult.Errors;
            return RedirectToAction(nameof(Add));
        }
        
        var result = await _carsService.CreateAsync(
            carInputModel.BrandName,
            carInputModel.BrandDescription,
            carInputModel.ModelName,
            imagesUploadResult.Data,
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