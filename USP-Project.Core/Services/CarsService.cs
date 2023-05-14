using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using USP_Project.Core.Contracts;
using USP_Project.Data.Models;
using Usp_Project.Utils;
using USP_Project.Data.Contracts;
using USP_Project.Data.Models.Enums;

namespace USP_Project.Core.Services;

public class CarsService : ICarsService
{
    private readonly IRepository<Car> _cars;
    private readonly IRepository<Extra> _extras;
    private readonly IRepository<Brand> _brands;
    private readonly IRepository<Model> _models;

    public CarsService(
        IRepository<Car> cars,
        IRepository<Brand> brands,
        IRepository<Model> models,
        IRepository<Extra> extras)
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
        int? productionYear,
        EngineType engineType,
        Transmission transmission,
        decimal? engineSize,
        IEnumerable<string> imageFileNames,
        IEnumerable<string> extras,
        CancellationToken cancellationToken = default)
    {
        var car = new Car
        {
            ImagePaths = imageFileNames.ToArray(),
            Engine = engineType,
            ProductionYear = productionYear,
            EngineSize = engineSize,
            Transmission = transmission
        };

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

        foreach (var extra in extras)
        {
            var extraResult = await _extras.GetAsync(new Expression<Func<Extra, bool>>[]
            {
                e => e.Name == extra
            }, null!, default);
            var extraEntity = extraResult.Data;
            
            if (!extraResult.IsSuccessfull || extraEntity is null)
            {
                extraEntity = new Extra { Name = extra };
                await _extras.CreateAsync(extraEntity, cancellationToken);
            }

            var carExtra = new CarsExtras
            {
                Car = car,
                Extra = extraEntity
            };
            car.CarsExtras.Add(carExtra);
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

    public async Task<OperationResult<IEnumerable<Car>>> SearchAsync(
        string brandQuery,
        string modelQuery,
        int? productionYear,
        decimal? engineSize,
        EngineType? engineType,
        Transmission? transmission,
        CancellationToken cancellationToken = default)
    {
        var filters = new List<Expression<Func<Car, bool>>>();
        
        if(!string.IsNullOrEmpty(brandQuery)) filters.Add(c => c.Brand.Name == brandQuery);
        if(!string.IsNullOrEmpty(modelQuery)) filters.Add(c => c.Model.Name == modelQuery);
        
        if(engineSize is not null) filters.Add(c => c.EngineSize == engineSize);
        if (engineType is not null) filters.Add(c => c.Engine == engineType);
        if (transmission is not null) filters.Add(c => c.Transmission == transmission);
        if (productionYear is not null) filters.Add(c => c.ProductionYear == productionYear);
            
        var searchResult  = await _cars.GetManyAsync(
            filters,
            new Expression<Func<Car, object>>[] { c => c.Brand, c => c.Model },
            default!, cancellationToken);
        
        return new OperationResult<IEnumerable<Car>> { Data = searchResult.Data };
    }

    public async Task<OperationResult<IEnumerable<Model>>> AllModels(CancellationToken cancellationToken = default)
    {
        var result = await _models.GetManyAsync(default!, default!, default!, cancellationToken);
        return result;
    }

    public async Task<OperationResult<IEnumerable<Brand>>> AllBrands(CancellationToken cancellationToken = default)
    {
        var result = await _brands.GetManyAsync(default!, default!, default!, cancellationToken);
        return result;
    }

    public async Task<OperationResult<IEnumerable<Extra>>> AllExtras(CancellationToken cancellationToken = default)
    {
        var result = await _extras.GetManyAsync(default!, default!, default!, cancellationToken);
        return result;
    }

    public async Task<OperationResult<IEnumerable<Model>>> ModelsByBrand(
        Guid brandId,
        CancellationToken cancellationToken = default)
    {
        var models = await _models
            .GetManyAsync(
                new Expression<Func<Model, bool>>[]
                {
                    m => m.BrandId == brandId
                },
                new Expression<Func<Model, object>>[]{ m => m.Brand },
                default!,
                cancellationToken);

        return models;
    }
}