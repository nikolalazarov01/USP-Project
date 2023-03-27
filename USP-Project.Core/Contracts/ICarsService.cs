using USP_Project.Data.Models;
using Usp_Project.Utils;

namespace USP_Project.Core.Contracts
{
    public interface ICarsService
    {
        Task<OperationResult<Car>> CreateAsync(
            Guid brandId,
            Guid modelId,
            IEnumerable<string> extras,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Brand>> GetAllBrands(CancellationToken cancellationToken = default);
        Task<IEnumerable<Model>> GetAllModels(CancellationToken cancellationToken = default);
        Task<IEnumerable<Model>> GetAllExtras(CancellationToken cancellationToken = default);
        Task<OperationResult<IEnumerable<Car>>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
        
    }
}