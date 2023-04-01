using Microsoft.AspNetCore.Http;
using Usp_Project.Utils;

namespace USP_Project.Core.Contracts;

public interface IFileService
{
    Task<OperationResult<string[]>> Upload(IFormFile[]? filesToUpload, string filePath);
}