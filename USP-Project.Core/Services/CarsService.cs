using System.Linq.Expressions;
using USP_Project.Core.Contracts;
using USP_Project.Data.Models;
using USP_Project.Data.Repository;
using Usp_Project.Utils;

namespace USP_Project.Core.Services;

public class CarsService : ICarsService
{
    private readonly Repository<Car> _cars;
    private readonly Repository<Extra> _extras;
    private readonly Repository<Brand> _brands;
    private readonly Repository<Model> _models;

    public CarsService(
        Repository<Car> cars,
        Repository<Brand> brands,
        Repository<Model> models,
        Repository<Extra> extras)
    {
        _cars = cars;
        _brands = brands;
        _models = models;
        _extras = extras;
    }

    public async Task<OperationResult<Car>> CreateAsync(
        string brandName,
        string? brandDescription,
        string modelName,
        IEnumerable<string> extras,
        CancellationToken cancellationToken = default)
    {
        var car = new Car();
        
        var brandResult = await _brands.GetAsync(new Expression<Func<Brand, bool>>[]
        {
            b => b.Name == brandName
        }, null!, default);
        var brand = brandResult.Data;
        
        if (!brandResult.IsSuccessfull)
        {
            brand = new Brand
            {
                Name = brandName,
                Description = brandDescription
            };
            
            await _brands.CreateAsync(brand, cancellationToken);
        }
        car.Brand = brand;
        
        var modelResult = await _models.GetAsync(new Expression<Func<Model, bool>>[]
        {
            m => m.Name == modelName
        }, null!, default);
        var model = modelResult.Data;
        
        if (!modelResult.IsSuccessfull)
        {
            model = new Model
            {
                Name = brandName
            };
            
            await _models.CreateAsync(model, cancellationToken);
        }
        car.Model = model;

        car.Extras ??= new List<Extra>();
        foreach (var extra in extras)
        {
            var extraResult = await _extras.GetAsync(new Expression<Func<Extra, bool>>[]
            {
                e => e.Name == extra
            }, null!, default);
            var extraEntity = extraResult.Data;
            
            if (!extraResult.IsSuccessfull)
            {
                extraEntity = new Extra { Name = extra };
                
                await _extras.CreateAsync(extraEntity, cancellationToken);
            }
            car.Extras.Add(extraEntity);
        }

        var operationResult = new OperationResult<Car>();
        var result = await _cars.CreateAsync(car, cancellationToken);
        if (!result.IsSuccessfull)
        {
            operationResult.AppendErrors(result);
        }
        else
        {
            operationResult.Data = car;
        }

        return operationResult;
    }
}