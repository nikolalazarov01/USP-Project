using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USP_Project.Core.Contracts;
using USP_Project.Web.Models.Cars;

namespace USP_Project.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarsService _carsService;

        private const string ErrorsKey = "Errors";

        public CarsController(ICarsService carsService)
        {
            _carsService = carsService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var createCarInputModel = new CreateCarInputModel
            {
                Brands = _carsService.GetAllBrands().Result.Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.Id.ToString()
                }),
                Models = _carsService.GetAllModels().Result.Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                })
            };

            return View(createCarInputModel);
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

            var result = await _carsService.CreateAsync(
                carInputModel.BrandId,
                carInputModel.ModelId,
                carInputModel.Extras);

            if (!result.Succeeded)
            {
                ViewData[ErrorsKey] = result.Errors;
                return RedirectToAction(nameof(Add));
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}