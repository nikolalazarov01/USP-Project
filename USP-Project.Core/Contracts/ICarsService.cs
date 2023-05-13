using USP_Project.Data.Models;
using USP_Project.Data.Models.Enums;
using Usp_Project.Utils;

namespace USP_Project.Core.Contracts;

public interface ICarsService
{
    Task<OperationResult<Car>> CreateAsync(
        string brandName,
        string? brandDescription,
        string modelName,
        int? productionYear,
        EngineType engineType,
        Transmission transmission,
        decimal? engineSize,
        IEnumerable<string> imageFileNames,
        IEnumerable<string> extras,
        CancellationToken cancellationToken = default);

    Task<OperationResult<IEnumerable<Car>>> SearchAsync(
        string brandQuery,
        string modelQuery,
        int? productionYear,
        decimal? engineSize,
        EngineType? engineType,
        Transmission? transmission,
        CancellationToken cancellationToken = default);
    
    Task<OperationResult<IEnumerable<Model>>> AllModels(CancellationToken cancellationToken = default);
    
    Task<OperationResult<IEnumerable<Brand>>> AllBrands(CancellationToken cancellationToken = default);
    
    Task<OperationResult<IEnumerable<Extra>>> AllExtras(CancellationToken cancellationToken = default);
    
    Task<OperationResult<IEnumerable<Model>>> ModelsByBrand(Guid brandId, CancellationToken cancellationToken = default);
}