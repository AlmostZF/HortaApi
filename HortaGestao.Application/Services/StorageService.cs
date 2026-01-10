using HortaGestao.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HortaGestao.Application.Services;

public class StorageService:IStorageService
{
    private readonly string _rootPath;
    private readonly string _baseUrl;
    
    public StorageService(IConfiguration configuration)
    {
        _rootPath = configuration["StorageConfig:Path"];
        _baseUrl = configuration["ApiConfig:BaseUrl"];
    }
    
    public async Task<string> SaveFileAsync(IFormFile file,  string containerName)
    {
        var targetPath = Path.Combine(_rootPath, containerName);
        
        if(!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);
        
        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid() + fileExtension;
        var filePath = Path.Combine(targetPath, fileName);

        await using (var FileStream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(FileStream);

        var virtualPath = $"/api/products/image/{fileName}";
        
        return virtualPath;
    }

    public async Task DeleteFileAsync(string fileName, string containerName)
    {
        var filePath = Path.Combine(_rootPath, containerName, fileName);
        if (File.Exists(filePath))
        {
            await Task.Run(() => File.Delete(filePath));
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file, string containerName)
    {
        if (file == null || file.Length == 0) return null;

        var targetDirectory = Path.Combine(_rootPath, containerName);
        if(!Directory.Exists(targetDirectory))
            Directory.CreateDirectory(targetDirectory);
        
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(targetDirectory, fileName);
        
        await using (var FileStream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(FileStream);

        var virtualPath = $"/api/products/image/{fileName}";

        return virtualPath;

    }

    public async Task<byte[]> GetFileAsync(string fileName, string containerName)
    {
        var filePath = Path.Combine(_rootPath, containerName, fileName);
        
        if (!System.IO.File.Exists(filePath))
        {
            return null;
        }
        
        return await System.IO.File.ReadAllBytesAsync(fileName);
    }
}