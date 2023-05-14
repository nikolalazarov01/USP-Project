﻿using Microsoft.AspNetCore.Mvc;
using USP_Project.Core.Contracts;
using USP_Project.Web.Models.Cars;
using USP_Project.Web.Models.Models;

namespace USP_Project.Web.Controllers;

public class CarsController : Controller
{
    private readonly ICarsService _carsService;
    private readonly IFileService _fileService;
    private readonly IWebHostEnvironment _hostingEnv;

    private const string ErrorsKey = "Errors";

    public CarsController(
        ICarsService carsService,
        IFileService fileService,
        IWebHostEnvironment hostingEnv)
    {
        _carsService = carsService;
        _fileService = fileService;
        _hostingEnv = hostingEnv;
    } 

    // TODO: Create a view with a form for creating a new Car:
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var allBrands = await _carsService.AllBrands();
        var allModels = await _carsService.AllModels();
        
        var model = new CreateCarInputModel
        {
            AllBrands = allBrands.Data.Select(b => b.Name).ToList(),
            AllModels = allModels.Data.Select(m => m.Name).ToList()
        };
        
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Models([FromQuery] Guid brandId)
    {
        var result = await _carsService.ModelsByBrand(brandId);
        if (!result.IsSuccessfull) return NotFound();

        var models = result
            .Data
            .Select(m => new ModelViewModel
            {
                Id = m.Id.ToString(),
                Name = m.Name
            });
        return Ok(models);
    }
    
    [HttpGet]
    [Route(nameof(Search))]
    public async Task<IActionResult> Search([FromForm] CarSearchInputModel searchInputModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData[ErrorsKey] = ModelState.SelectMany(x =>
                x.Value?.Errors.Select(e => e.ErrorMessage)
                ?? Array.Empty<string>());
            
            return RedirectToAction(nameof(Index), "Home");
        }
        
        var searchResult = await _carsService.SearchAsync(
            searchInputModel.Brand ?? string.Empty,
            searchInputModel.Model ?? string.Empty,
            searchInputModel.YearOfProduction,
            searchInputModel.MinEngineSize,
            searchInputModel.EngineType,
            searchInputModel.Transmission);

        // TODO: Add proper view models to be consumed by the client ...
        searchInputModel.FeaturedCars = searchResult.Data.ToList();
        
        return searchResult.IsSuccessfull
            ? Ok(searchResult.Data)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] CreateCarInputModel carInputModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData[ErrorsKey] = ModelState.SelectMany(x =>
                    x.Value?.Errors
                        .Select(e => e.ErrorMessage)
                                 ?? Array.Empty<string>())
                .ToList();
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
            carInputModel.ProductionYear,
            carInputModel.EngineType,
            carInputModel.Transmission,
            carInputModel.EngineSize,
            imagesUploadResult.Data.Select(f => f.Replace(_hostingEnv.WebRootPath, string.Empty)),
            carInputModel.Extras);

        if (!result.IsSuccessfull)
        {
            ViewData[ErrorsKey] = result.Errors;
            return RedirectToAction(nameof(Add));
        }

        
        // TODO: Potentially add a Success message to the View Data ...
        var car = result.Data;
        ViewData["Message"] = $"Car {car.Brand.Name} {car.Model.Name} was successfully added!";
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}