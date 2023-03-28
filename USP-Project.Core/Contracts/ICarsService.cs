using USP_Project.Data.Models;
using Usp_Project.Utils;

namespace USP_Project.Core.Contracts;

public interface ICarsService
{
    Task<OperationResult<Car>> CreateAsync(
        string brandName,
        string? brandDescription,
        string modelName,
        IEnumerable<string> extras,
        CancellationToken cancellationToken = default);

    Task<OperationResult<IEnumerable<Car>>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Model>> AllModels(CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Brand>> AllBrands(CancellationToken cancellationToken = default);
}