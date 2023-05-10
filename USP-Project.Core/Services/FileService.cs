using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using USP_Project.Core.Contracts;
using Usp_Project.Utils;

namespace USP_Project.Core.Services;

public class FileService : IFileService
{
    public async Task<OperationResult<string[]>> Upload(
        IFormFile[]? filesToUpload,
        string filePath)
    {
        var operationResult = new OperationResult<string[]>
        { 
            Data = Array.Empty<string>()
        };
        
        if (filesToUpload is null)
        {
            operationResult.AddError("The provided file(s) is / are null!");
            return operationResult;
        }

        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

        var fileUploadTasks = filesToUpload.Select(async f =>
        {
            var extension = Path.GetExtension(f.FileName);
            var fileName = Path.GetFileNameWithoutExtension(f.FileName);
            
            var fullFileName = $"{fileName}_{Guid.NewGuid().ToString()}{extension}";
            var fullPath = Path.Combine(filePath, fullFileName);

            await using var fileStream = File.Create(fullPath);
            await f.CopyToAsync(fileStream);
            return fullPath;
        }).ToList();

        try
        {
            var fileNames = await Task.WhenAll(fileUploadTasks);
            operationResult.Data = fileNames;
            return operationResult;
        }
        catch (Exception e)
        {
            operationResult.AddError(e.Message);
            operationResult.Data = Array.Empty<string>();
            
            return operationResult;
        }
    }
}