using Microsoft.AspNetCore.Http;
using Usp_Project.Utils;

namespace USP_Project.Core.Contracts;

public interface IFileService
{
    Task<OperationResult> Upload(IFormFile? fileToUpload, string filePath);
}