using Microsoft.AspNetCore.Http;
using USP_Project.Core.Contracts;
using Usp_Project.Utils;

namespace USP_Project.Core.Services;

public class FileService : IFileService
{
    public async Task<OperationResult> Upload(IFormFile? fileToUpload, string filePath)
    {
        var operationResult = new OperationResult();
        
        if (fileToUpload is null)
        {
            operationResult.AddError("The provided file is null!");
            return operationResult;
        }

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        
        var fileName = fileToUpload.FileName;
        var fullPath = Path.Combine(filePath, fileName);


        await using FileStream fs = System.IO.File.Create(fullPath);
        await fileToUpload.CopyToAsync(fs);

        return operationResult;
    }
}