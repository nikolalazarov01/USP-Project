﻿using System.Linq.Expressions;
using USP_Project.Core.Contracts;
using USP_Project.Data.Models;
using Usp_Project.Utils;
using USP_Project.Data.Contracts;

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
        IEnumerable<string> imageFileNames,
        IEnumerable<string> extras,
        CancellationToken cancellationToken = default)
    {
        var car = new Car { ImagePaths = imageFileNames.ToArray() };

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

    public async Task<OperationResult<IEnumerable<Car>>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        var carsResult = await _cars
            .FuzzySearchAsync(new (Expression<Func<Car, string>>, string)[]
            {
                (c => c.Brand.Name, searchTerm)
            }, cancellationToken);

        return carsResult;
    }

    public async Task<IEnumerable<Model>> AllModels(CancellationToken cancellationToken = default)
    {
        var result = await _models.GetManyAsync(default!, default!, cancellationToken);
        return result.IsSuccessfull ? result.Data : Enumerable.Empty<Model>();
    }

    public async Task<IEnumerable<Brand>> AllBrands(CancellationToken cancellationToken = default)
    {
        var result = await _brands.GetManyAsync(default!, default!, cancellationToken);
        return result.IsSuccessfull ? result.Data : Enumerable.Empty<Brand>();
    }
}